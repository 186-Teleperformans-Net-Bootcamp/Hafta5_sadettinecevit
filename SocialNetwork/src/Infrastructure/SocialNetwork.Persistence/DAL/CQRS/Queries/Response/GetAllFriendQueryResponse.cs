using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllFriendQueryResponse
    {
        public string Id { get; set; }
        public User User { get; set; }
        public User FriendUser { get; set; }
    }
}
