using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateMessageTypeCommandRequest : IRequest<CreateMessageTypeCommandResponse>
    {
        public string Type { get; set; }
    }
}
