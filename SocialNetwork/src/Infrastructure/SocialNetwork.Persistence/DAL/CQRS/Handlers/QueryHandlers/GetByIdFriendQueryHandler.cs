using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdFriendQueryHandler : IRequestHandler<GetByIdFriendQueryRequest, GetByIdFriendQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdFriendQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdFriendQueryResponse> Handle(GetByIdFriendQueryRequest request, CancellationToken cancellationToken)
        {
            Friend result = _context.Friends.FirstOrDefault(f => f.Id == request.Id);
            GetByIdFriendQueryResponse getByIdFriendQueryResponse = new GetByIdFriendQueryResponse()
            {
                Id = request.Id,
                FriendUser = result.FriendUser,
                User = result.User
            };

            return getByIdFriendQueryResponse;
        }
    }
}
