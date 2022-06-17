using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class Group : IBaseEntity
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		[Required]
		public string Name { get; set; }
	}
}
