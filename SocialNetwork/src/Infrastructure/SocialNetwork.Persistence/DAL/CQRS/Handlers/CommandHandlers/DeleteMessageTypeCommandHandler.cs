using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteMessageTypeCommandHandler : IRequestHandler<DeleteMessageTypeCommandRequest, DeleteMessageTypeCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public DeleteMessageTypeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteMessageTypeCommandResponse> Handle(DeleteMessageTypeCommandRequest deleteMessageTypeCommandRequest, CancellationToken cancellationToken)
        {
            DeleteMessageTypeCommandResponse deleteMessageTypeCommandResponse = new DeleteMessageTypeCommandResponse();

            MessageType messageType = _context.MessageTypes.FirstOrDefault<MessageType>(m=>m.Id == deleteMessageTypeCommandRequest.Id);
            EntityState result = _context.MessageTypes.Remove(messageType).State;
            deleteMessageTypeCommandResponse.IsSuccess = result == EntityState.Deleted;

            return deleteMessageTypeCommandResponse;
        }
    }
}
