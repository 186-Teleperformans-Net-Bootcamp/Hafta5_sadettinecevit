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
    public class CreateFriendRequestCommandHandler : IRequestHandler<CreateFriendRequestCommandRequest, CreateFriendRequestCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public CreateFriendRequestCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateFriendRequestCommandResponse> Handle(CreateFriendRequestCommandRequest createFriendRequestCommandRequest, CancellationToken cancellationToken)
        {
            CreateFriendRequestCommandResponse createFriendRequestCommandResponse = new CreateFriendRequestCommandResponse();
            EntityEntry<FriendRequest> result = null;
            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            try
            {
                result = await _unitOfWork.FriendRequestRepository.Add(
                    new FriendRequest
                    {
                        FromUser = createFriendRequestCommandRequest.FromUser,
                        ToUser = createFriendRequestCommandRequest.ToUser,
                        RequestTime = DateTime.Now
                    });
                createFriendRequestCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception)
            {
                await retVal.RollbackAsync();
            }

            createFriendRequestCommandResponse.FriendRequest = result?.Entity;

            if (createFriendRequestCommandResponse.FriendRequest != null)
            {
                await _distributedCache.RemoveAsync("friends");
            }

            return createFriendRequestCommandResponse;
        }
    }
}
