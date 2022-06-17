using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteFriendCommandRequest : IRequest<DeleteFriendCommandResponse>
    {
        public string Id { get; set; }
    }
}
