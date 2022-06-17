using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateMessageTypeCommandHandler : IRequestHandler<CreateMessageTypeCommandRequest, CreateMessageTypeCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateMessageTypeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateMessageTypeCommandResponse> Handle(CreateMessageTypeCommandRequest createMessageTypeCommandRequest, CancellationToken cancellationToken)
        {
            CreateMessageTypeCommandResponse createMessageTypeCommandResponse = new CreateMessageTypeCommandResponse();

            var result = _context.MessageTypes.Add(
                new MessageType
                {
                    Type = createMessageTypeCommandRequest.Type
                });

            createMessageTypeCommandResponse.IsSuccess = result.State == EntityState.Added;
            createMessageTypeCommandResponse.MessageType = result.Entity;

            return createMessageTypeCommandResponse;
        }
    }
}
