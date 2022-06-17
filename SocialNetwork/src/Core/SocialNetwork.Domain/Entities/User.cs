using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class User : IdentityUser, IBaseEntity
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string LastName { get; set; }
	}
}
