using MediatR;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommandRequest, DeleteUserCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public DeleteUserCommandHandler(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommandRequest deleteUserCommandRequest, CancellationToken cancellationToken)
        {
            DeleteUserCommandResponse deleteUserCommandResponse = new DeleteUserCommandResponse();

            User user = _userManager.FindByIdAsync(deleteUserCommandRequest.Id).Result;

            var result = _userManager.DeleteAsync(user).Result;

            deleteUserCommandResponse.IsSuccess = result.Succeeded;

            return deleteUserCommandResponse;
        }
    }
}
