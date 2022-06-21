using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Application.Dto;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;
using SocialNetwork.Application.Interfaces.UnitOfWork;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdCommentQueryHandler : IRequestHandler<GetByIdCommentQueryRequest, GetByIdCommentQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdCommentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetByIdCommentQueryResponse> Handle(GetByIdCommentQueryRequest request, CancellationToken cancellationToken)
        {
            Comment result = _unitOfWork.CommentRepository.GetByIdAsync(request.Id).Result;
            GetByIdCommentQueryResponse getByIdCommentQueryResponse = new GetByIdCommentQueryResponse()
            {
                CommentQueryResponse = new CommentQueryResponseDTO()
                {
                    ToUser = result.ToUser,
                    IsPrivate = result.IsPrivate,
                    CommentText = result.CommentText,
                    Id = request.Id,
                    FromUser = result.FromUser
                }
            };

            return getByIdCommentQueryResponse;
        }
    }
}
