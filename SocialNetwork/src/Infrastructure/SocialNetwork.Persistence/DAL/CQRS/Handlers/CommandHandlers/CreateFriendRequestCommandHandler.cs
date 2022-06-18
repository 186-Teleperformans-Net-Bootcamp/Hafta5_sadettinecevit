using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateFriendRequestCommandHandler : IRequestHandler<CreateFriendRequestCommandRequest, CreateFriendRequestCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public CreateFriendRequestCommandHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<CreateFriendRequestCommandResponse> Handle(CreateFriendRequestCommandRequest createFriendRequestCommandRequest, CancellationToken cancellationToken)
        {
            CreateFriendRequestCommandResponse createFriendRequestCommandResponse = new CreateFriendRequestCommandResponse();

            var result = _context.FriendRequests.Add(
                new FriendRequest
                {
                    FromUser = createFriendRequestCommandRequest.FromUser,
                    ToUser = createFriendRequestCommandRequest.ToUser,
                    RequestTime = DateTime.Now
                });

            createFriendRequestCommandResponse.IsSuccess = result.State == EntityState.Added;
            createFriendRequestCommandResponse.FriendRequest = result.Entity;

            if(createFriendRequestCommandResponse.FriendRequest != null)
            {
                _distributedCache.RemoveAsync("friends");
            }

            return createFriendRequestCommandResponse;
        }
    }
}
