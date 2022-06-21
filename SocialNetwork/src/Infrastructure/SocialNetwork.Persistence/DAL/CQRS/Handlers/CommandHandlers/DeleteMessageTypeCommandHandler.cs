using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteMessageTypeCommandHandler : IRequestHandler<DeleteMessageTypeCommandRequest, DeleteMessageTypeCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteMessageTypeCommandHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteMessageTypeCommandResponse> Handle(DeleteMessageTypeCommandRequest deleteMessageTypeCommandRequest, CancellationToken cancellationToken)
        {
            DeleteMessageTypeCommandResponse deleteMessageTypeCommandResponse = new DeleteMessageTypeCommandResponse();

            MessageType messageType = _unitOfWork.MessageTypeRepository.GetAsync().Result.FirstOrDefault<MessageType>(m => m.Id == deleteMessageTypeCommandRequest.Id);
            EntityEntry<MessageType> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            try
            {
                result = await _unitOfWork.MessageTypeRepository.Delete(messageType);

                deleteMessageTypeCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception)
            {
                await retVal.RollbackAsync();
            }

            if (deleteMessageTypeCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messageTypes");
            }

            return deleteMessageTypeCommandResponse;
        }
    }
}
