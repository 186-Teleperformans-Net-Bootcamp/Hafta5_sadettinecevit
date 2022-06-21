using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetAllFriendQueryRequest : IRequest<GetAllFriendQueryResponse>
    {
        public int Limit { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string Keyword { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FriendUserName { get; set; } = string.Empty;
    }
}
