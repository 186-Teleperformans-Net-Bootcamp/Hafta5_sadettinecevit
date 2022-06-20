using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdFriendRequestQueryHandler : IRequestHandler<GetByIdFriendRequestQueryRequest, GetByIdFriendRequestQueryResponse>
    {
        private readonly IFriendRequestRepository _repo;

        public GetByIdFriendRequestQueryHandler(FriendRequestRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetByIdFriendRequestQueryResponse> Handle(GetByIdFriendRequestQueryRequest request, CancellationToken cancellationToken)
        {
            FriendRequest result = _repo.GetByIdAsync(request.Id).Result;
            GetByIdFriendRequestQueryResponse getByIdFriendRequestQueryResponse = new GetByIdFriendRequestQueryResponse()
            {
                Id=request.Id,
                FromUser = result.FromUser,
                ToUser = result.ToUser,
                ResponseTime = result.ResponseTime,
                Response = result.Response
            };

            return getByIdFriendRequestQueryResponse;
        }
    }
}
