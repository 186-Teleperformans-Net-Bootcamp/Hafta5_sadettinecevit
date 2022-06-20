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
    public class CreateGroupMemberCommandHandler : IRequestHandler<CreateGroupMemberCommandRequest, CreateGroupMemberCommandResponse>
    {
        private readonly IGroupMemberRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public CreateGroupMemberCommandHandler(IDistributedCache distributedCache, GroupMemberRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<CreateGroupMemberCommandResponse> Handle(CreateGroupMemberCommandRequest createGroupMemberCommandRequest, CancellationToken cancellationToken)
        {
            CreateGroupMemberCommandResponse createGroupMemberCommandResponse = new CreateGroupMemberCommandResponse();

            var result = await _repo.Add(
                new GroupMember
                {
                    Group = createGroupMemberCommandRequest.Group,
                    User = createGroupMemberCommandRequest.User
                });

            createGroupMemberCommandResponse.IsSuccess = result.State == EntityState.Added;
            createGroupMemberCommandResponse.GroupMember = result.Entity;

            if (createGroupMemberCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("groupMember");
            }

            return createGroupMemberCommandResponse;
        }
    }
}
