using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateFriendCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Friend Friend { get; set; }
    }
}
