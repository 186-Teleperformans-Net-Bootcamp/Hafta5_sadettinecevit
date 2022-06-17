using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetByIdGroupQueryRequest : IRequest<GetByIdGroupQueryResponse>
    {
        public string Id { get; set; }
    }
}
