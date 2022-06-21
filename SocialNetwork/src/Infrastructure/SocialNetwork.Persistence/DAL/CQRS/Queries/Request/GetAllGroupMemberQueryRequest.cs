using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetAllGroupMemberQueryRequest : IRequest<GetAllGroupMemberQueryResponse>
    {
        public int Limit { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string Keyword { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
