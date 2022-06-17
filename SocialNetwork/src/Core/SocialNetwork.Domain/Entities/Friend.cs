using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class Friend : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public User User { get; set; }
        [Required]
        public User FriendUser { get; set; }
        public DateTime TimeToBeFriend { get; set; } = DateTime.Now;
    }
}
