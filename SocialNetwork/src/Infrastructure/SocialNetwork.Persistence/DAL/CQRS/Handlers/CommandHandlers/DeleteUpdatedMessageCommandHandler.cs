using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteUpdatedMessageCommandHandler : IRequestHandler<DeleteUpdatedMessageCommandRequest, DeleteUpdatedMessageCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public DeleteUpdatedMessageCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteUpdatedMessageCommandResponse> Handle(DeleteUpdatedMessageCommandRequest deleteUpdateMessageCommandRequest, CancellationToken cancellationToken)
        {
            DeleteUpdatedMessageCommandResponse deleteUpdateMessageCommandResponse = new DeleteUpdatedMessageCommandResponse();

            UpdatedMessage updatedMessage = _context.UpdatedMessages.FirstOrDefault<UpdatedMessage>(u => u.Id == deleteUpdateMessageCommandRequest.Id);
            EntityState result = _context.UpdatedMessages.Remove(updatedMessage).State;
            deleteUpdateMessageCommandResponse.IsSuccess = result == EntityState.Deleted;

            return deleteUpdateMessageCommandResponse;
        }
    }
}
