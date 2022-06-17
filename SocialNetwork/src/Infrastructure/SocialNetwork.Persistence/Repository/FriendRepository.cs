using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.Repository
{
    public class FriendRepository : Repository<Friend>, IFriendRepository
    {
        public FriendRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
