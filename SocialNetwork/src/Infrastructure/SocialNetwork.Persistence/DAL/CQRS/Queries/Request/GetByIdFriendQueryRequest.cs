using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetByIdFriendQueryRequest : IRequest<GetByIdFriendQueryResponse>
    {
        public string Id { get; set; }
    }
}
