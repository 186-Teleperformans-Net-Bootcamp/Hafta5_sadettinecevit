using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdFriendRequestQueryHandler : IRequestHandler<GetByIdFriendRequestQueryRequest, GetByIdFriendRequestQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdFriendRequestQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdFriendRequestQueryResponse> Handle(GetByIdFriendRequestQueryRequest request, CancellationToken cancellationToken)
        {
            FriendRequest result = _context.FriendRequests.FirstOrDefault(f => f.Id == request.Id);
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
