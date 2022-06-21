using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Application.Dto
{
    public class MessageQueryResponseDTO
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
