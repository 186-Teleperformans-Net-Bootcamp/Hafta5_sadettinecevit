using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommandRequest, CreateMessageCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateMessageCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateMessageCommandResponse> Handle(CreateMessageCommandRequest createMessageCommandRequest, CancellationToken cancellationToken)
        {
            CreateMessageCommandResponse createMessageCommandResponse = new CreateMessageCommandResponse();

            var result = _context.Messages.Add(
                new Message
                {
                    FromUser = createMessageCommandRequest.FromUser,
                    ImageURL = createMessageCommandRequest.ImageURL,
                    MessageText = createMessageCommandRequest.MessageText,
                    TimeToSent = DateTime.Now,
                    ToUsers = createMessageCommandRequest.ToUsers,
                    Type = createMessageCommandRequest.Type,
                    VideoURL = createMessageCommandRequest.VideoURL
                });

            createMessageCommandResponse.IsSuccess = result.State == EntityState.Added;
            createMessageCommandResponse.Message = result.Entity;

            return createMessageCommandResponse;
        }
    }
}
