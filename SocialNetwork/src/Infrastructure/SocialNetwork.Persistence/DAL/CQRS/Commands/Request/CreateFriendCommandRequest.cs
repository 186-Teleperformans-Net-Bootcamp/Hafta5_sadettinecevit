using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateFriendCommandRequest : IRequest<CreateFriendCommandResponse>
    {
        public User User { get; set; }
        public User FriendUser { get; set; }
    }
}
