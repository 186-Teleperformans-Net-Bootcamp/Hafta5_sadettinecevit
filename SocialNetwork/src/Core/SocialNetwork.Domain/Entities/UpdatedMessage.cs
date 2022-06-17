using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class UpdatedMessage : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public Message Message { get; set; }
        [Required]
        public MessageType OldMessageType { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public string OldMessageText { get; set; }
        public string OldImageURL { get; set; }
        public string OldVideoURL { get; set; }

    }
}
