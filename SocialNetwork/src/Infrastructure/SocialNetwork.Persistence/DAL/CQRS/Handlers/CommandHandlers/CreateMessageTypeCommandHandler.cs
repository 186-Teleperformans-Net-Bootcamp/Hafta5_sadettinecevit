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
    public class CreateMessageTypeCommandHandler : IRequestHandler<CreateMessageTypeCommandRequest, CreateMessageTypeCommandResponse>
    {
        private readonly IMessageTypeRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public CreateMessageTypeCommandHandler(IDistributedCache distributedCache, MessageTypeRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<CreateMessageTypeCommandResponse> Handle(CreateMessageTypeCommandRequest createMessageTypeCommandRequest, CancellationToken cancellationToken)
        {
            CreateMessageTypeCommandResponse createMessageTypeCommandResponse = new CreateMessageTypeCommandResponse();

            EntityEntry<MessageType> result = _repo.Add(
                new MessageType
                {
                    Type = createMessageTypeCommandRequest.Type
                }).Result;

            createMessageTypeCommandResponse.IsSuccess = result.State == EntityState.Added;
            createMessageTypeCommandResponse.MessageType = result.Entity;

            if(createMessageTypeCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messageTypes");
            }

            return createMessageTypeCommandResponse;
        }
    }
}
