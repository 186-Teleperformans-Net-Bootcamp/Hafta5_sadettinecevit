using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommandRequest, DeleteGroupCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteGroupCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteGroupCommandResponse> Handle(DeleteGroupCommandRequest deleteGroupCommandRequest, CancellationToken cancellationToken)
        {
            DeleteGroupCommandResponse deleteGroupCommandResponse = new DeleteGroupCommandResponse();

            Group group = _unitOfWork.GroupRepository.GetAsync().Result.FirstOrDefault<Group>(g => g.Id == deleteGroupCommandRequest.Id);
            EntityEntry<Group> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = _unitOfWork.GroupRepository.Delete(group).Result;
                deleteGroupCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            if (deleteGroupCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("groups");
            }

            return deleteGroupCommandResponse;
        }
    }
}
