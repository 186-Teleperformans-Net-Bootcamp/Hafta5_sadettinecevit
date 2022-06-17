using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
	{
		public string UserName { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
