using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Application.Dto
{
    public class FriendQueryResponseDTO
    {
        public string Id { get; set; }
        public User User { get; set; }
        public User FriendUser { get; set; }
    }

}
