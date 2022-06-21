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
    public class CreateFriendCommandHandler : IRequestHandler<CreateFriendCommandRequest, CreateFriendCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public CreateFriendCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateFriendCommandResponse> Handle(CreateFriendCommandRequest createFriendCommandRequest, CancellationToken cancellationToken)
        {
            CreateFriendCommandResponse createFriendCommandResponse = new CreateFriendCommandResponse();
            EntityEntry<Friend> result = null;
            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            try
            {
                result = await _unitOfWork.FriendRepository.Add(
                new Friend
                {
                    FriendUser = createFriendCommandRequest.FriendUser,
                    User = createFriendCommandRequest.User,
                    TimeToBeFriend = DateTime.Now
                });
                createFriendCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            createFriendCommandResponse.Friend = result?.Entity;

            if (createFriendCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("friends");
            }

            return createFriendCommandResponse;
        }
    }
}
