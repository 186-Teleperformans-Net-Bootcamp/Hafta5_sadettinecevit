using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public CreateCommentCommandHandler(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest createCommentCommandRequest, CancellationToken cancellationToken)
        {
            CreateCommentCommandResponse createCommentCommandResponse = new CreateCommentCommandResponse();

            var result = _context.Comments.Add(
                new Comment
                {
                    FromUser = createCommentCommandRequest.FromUser,
                    ToUser = createCommentCommandRequest.ToUser,
                    CommentText = createCommentCommandRequest.CommentText,
                    CommentTime = DateTime.Now,
                    IsPrivate = createCommentCommandRequest.IsPrivate
                });

            createCommentCommandResponse.IsSuccess = result.State == EntityState.Added;
            createCommentCommandResponse.Comment = result.Entity;

            if(createCommentCommandResponse.IsSuccess)
            {
                _distributedCache.RemoveAsync("comments");
            }

            return createCommentCommandResponse;
        }
    }
}
