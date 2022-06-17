using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateUpdatedMessageCommandHandler : IRequestHandler<CreateUpdatedMessageCommandRequest, CreateUpdatedMessageCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateUpdatedMessageCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateUpdatedMessageCommandResponse> Handle(CreateUpdatedMessageCommandRequest createUpdateMessageCommandRequest, CancellationToken cancellationToken)
        {
            CreateUpdatedMessageCommandResponse createUpdateMessageCommandResponse = new CreateUpdatedMessageCommandResponse();

            var result = _context.UpdatedMessages.Add(
                new UpdatedMessage
                {
                    Message = createUpdateMessageCommandRequest.Message,
                    OldImageURL = createUpdateMessageCommandRequest.OldImageURL,
                    OldMessageText = createUpdateMessageCommandRequest.OldMessageText,
                    OldMessageType = createUpdateMessageCommandRequest.OldMessageType,
                    OldVideoURL = createUpdateMessageCommandRequest.OldVideoURL,
                    SendTime = createUpdateMessageCommandRequest.SendTime,
                    UpdateTime = DateTime.Now
                });

            createUpdateMessageCommandResponse.IsSuccess = result.State == EntityState.Added;
            createUpdateMessageCommandResponse.UpdatedMessage = result.Entity;

            return createUpdateMessageCommandResponse;
        }
    }
}
