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
    public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommandRequest, DeleteFriendCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteFriendCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteFriendCommandResponse> Handle(DeleteFriendCommandRequest deleteFriendCommandRequest, CancellationToken cancellationToken)
        {
            DeleteFriendCommandResponse deleteFriendCommandResponse = new DeleteFriendCommandResponse();

            Friend friend = _unitOfWork.FriendRepository.GetAsync().Result.FirstOrDefault<Friend>(f => f.Id == deleteFriendCommandRequest.Id);
            EntityEntry<Friend> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = _unitOfWork.FriendRepository.Delete(friend).Result;
                deleteFriendCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }


            if (deleteFriendCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("friends");
            }

            return deleteFriendCommandResponse;
        }
    }
}
