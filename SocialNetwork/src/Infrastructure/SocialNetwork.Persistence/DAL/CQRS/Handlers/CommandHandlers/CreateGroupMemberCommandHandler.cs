using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateGroupMemberCommandHandler : IRequestHandler<CreateGroupMemberCommandRequest, CreateGroupMemberCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public CreateGroupMemberCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateGroupMemberCommandResponse> Handle(CreateGroupMemberCommandRequest createGroupMemberCommandRequest, CancellationToken cancellationToken)
        {
            CreateGroupMemberCommandResponse createGroupMemberCommandResponse = new CreateGroupMemberCommandResponse();
            EntityEntry<GroupMember> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            try
            {
                result = await _unitOfWork.GroupMemberRepository.Add(
                    new GroupMember
                    {
                        Group = createGroupMemberCommandRequest.Group,
                        User = createGroupMemberCommandRequest.User
                    });
                createGroupMemberCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            createGroupMemberCommandResponse.GroupMember = result?.Entity;

            if (createGroupMemberCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("groupMember");
            }

            return createGroupMemberCommandResponse;
        }
    }
}
