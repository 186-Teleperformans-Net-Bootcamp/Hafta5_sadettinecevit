using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommandRequest, DeleteMessageCommandResponse>
    {
        private readonly IMessageRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteMessageCommandHandler(IDistributedCache distributedCache, MessageRepository repo)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteMessageCommandResponse> Handle(DeleteMessageCommandRequest deleteMessageCommandRequest, CancellationToken cancellationToken)
        {
            Message message = _repo.GetAsync().Result.FirstOrDefault<Message>(m=>m.Id == deleteMessageCommandRequest.Id);
            EntityEntry<Message> result = _repo.Delete(message).Result;
           
            DeleteMessageCommandResponse deleteMessageCommandResponse = new DeleteMessageCommandResponse()
            {
                IsSuccess = result.State == EntityState.Deleted
            };

            if(deleteMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messages");
            }

            return deleteMessageCommandResponse;
        }
    }
}
