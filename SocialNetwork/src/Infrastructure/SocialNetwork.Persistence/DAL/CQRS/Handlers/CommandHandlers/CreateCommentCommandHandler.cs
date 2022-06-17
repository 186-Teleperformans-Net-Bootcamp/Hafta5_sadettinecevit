using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateCommentCommandHandler(ApplicationDbContext context)
        {
            _context = context;
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

            return createCommentCommandResponse;
        }
    }
}
