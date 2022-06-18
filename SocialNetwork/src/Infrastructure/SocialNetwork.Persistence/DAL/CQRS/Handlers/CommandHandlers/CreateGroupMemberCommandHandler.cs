using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateGroupMemberCommandHandler : IRequestHandler<CreateGroupMemberCommandRequest, CreateGroupMemberCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public CreateGroupMemberCommandHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<CreateGroupMemberCommandResponse> Handle(CreateGroupMemberCommandRequest createGroupMemberCommandRequest, CancellationToken cancellationToken)
        {
            CreateGroupMemberCommandResponse createGroupMemberCommandResponse = new CreateGroupMemberCommandResponse();

            var result = _context.GroupMembers.Add(
                new GroupMember
                {
                    Group = createGroupMemberCommandRequest.Group,
                    User = createGroupMemberCommandRequest.User
                });

            createGroupMemberCommandResponse.IsSuccess = result.State == EntityState.Added;
            createGroupMemberCommandResponse.GroupMember = result.Entity;

            if(createGroupMemberCommandResponse.IsSuccess)
            {
                _distributedCache.RemoveAsync("groupMember");
            }

            return createGroupMemberCommandResponse;
        }
    }
}
