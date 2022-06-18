using SocialNetwork.Persistence.DAL.CQRS.Queries;

namespace SocialNetwork.Persistence.DAL.Filters
{
    public class MessagePaginingRequest : PaginingRequest
    {
        public string Type { get; set; } = string.Empty;
        public string FromUserName { get; set; } = string.Empty;
        public string ToUserName { get; set; } = string.Empty;
        public DateTime? TimeToSent { get; set; }
    }
}
