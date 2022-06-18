using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommandRequest, DeleteCommentCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public DeleteCommentCommandHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommandRequest deleteCommentCommandRequest, CancellationToken cancellationToken)
        {
            DeleteCommentCommandResponse deleteCommentCommandResponse = new DeleteCommentCommandResponse();

            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == deleteCommentCommandRequest.Id);
            EntityState result = _context.Comments.Remove(comment).State;
            deleteCommentCommandResponse.IsSuccess = result == EntityState.Deleted;
            
            if(deleteCommentCommandResponse.IsSuccess)
            {
                _distributedCache.RemoveAsync("comments");
            }

            return deleteCommentCommandResponse;
        }
    }
}
