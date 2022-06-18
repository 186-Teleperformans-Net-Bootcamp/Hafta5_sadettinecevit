using SocialNetwork.Persistence.DAL.CQRS.Queries;

namespace SocialNetwork.Persistence.DAL.Filters
{
    public class GroupPaginingRequest : PaginingRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
