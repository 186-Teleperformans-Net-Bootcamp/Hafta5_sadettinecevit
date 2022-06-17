using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateUserCommandResponse
    {
        public bool IsSuccess { get; set; }
        public User User { get; set; }
    }
}
