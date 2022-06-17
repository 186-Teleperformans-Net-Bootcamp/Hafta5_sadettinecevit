using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateFriendRequestCommandResponse
    {
        public bool IsSuccess { get; set; }
        public FriendRequest FriendRequest { get; set; }
    }
}
