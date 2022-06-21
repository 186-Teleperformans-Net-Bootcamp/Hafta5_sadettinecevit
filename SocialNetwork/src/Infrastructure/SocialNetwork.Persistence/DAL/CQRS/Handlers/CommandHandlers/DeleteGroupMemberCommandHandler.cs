using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteGroupMemberCommandHandler : IRequestHandler<DeleteGroupMemberCommandRequest, DeleteGroupMemberCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteGroupMemberCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteGroupMemberCommandResponse> Handle(DeleteGroupMemberCommandRequest deleteGroupMemberCommandRequest, CancellationToken cancellationToken)
        {
            DeleteGroupMemberCommandResponse deleteGroupMemberCommandResponse = new DeleteGroupMemberCommandResponse();

            GroupMember groupMember = _unitOfWork.GroupMemberRepository.GetAsync().Result.FirstOrDefault<GroupMember>(g => g.Id == deleteGroupMemberCommandRequest.Id);
            EntityEntry<GroupMember> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = _unitOfWork.GroupMemberRepository.Delete(groupMember).Result;
                deleteGroupMemberCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            if (deleteGroupMemberCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("groupMembers");
            }

            return deleteGroupMemberCommandResponse;
        }
    }
}
