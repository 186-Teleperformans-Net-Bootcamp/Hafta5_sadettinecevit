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
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommandRequest, DeleteGroupCommandResponse>
    {
        private readonly IGroupRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteGroupCommandHandler(IDistributedCache distributedCache, GroupRepository repo)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteGroupCommandResponse> Handle(DeleteGroupCommandRequest deleteGroupCommandRequest, CancellationToken cancellationToken)
        {
            DeleteGroupCommandResponse deleteGroupCommandResponse = new DeleteGroupCommandResponse();

            Group group = _repo.GetAsync().Result.FirstOrDefault<Group>(g => g.Id == deleteGroupCommandRequest.Id);
            EntityEntry<Group> result = _repo.Delete(group).Result;
            deleteGroupCommandResponse.IsSuccess = result.State == EntityState.Deleted;

            if(deleteGroupCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("groups");
            }

            return deleteGroupCommandResponse;
        }
    }
}
