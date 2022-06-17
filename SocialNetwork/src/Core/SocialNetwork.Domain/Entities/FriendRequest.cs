using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    //Kullanıcı response cevabına göre veri silinecek. İlk istekte otomatik eklenecek.
    public class FriendRequest : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public User FromUser { get; set; }
        [Required]
        public User ToUser { get; set; }
        public DateTime RequestTime { get; set; } = DateTime.Now;
        public bool	Response { get; set; }
        public DateTime ResponseTime { get; set; }
    }
}
