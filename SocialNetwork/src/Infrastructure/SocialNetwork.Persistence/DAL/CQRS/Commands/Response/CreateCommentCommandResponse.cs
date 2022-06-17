using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateCommentCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Comment Comment { get; set; }
    }
}
