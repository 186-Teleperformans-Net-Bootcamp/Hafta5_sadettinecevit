using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommandRequest, DeleteMessageCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public DeleteMessageCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteMessageCommandResponse> Handle(DeleteMessageCommandRequest deleteMessageCommandRequest, CancellationToken cancellationToken)
        {
            Message message = _context.Messages.FirstOrDefault<Message>(m=>m.Id == deleteMessageCommandRequest.Id);
            EntityState result = _context.Messages.Remove(message).State;
           
            DeleteMessageCommandResponse deleteMessageCommandResponse = new DeleteMessageCommandResponse()
            {
                IsSuccess = result == EntityState.Deleted
            };

            return deleteMessageCommandResponse;
        }
    }
}
