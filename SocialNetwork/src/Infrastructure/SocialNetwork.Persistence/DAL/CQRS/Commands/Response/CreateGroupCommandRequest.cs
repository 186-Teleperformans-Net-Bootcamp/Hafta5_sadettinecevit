using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateGroupCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Group Group { get; set; }
    }
}
