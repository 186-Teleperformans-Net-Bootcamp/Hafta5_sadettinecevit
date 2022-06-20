using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommandRequest, DeleteCommentCommandResponse>
    {
        private readonly ICommentRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public DeleteCommentCommandHandler(IDistributedCache distributedCache, CommentRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommandRequest deleteCommentCommandRequest, CancellationToken cancellationToken)
        {
            DeleteCommentCommandResponse deleteCommentCommandResponse = new DeleteCommentCommandResponse();

            Comment comment = _repo.GetAsync().Result.FirstOrDefault(c => c.Id == deleteCommentCommandRequest.Id);
            EntityEntry<Comment> result = _repo.Delete(comment).Result;
            deleteCommentCommandResponse.IsSuccess = result.State == EntityState.Deleted;
            
            if(deleteCommentCommandResponse.IsSuccess)
            {
                 await _distributedCache.RemoveAsync("comments");
            }

            return deleteCommentCommandResponse;
        }
    }
}
