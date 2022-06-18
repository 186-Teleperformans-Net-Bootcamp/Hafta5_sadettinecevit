using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommandRequest, DeleteFriendCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public DeleteFriendCommandHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<DeleteFriendCommandResponse> Handle(DeleteFriendCommandRequest deleteFriendCommandRequest, CancellationToken cancellationToken)
        {
            DeleteFriendCommandResponse deleteFriendCommandResponse = new DeleteFriendCommandResponse();

            Friend friend = _context.Friends.FirstOrDefault<Friend>(f => f.Id == deleteFriendCommandRequest.Id);
            EntityState result = _context.Friends.Remove(friend).State;
            deleteFriendCommandResponse.IsSuccess = result == EntityState.Deleted;

            if(deleteFriendCommandResponse.IsSuccess)
            {
                _distributedCache.RemoveAsync("friends");
            }

            return deleteFriendCommandResponse;
        }
    }
}
