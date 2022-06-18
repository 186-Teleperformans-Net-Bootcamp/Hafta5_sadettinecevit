using SocialNetwork.Persistence.DAL.CQRS.Queries;

namespace SocialNetwork.Persistence.DAL.Filters
{
    public class GroupMemberPaginingRequest : PaginingRequest
    {
        public string GroupName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
