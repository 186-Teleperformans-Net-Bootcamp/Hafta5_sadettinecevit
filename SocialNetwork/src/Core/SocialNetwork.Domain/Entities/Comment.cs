using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class Comment : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
		public User FromUser { get; set; }
        [Required]
        public User ToUser { get; set; }
        public bool IsPrivate { get; set; }
        [Required]
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.Now;
    }
}
