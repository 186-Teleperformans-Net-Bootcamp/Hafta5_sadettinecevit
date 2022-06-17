using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllFriendQueryHandler : IRequestHandler<GetAllFriendQueryRequest, List<GetAllFriendQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllFriendQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllFriendQueryResponse>> Handle(GetAllFriendQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllFriendQueryResponse> getAllFriendQueryResponse = new List<GetAllFriendQueryResponse>();

            getAllFriendQueryResponse = _context.Friends.Select(f => 
            new GetAllFriendQueryResponse
            {
                Id = f.Id,
                FriendUser = f.FriendUser,
                User = f.User
            }).ToList();

            return getAllFriendQueryResponse;
        }
    }
}
