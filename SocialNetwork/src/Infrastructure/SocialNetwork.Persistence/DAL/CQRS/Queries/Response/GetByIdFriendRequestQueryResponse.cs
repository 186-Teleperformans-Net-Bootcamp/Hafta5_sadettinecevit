using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetByIdFriendRequestQueryResponse
    {
        public string Id { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public bool Response { get; set; }
        public DateTime ResponseTime { get; set; }
    }
}
