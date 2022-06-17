using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateMessageCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Message Message { get; set; }
    }
}
