using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteUpdatedMessageCommandHandler : IRequestHandler<DeleteUpdatedMessageCommandRequest, DeleteUpdatedMessageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteUpdatedMessageCommandHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteUpdatedMessageCommandResponse> Handle(DeleteUpdatedMessageCommandRequest deleteUpdateMessageCommandRequest, CancellationToken cancellationToken)
        {
            DeleteUpdatedMessageCommandResponse deleteUpdateMessageCommandResponse = new DeleteUpdatedMessageCommandResponse();

            UpdatedMessage updatedMessage = _unitOfWork.UpdatedMessageRepository.GetAsync().Result.FirstOrDefault<UpdatedMessage>(u => u.Id == deleteUpdateMessageCommandRequest.Id);
            EntityEntry<UpdatedMessage> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            
            try
            {
                result = await _unitOfWork.UpdatedMessageRepository.Delete(updatedMessage);

                deleteUpdateMessageCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception)
            {
                await retVal.RollbackAsync();
            }

            if (deleteUpdateMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("updatedMessages");
            }

            return deleteUpdateMessageCommandResponse;
        }
    }
}
