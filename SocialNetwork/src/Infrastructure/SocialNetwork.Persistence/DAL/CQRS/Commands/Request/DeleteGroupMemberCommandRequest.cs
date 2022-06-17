using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteGroupMemberCommandRequest : IRequest<DeleteGroupMemberCommandResponse>
    {
        public string Id { get; set; }
    }
}
