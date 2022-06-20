using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteMessageTypeCommandHandler : IRequestHandler<DeleteMessageTypeCommandRequest, DeleteMessageTypeCommandResponse>
    {
        private readonly IMessageTypeRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteMessageTypeCommandHandler(MessageTypeRepository repo, IDistributedCache distributedCache)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteMessageTypeCommandResponse> Handle(DeleteMessageTypeCommandRequest deleteMessageTypeCommandRequest, CancellationToken cancellationToken)
        {
            DeleteMessageTypeCommandResponse deleteMessageTypeCommandResponse = new DeleteMessageTypeCommandResponse();

            MessageType messageType = _repo.GetAsync().Result.FirstOrDefault<MessageType>(m=>m.Id == deleteMessageTypeCommandRequest.Id);
            EntityEntry<MessageType> result = await _repo.Delete(messageType);
            deleteMessageTypeCommandResponse.IsSuccess = result.State == EntityState.Deleted;

            if (deleteMessageTypeCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messageTypes");
            }

            return deleteMessageTypeCommandResponse;
        }
    }
}
