using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateFriendCommandHandler : IRequestHandler<CreateFriendCommandRequest, CreateFriendCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public CreateFriendCommandHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<CreateFriendCommandResponse> Handle(CreateFriendCommandRequest createFriendCommandRequest, CancellationToken cancellationToken)
        {
            CreateFriendCommandResponse createFriendCommandResponse = new CreateFriendCommandResponse();

            var result = _context.Friends.Add(
                new Friend
                {
                    FriendUser = createFriendCommandRequest.FriendUser,
                    User = createFriendCommandRequest.User,
                    TimeToBeFriend = DateTime.Now
                });

            createFriendCommandResponse.IsSuccess = result.State == EntityState.Added;
            createFriendCommandResponse.Friend = result.Entity;

            if(createFriendCommandResponse.IsSuccess)
            {
                _distributedCache.RemoveAsync("friends");
            }

            return createFriendCommandResponse;
        }
    }
}
