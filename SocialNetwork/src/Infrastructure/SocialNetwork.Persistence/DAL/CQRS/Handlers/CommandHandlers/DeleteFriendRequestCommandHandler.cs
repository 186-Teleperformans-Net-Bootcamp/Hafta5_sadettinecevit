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
    public class DeleteFriendRequestCommandHandler : IRequestHandler<DeleteFriendRequestCommandRequest, DeleteFriendRequestCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteFriendRequestCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteFriendRequestCommandResponse> Handle(DeleteFriendRequestCommandRequest deleteFriendRequestCommandRequest, CancellationToken cancellationToken)
        {
            DeleteFriendRequestCommandResponse deleteFriendRequestCommandResponse = new DeleteFriendRequestCommandResponse();

            FriendRequest friendRequest = _unitOfWork.FriendRequestRepository.GetAsync().Result.FirstOrDefault<FriendRequest>(f => f.Id == deleteFriendRequestCommandRequest.Id);
            EntityEntry<FriendRequest> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = _unitOfWork.FriendRequestRepository.Delete(friendRequest).Result;
                deleteFriendRequestCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception)
            {
                await retVal.RollbackAsync();
            }

            if (deleteFriendRequestCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("friendRequests");
            }

            return deleteFriendRequestCommandResponse;
        }
    }
}
