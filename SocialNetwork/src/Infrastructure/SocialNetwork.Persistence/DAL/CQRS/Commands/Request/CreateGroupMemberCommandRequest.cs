using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateGroupMemberCommandRequest : IRequest<CreateGroupMemberCommandResponse>
	{
		public Group Group { get; set; }
		public User User { get; set; }
	}
}
