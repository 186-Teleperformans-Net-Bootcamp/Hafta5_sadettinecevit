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
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommandRequest, CreateGroupCommandResponse>
    {
        private readonly IGroupRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public CreateGroupCommandHandler(IDistributedCache distributedCache, GroupRepository repo)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<CreateGroupCommandResponse> Handle(CreateGroupCommandRequest createGroupCommandRequest, CancellationToken cancellationToken)
        {
            CreateGroupCommandResponse createGroupCommandResponse = new CreateGroupCommandResponse();

            var result = await _repo.Add(
                new Group
                {
                    Name = createGroupCommandRequest.Name
                });

            createGroupCommandResponse.IsSuccess = result.State == EntityState.Added;
            createGroupCommandResponse.Group = result.Entity;

            if(createGroupCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("group");
            }

            return createGroupCommandResponse;
        }
    }
}
