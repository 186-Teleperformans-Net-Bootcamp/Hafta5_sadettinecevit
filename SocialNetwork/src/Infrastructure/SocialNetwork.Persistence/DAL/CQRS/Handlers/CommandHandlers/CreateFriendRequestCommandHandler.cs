using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateFriendRequestCommandHandler : IRequestHandler<CreateFriendRequestCommandRequest, CreateFriendRequestCommandResponse>
    {
        private readonly IFriendRequestRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public CreateFriendRequestCommandHandler(IDistributedCache distributedCache, FriendRequestRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<CreateFriendRequestCommandResponse> Handle(CreateFriendRequestCommandRequest createFriendRequestCommandRequest, CancellationToken cancellationToken)
        {
            CreateFriendRequestCommandResponse createFriendRequestCommandResponse = new CreateFriendRequestCommandResponse();

            var result = await _repo.Add(
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
                await _distributedCache.RemoveAsync("friends");
            }

            return createFriendRequestCommandResponse;
        }
    }
}
