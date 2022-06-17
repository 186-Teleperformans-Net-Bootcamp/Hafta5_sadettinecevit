using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateMessageTypeCommandResponse
    {
        public bool IsSuccess { get; set; }
        public MessageType MessageType { get; set; }
    }
}
