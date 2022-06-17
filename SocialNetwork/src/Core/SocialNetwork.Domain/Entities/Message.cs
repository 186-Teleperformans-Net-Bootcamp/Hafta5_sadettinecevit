using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class Message : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public MessageType Type { get; set; }
        [Required]
        public User FromUser { get; set; }
        [Required]
        public List<User> ToUsers { get; set; }
        public DateTime TimeToSent { get; set; } = DateTime.Now;
        public string MessageText { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }

    }
}
