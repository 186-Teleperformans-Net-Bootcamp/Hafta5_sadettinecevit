using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommandRequest, CreateGroupCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateGroupCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateGroupCommandResponse> Handle(CreateGroupCommandRequest createGroupCommandRequest, CancellationToken cancellationToken)
        {
            CreateGroupCommandResponse createGroupCommandResponse = new CreateGroupCommandResponse();

            var result = _context.Groups.Add(
                new Group
                {
                    Name = createGroupCommandRequest.Name
                });

            createGroupCommandResponse.IsSuccess = result.State == EntityState.Added;
            createGroupCommandResponse.Group = result.Entity;

            return createGroupCommandResponse;
        }
    }
}
