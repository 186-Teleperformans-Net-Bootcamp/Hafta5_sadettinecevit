using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdCommentQueryHandler : IRequestHandler<GetByIdCommentQueryRequest, GetByIdCommentQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdCommentQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdCommentQueryResponse> Handle(GetByIdCommentQueryRequest request, CancellationToken cancellationToken)
        {
            Comment result = _context.Comments.FirstOrDefault(c => c.Id == request.Id);
            GetByIdCommentQueryResponse getByIdCommentQueryResponse = new GetByIdCommentQueryResponse()
            {
                ToUser = result.ToUser,
                IsPrivate = result.IsPrivate,
                CommentText = result.CommentText,
                Id = request.Id,
                FromUser = result.FromUser
            };

            return getByIdCommentQueryResponse;
        }
    }
}
