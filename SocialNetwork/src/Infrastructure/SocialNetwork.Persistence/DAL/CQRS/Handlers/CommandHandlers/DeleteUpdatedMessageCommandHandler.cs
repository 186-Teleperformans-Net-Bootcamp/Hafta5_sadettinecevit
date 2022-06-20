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
    public class DeleteUpdatedMessageCommandHandler : IRequestHandler<DeleteUpdatedMessageCommandRequest, DeleteUpdatedMessageCommandResponse>
    {
        private readonly IUpdatedMessageRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteUpdatedMessageCommandHandler(UpdatedMessageRepository repo, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<DeleteUpdatedMessageCommandResponse> Handle(DeleteUpdatedMessageCommandRequest deleteUpdateMessageCommandRequest, CancellationToken cancellationToken)
        {
            DeleteUpdatedMessageCommandResponse deleteUpdateMessageCommandResponse = new DeleteUpdatedMessageCommandResponse();

            UpdatedMessage updatedMessage = _repo.GetAsync().Result.FirstOrDefault<UpdatedMessage>(u => u.Id == deleteUpdateMessageCommandRequest.Id);
            EntityEntry<UpdatedMessage> result = await _repo.Delete(updatedMessage);
            deleteUpdateMessageCommandResponse.IsSuccess = result.State == EntityState.Deleted;

            if(deleteUpdateMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("updatedMessages");
            }

            return deleteUpdateMessageCommandResponse;
        }
    }
}
