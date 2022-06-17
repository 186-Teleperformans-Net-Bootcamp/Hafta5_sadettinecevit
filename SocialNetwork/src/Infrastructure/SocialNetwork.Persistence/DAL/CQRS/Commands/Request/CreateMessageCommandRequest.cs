using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateMessageCommandRequest : IRequest<CreateMessageCommandResponse>
    {
        public MessageType Type { get; set; }
        public User FromUser { get; set; }
        public List<User> ToUsers { get; set; }
        public string MessageText { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
    }
}
