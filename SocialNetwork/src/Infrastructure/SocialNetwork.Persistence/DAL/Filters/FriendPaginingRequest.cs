using SocialNetwork.Persistence.DAL.CQRS.Queries;

namespace SocialNetwork.Persistence.DAL.Filters
{
    public class FriendPaginingRequest : PaginingRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string FriendUserName { get; set; } = string.Empty;
    }
}
