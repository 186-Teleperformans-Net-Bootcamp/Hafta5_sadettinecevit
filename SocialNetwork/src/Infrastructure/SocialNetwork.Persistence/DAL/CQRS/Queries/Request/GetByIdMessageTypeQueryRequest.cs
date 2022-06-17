using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetByIdMessageTypeQueryRequest : IRequest<GetByIdMessageTypeQueryResponse>
    {
        public string Id { get; set; }
    }
}
