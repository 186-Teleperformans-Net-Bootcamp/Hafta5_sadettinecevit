using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Response
{
    public class CreateGroupMemberCommandResponse
    {
        public bool IsSuccess { get; set; }
        public GroupMember GroupMember { get; set; }
    }
}
