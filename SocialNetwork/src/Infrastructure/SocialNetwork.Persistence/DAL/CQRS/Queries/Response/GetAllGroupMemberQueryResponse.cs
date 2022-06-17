using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllGroupMemberQueryResponse
    {
        public string Id { get; set; }
        public Group Group { get; set; }
        public User User { get; set; }
    }
}
