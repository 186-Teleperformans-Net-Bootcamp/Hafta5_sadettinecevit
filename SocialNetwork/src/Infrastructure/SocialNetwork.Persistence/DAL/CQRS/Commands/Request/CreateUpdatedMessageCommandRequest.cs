using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateUpdatedMessageCommandRequest : IRequest<CreateUpdatedMessageCommandResponse>
    {
        public Message Message { get; set; }
        public MessageType OldMessageType { get; set; }
        public DateTime SendTime { get; set; }
        public string OldMessageText { get; set; }
        public string OldImageURL { get; set; }
        public string OldVideoURL { get; set; }
    }
}
