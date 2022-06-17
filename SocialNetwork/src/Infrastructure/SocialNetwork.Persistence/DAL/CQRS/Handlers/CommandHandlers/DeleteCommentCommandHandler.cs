using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommandRequest, DeleteCommentCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public DeleteCommentCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommandRequest deleteCommentCommandRequest, CancellationToken cancellationToken)
        {
            DeleteCommentCommandResponse deleteCommentCommandResponse = new DeleteCommentCommandResponse();

            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == deleteCommentCommandRequest.Id);
            EntityState result = _context.Comments.Remove(comment).State;
            deleteCommentCommandResponse.IsSuccess = result == EntityState.Deleted;

            return deleteCommentCommandResponse;
        }
    }
}
