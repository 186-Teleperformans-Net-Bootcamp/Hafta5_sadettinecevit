using SocialNetwork.Persistence.DAL.CQRS.Queries;

namespace SocialNetwork.Persistence.DAL.Filters
{
    public class CommentPaginingRequest : PaginingRequest
    {
        public string FromUserName { get; set; } = string.Empty;
        public string ToUserName { get; set; } = string.Empty;
    }
}
