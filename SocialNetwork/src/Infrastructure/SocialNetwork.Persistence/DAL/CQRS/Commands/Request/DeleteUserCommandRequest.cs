using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteUserCommandRequest : IRequest<DeleteUserCommandResponse>
	{
		public string Id { get; set; }
	}
}
