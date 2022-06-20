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
    public class DeleteFriendRequestCommandHandler : IRequestHandler<DeleteFriendRequestCommandRequest, DeleteFriendRequestCommandResponse>
    {
        private readonly IFriendRequestRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteFriendRequestCommandHandler(IDistributedCache distributedCache, FriendRequestRepository repo)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteFriendRequestCommandResponse> Handle(DeleteFriendRequestCommandRequest deleteFriendRequestCommandRequest, CancellationToken cancellationToken)
        {
            DeleteFriendRequestCommandResponse deleteFriendRequestCommandResponse = new DeleteFriendRequestCommandResponse();

            FriendRequest friendRequest = _repo.GetAsync().Result.FirstOrDefault<FriendRequest>(f => f.Id == deleteFriendRequestCommandRequest.Id);
            EntityEntry<FriendRequest> result = _repo.Delete(friendRequest).Result;
            deleteFriendRequestCommandResponse.IsSuccess = result.State == EntityState.Deleted;

            if(deleteFriendRequestCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("friendRequests");
            }

            return deleteFriendRequestCommandResponse;
        }
    }
}
