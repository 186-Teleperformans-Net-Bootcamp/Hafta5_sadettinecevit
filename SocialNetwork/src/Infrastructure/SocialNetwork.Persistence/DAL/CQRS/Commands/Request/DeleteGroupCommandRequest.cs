using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteGroupCommandRequest : IRequest<DeleteGroupCommandResponse>
    {
        public string Id { get; set; }
    }
}
