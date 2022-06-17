using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetByIdUserQueryRequest : IRequest<GetByIdUserQueryResponse>
    {
        public string Id { get; set; }
    }
}
