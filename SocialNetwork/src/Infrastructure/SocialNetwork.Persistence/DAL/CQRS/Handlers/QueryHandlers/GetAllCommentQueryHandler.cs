using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllCommentQueryHandler : IRequestHandler<GetAllCommentQueryRequest, List<GetAllCommentQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllCommentQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllCommentQueryResponse>> Handle(GetAllCommentQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllCommentQueryResponse> getAllCommentQueryResponse = new List<GetAllCommentQueryResponse>();

            getAllCommentQueryResponse = _context.Comments.Select(c =>
            new GetAllCommentQueryResponse
            {
                CommentText = c.CommentText,
                FromUser = c.FromUser,
                ToUser = c.ToUser,
                IsPrivate = c.IsPrivate,
                Id = c.Id
            }).ToList();

            return getAllCommentQueryResponse;
        }
    }
}
