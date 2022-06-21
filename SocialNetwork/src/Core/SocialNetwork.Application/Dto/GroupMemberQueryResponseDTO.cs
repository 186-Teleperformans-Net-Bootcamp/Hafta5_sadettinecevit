using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Application.Dto
{
    public class GroupMemberQueryResponseDTO
    {
        public string Id { get; set; }
        public Group Group { get; set; }
        public User User { get; set; }
    }

}
