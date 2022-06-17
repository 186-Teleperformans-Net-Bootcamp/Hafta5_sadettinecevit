using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetByIdGroupMemberQueryRequest : IRequest<GetByIdGroupMemberQueryResponse>
    {
        public string Id { get; set; }
    }
}
