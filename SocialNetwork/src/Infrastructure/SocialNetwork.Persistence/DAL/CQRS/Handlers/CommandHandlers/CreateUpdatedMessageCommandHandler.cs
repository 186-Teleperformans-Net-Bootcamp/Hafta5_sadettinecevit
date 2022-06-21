using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateUpdatedMessageCommandHandler : IRequestHandler<CreateUpdatedMessageCommandRequest, CreateUpdatedMessageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public CreateUpdatedMessageCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUpdatedMessageCommandResponse> Handle(CreateUpdatedMessageCommandRequest createUpdateMessageCommandRequest, CancellationToken cancellationToken)
        {
            CreateUpdatedMessageCommandResponse createUpdateMessageCommandResponse = new CreateUpdatedMessageCommandResponse();

            EntityEntry<UpdatedMessage> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = _unitOfWork.UpdatedMessageRepository.Add(
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

                createUpdateMessageCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            createUpdateMessageCommandResponse.UpdatedMessage = result?.Entity;

            if (createUpdateMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("updatedMessages");
            }

            return createUpdateMessageCommandResponse;
        }
    }
}
