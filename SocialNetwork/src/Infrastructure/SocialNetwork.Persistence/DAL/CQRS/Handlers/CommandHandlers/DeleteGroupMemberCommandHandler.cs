using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteGroupMemberCommandHandler : IRequestHandler<DeleteGroupMemberCommandRequest, DeleteGroupMemberCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public DeleteGroupMemberCommandHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteGroupMemberCommandResponse> Handle(DeleteGroupMemberCommandRequest deleteGroupMemberCommandRequest, CancellationToken cancellationToken)
        {
            DeleteGroupMemberCommandResponse deleteGroupMemberCommandResponse = new DeleteGroupMemberCommandResponse();

            GroupMember groupMember = _context.GroupMembers.FirstOrDefault<GroupMember>(g => g.Id == deleteGroupMemberCommandRequest.Id);
            EntityState result = _context.GroupMembers.Remove(groupMember).State;
            deleteGroupMemberCommandResponse.IsSuccess = result == EntityState.Deleted;

            if(deleteGroupMemberCommandResponse.IsSuccess)
            {
                _distributedCache.RemoveAsync("groupMembers");
            }

            return deleteGroupMemberCommandResponse;
        }
    }
}
