using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommandRequest, DeleteUserCommandResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;
        public DeleteUserCommandHandler(UserManager<User> userManager, IDistributedCache distributedCache)
        {
            _userManager = userManager;
            _distributedCache = distributedCache;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommandRequest deleteUserCommandRequest, CancellationToken cancellationToken)
        {
            DeleteUserCommandResponse deleteUserCommandResponse = new DeleteUserCommandResponse();

            User user = _userManager.FindByIdAsync(deleteUserCommandRequest.Id).Result;

            var result = _userManager.DeleteAsync(user).Result;

            deleteUserCommandResponse.IsSuccess = result.Succeeded;

            if (deleteUserCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("comments");
            }

            return deleteUserCommandResponse;
        }
    }
}
