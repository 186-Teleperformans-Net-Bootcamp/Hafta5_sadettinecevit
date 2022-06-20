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
    public class DeleteGroupMemberCommandHandler : IRequestHandler<DeleteGroupMemberCommandRequest, DeleteGroupMemberCommandResponse>
    {
        private readonly IGroupMemberRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteGroupMemberCommandHandler(IDistributedCache distributedCache, GroupMemberRepository repo)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteGroupMemberCommandResponse> Handle(DeleteGroupMemberCommandRequest deleteGroupMemberCommandRequest, CancellationToken cancellationToken)
        {
            DeleteGroupMemberCommandResponse deleteGroupMemberCommandResponse = new DeleteGroupMemberCommandResponse();

            GroupMember groupMember = _repo.GetAsync().Result.FirstOrDefault<GroupMember>(g => g.Id == deleteGroupMemberCommandRequest.Id);
            EntityEntry<GroupMember> result = _repo.Delete(groupMember).Result;
            deleteGroupMemberCommandResponse.IsSuccess = result.State == EntityState.Deleted;

            if(deleteGroupMemberCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("groupMembers");
            }

            return deleteGroupMemberCommandResponse;
        }
    }
}
