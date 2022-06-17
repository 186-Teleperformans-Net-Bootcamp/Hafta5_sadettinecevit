using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteFriendRequestCommandRequest : IRequest<DeleteFriendRequestCommandResponse>
    {
        public string Id { get; set; }
    }
}
