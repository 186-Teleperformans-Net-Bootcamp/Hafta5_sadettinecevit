using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllMessageQueryResponse
    {
        public string Id { get; set; }
        public MessageType Type { get; set; }
        public User FromUser { get; set; }
        public List<User> ToUsers { get; set; }
        public string MessageText { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
    }
}
