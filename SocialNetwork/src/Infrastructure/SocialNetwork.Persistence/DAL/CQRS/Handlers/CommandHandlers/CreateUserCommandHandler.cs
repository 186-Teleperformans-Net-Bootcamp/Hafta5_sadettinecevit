using MediatR;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public CreateUserCommandHandler(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest createUserCommandRequest, CancellationToken cancellationToken)
        {
            CreateUserCommandResponse createUserCommandResponse = new CreateUserCommandResponse();

            User user = new User()
            {
                UserName = createUserCommandRequest.UserName,
                Name = createUserCommandRequest.UserName,
                LastName = createUserCommandRequest.LastName,
                Email = createUserCommandRequest.Email
            };

            var result = _userManager.CreateAsync(user, createUserCommandRequest.Password).Result;

            createUserCommandResponse.IsSuccess = result.Succeeded;
            createUserCommandResponse.User = result.Succeeded ? user : null;

            return createUserCommandResponse;
        }
    }
}
