using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommandRequest, DeleteGroupCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public DeleteGroupCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteGroupCommandResponse> Handle(DeleteGroupCommandRequest deleteGroupCommandRequest, CancellationToken cancellationToken)
        {
            DeleteGroupCommandResponse deleteGroupCommandResponse = new DeleteGroupCommandResponse();

            Group group = _context.Groups.FirstOrDefault<Group>(g => g.Id == deleteGroupCommandRequest.Id);
            EntityState result = _context.Groups.Remove(group).State;
            deleteGroupCommandResponse.IsSuccess = result == EntityState.Deleted;

            return deleteGroupCommandResponse;
        }
    }
}
