using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdFriendQueryHandler : IRequestHandler<GetByIdFriendQueryRequest, GetByIdFriendQueryResponse>
    {
        private readonly IFriendRepository _context;

        public GetByIdFriendQueryHandler(IFriendRepository context)
        {
            _context = context;
        }

        public async Task<GetByIdFriendQueryResponse> Handle(GetByIdFriendQueryRequest request, CancellationToken cancellationToken)
        {
            Friend result = _context.GetByIdAsync(request.Id).Result;
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
