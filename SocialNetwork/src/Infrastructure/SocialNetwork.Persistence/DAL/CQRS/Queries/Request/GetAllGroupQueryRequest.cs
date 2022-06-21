using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetAllGroupQueryRequest : IRequest<GetAllGroupQueryResponse>
    {
        public int Limit { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string Keyword { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
