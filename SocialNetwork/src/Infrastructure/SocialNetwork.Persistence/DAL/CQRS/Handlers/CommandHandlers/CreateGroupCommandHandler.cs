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
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommandRequest, CreateGroupCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public CreateGroupCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<CreateGroupCommandResponse> Handle(CreateGroupCommandRequest createGroupCommandRequest, CancellationToken cancellationToken)
        {
            CreateGroupCommandResponse createGroupCommandResponse = new CreateGroupCommandResponse();
            EntityEntry<Group> result = null;
            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            try
            {
                result = await _unitOfWork.GroupRepository.Add(
                new Group
                {
                    Name = createGroupCommandRequest.Name
                });
                createGroupCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            createGroupCommandResponse.Group = result?.Entity;

            if (createGroupCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("group");
            }

            return createGroupCommandResponse;
        }
    }
}
