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
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommandRequest, DeleteMessageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteMessageCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteMessageCommandResponse> Handle(DeleteMessageCommandRequest deleteMessageCommandRequest, CancellationToken cancellationToken)
        {
            Message message = _unitOfWork.MessageRepository.GetAsync().Result.FirstOrDefault<Message>(m=>m.Id == deleteMessageCommandRequest.Id);

            DeleteMessageCommandResponse deleteMessageCommandResponse = new DeleteMessageCommandResponse();
            EntityEntry<Message> result = null;
            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = _unitOfWork.MessageRepository.Delete(message).Result;
                deleteMessageCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch(Exception ex)
            {
                await retVal.RollbackAsync();
            }

            if(deleteMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messages");
            }

            return deleteMessageCommandResponse;
        }
    }
}
