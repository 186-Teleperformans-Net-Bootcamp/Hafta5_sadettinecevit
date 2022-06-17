using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.Repository
{
    public class GroupMemberRepository : Repository<GroupMember>, IGroupMemberRepository
    {
        public GroupMemberRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
