using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdGroupQueryHandler : IRequestHandler<GetByIdGroupQueryRequest, GetByIdGroupQueryResponse>
    {
        private readonly IGroupRepository _repo;

        public GetByIdGroupQueryHandler(GroupRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetByIdGroupQueryResponse> Handle(GetByIdGroupQueryRequest request, CancellationToken cancellationToken)
        {
            Group result = _repo.GetByIdAsync(request.Id).Result;
            GetByIdGroupQueryResponse getByIdGroupQueryResponse = new GetByIdGroupQueryResponse()
            {
                Id = result.Id,
                Name = result.Name
            };

            return getByIdGroupQueryResponse;
        }
    }
}
