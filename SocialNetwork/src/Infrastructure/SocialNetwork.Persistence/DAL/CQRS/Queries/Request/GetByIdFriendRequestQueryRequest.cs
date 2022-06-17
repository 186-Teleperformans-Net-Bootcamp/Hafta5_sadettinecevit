using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetByIdFriendRequestQueryRequest : IRequest<GetByIdFriendRequestQueryResponse>
    {
        public string Id { get; set; }
    }
}
