using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteMessageTypeCommandRequest : IRequest<DeleteMessageTypeCommandResponse>
    {
        public string Id { get; set; }
    }
}
