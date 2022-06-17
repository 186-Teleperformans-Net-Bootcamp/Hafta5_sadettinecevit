using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllFriendRequestQueryHandler : IRequestHandler<GetAllFriendRequestQueryRequest, List<GetAllFriendRequestQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllFriendRequestQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllFriendRequestQueryResponse>> Handle(GetAllFriendRequestQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllFriendRequestQueryResponse> response = new List<GetAllFriendRequestQueryResponse>();
            
            response = _context.FriendRequests.Select(f => 
            new GetAllFriendRequestQueryResponse
            {
                Id=f.Id,
                FromUser = f.FromUser,
                Response = f.Response,
                ResponseTime = f.ResponseTime,
                ToUser = f.ToUser
            }).ToList();

            return response;
        }
    }
}
