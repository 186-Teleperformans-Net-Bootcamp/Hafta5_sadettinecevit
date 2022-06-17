using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteCommentCommandRequest : IRequest<DeleteCommentCommandResponse>
    {
        public string Id { get; set; }
    }
}
