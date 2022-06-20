using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdCommentQueryHandler : IRequestHandler<GetByIdCommentQueryRequest, GetByIdCommentQueryResponse>
    {
        private readonly ICommentRepository _repo;

        public GetByIdCommentQueryHandler(CommentRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetByIdCommentQueryResponse> Handle(GetByIdCommentQueryRequest request, CancellationToken cancellationToken)
        {
            Comment result = _repo.GetByIdAsync(request.Id).Result;
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
