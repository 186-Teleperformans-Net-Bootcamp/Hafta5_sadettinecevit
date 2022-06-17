using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class GroupMember : IBaseEntity
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		[Required]
		public Group Group { get; set; }
		[Required]
		public User User { get; set; }
	}
}
