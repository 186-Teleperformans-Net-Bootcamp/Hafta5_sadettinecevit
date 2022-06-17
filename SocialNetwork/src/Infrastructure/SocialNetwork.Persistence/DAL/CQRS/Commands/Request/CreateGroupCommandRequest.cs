using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateGroupCommandRequest : IRequest<CreateGroupCommandResponse>
    {
        public string Name { get; set; }
    }
}
