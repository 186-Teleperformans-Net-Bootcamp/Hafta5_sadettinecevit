using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class MessageType : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Type { get; set; }
    }
}
