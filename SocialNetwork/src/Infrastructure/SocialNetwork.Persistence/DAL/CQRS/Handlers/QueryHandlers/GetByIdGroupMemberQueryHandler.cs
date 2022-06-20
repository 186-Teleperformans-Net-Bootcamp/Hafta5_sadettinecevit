using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdGroupMemberQueryHandler : IRequestHandler<GetByIdGroupMemberQueryRequest, GetByIdGroupMemberQueryResponse>
    {
        private readonly IGroupMemberRepository _repo;

        public GetByIdGroupMemberQueryHandler(GroupMemberRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetByIdGroupMemberQueryResponse> Handle(GetByIdGroupMemberQueryRequest request, CancellationToken cancellationToken)
        {
            GroupMember result = _repo.GetByIdAsync(request.Id).Result;
            GetByIdGroupMemberQueryResponse getByIdGroupMemberQueryResponse = new GetByIdGroupMemberQueryResponse()
            {
                Id = result.Id,
                User = result.User,
                Group = result.Group
            };

            return getByIdGroupMemberQueryResponse;
        }
    }
}
