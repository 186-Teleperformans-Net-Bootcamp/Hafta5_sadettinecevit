using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateFriendRequestCommandRequest : IRequest<CreateFriendRequestCommandResponse>
    {
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public bool Response { get; set; }
        public DateTime ResponseTime { get; set; }
    }
}
