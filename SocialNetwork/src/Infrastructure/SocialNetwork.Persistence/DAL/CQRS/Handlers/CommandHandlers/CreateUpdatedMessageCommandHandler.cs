using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateUpdatedMessageCommandHandler : IRequestHandler<CreateUpdatedMessageCommandRequest, CreateUpdatedMessageCommandResponse>
    {
        private readonly IUpdatedMessageRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public CreateUpdatedMessageCommandHandler(IDistributedCache distributedCache, UpdatedMessageRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<CreateUpdatedMessageCommandResponse> Handle(CreateUpdatedMessageCommandRequest createUpdateMessageCommandRequest, CancellationToken cancellationToken)
        {
            CreateUpdatedMessageCommandResponse createUpdateMessageCommandResponse = new CreateUpdatedMessageCommandResponse();

            var result = _repo.Add(
                new UpdatedMessage
                {
                    Message = createUpdateMessageCommandRequest.Message,
                    OldImageURL = createUpdateMessageCommandRequest.OldImageURL,
                    OldMessageText = createUpdateMessageCommandRequest.OldMessageText,
                    OldMessageType = createUpdateMessageCommandRequest.OldMessageType,
                    OldVideoURL = createUpdateMessageCommandRequest.OldVideoURL,
                    SendTime = createUpdateMessageCommandRequest.SendTime,
                    UpdateTime = DateTime.Now
                }).Result;

            createUpdateMessageCommandResponse.IsSuccess = result.State == EntityState.Added;
            createUpdateMessageCommandResponse.UpdatedMessage = result.Entity;

            if(createUpdateMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("updatedMessages");
            }

            return createUpdateMessageCommandResponse;
        }
    }
}
