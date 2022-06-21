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
    public class CreateMessageTypeCommandHandler : IRequestHandler<CreateMessageTypeCommandRequest, CreateMessageTypeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public CreateMessageTypeCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateMessageTypeCommandResponse> Handle(CreateMessageTypeCommandRequest createMessageTypeCommandRequest, CancellationToken cancellationToken)
        {
            CreateMessageTypeCommandResponse createMessageTypeCommandResponse = new CreateMessageTypeCommandResponse();

            EntityEntry<MessageType> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = await _unitOfWork.MessageTypeRepository.Add(
                new MessageType
                {
                    Type = createMessageTypeCommandRequest.Type
                });
                createMessageTypeCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }
            createMessageTypeCommandResponse.MessageType = result?.Entity;

            if (createMessageTypeCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messageTypes");
            }

            return createMessageTypeCommandResponse;
        }
    }
}
