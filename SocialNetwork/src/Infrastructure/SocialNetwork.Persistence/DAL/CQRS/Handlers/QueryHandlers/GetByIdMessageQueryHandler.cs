using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdMessageQueryHandler : IRequestHandler<GetByIdMessageQueryRequest, GetByIdMessageQueryResponse>
    {
        private readonly IUnitOfWork _repo;

        public GetByIdMessageQueryHandler(IUnitOfWork repo)
        {
            _repo = repo;
        }

        public async Task<GetByIdMessageQueryResponse> Handle(GetByIdMessageQueryRequest request, CancellationToken cancellationToken)
        {
            Message result = _repo.MessageRepository.GetByIdAsync(request.Id).Result;
            GetByIdMessageQueryResponse getByIdMessageQueryResponse = new GetByIdMessageQueryResponse()
            {
                MessageQueryResponse = new()
                {
                    Id = result.Id,
                    FromUser = result.FromUser,
                    ImageURL = result.ImageURL,
                    MessageText = result.MessageText,
                    ToUsers = result.ToUsers,
                    Type = result.Type,
                    VideoURL = result.VideoURL
                }
            };

            return getByIdMessageQueryResponse;
        }
    }
}
