using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateUpdatedMessageCommandResponse
    {
        public bool IsSuccess { get; set; }
        public UpdatedMessage UpdatedMessage { get; set; }
    }
}
