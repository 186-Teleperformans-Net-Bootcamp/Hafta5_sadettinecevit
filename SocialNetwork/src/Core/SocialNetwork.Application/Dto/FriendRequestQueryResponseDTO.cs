using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Application.Dto
{
    public class FriendRequestQueryResponseDTO
    {
        public string Id { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public bool Response { get; set; }
        public DateTime ResponseTime { get; set; }
    }

}
