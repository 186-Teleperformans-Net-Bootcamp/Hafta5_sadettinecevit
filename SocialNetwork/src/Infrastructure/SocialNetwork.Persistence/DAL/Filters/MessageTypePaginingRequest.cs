using SocialNetwork.Persistence.DAL.CQRS.Queries;

namespace SocialNetwork.Persistence.DAL.Filters
{
    public class MessageTypePaginingRequest : PaginingRequest
    {
        public string Type { get; set; } = string.Empty;
    }
}
