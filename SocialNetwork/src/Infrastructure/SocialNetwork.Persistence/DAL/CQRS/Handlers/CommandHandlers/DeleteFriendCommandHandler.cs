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
    public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommandRequest, DeleteFriendCommandResponse>
    {
        private readonly IFriendRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteFriendCommandHandler(IDistributedCache distributedCache, FriendRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<DeleteFriendCommandResponse> Handle(DeleteFriendCommandRequest deleteFriendCommandRequest, CancellationToken cancellationToken)
        {
            DeleteFriendCommandResponse deleteFriendCommandResponse = new DeleteFriendCommandResponse();

            Friend friend = _repo.GetAsync().Result.FirstOrDefault<Friend>(f => f.Id == deleteFriendCommandRequest.Id);
            EntityEntry<Friend> result = _repo.Delete(friend).Result;
            deleteFriendCommandResponse.IsSuccess = result.State == EntityState.Deleted;

            if(deleteFriendCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("friends");
            }

            return deleteFriendCommandResponse;
        }
    }
}
