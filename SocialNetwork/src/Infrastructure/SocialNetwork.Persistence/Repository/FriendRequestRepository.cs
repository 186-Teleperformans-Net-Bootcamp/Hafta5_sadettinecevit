using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.Repository
{
    public class FriendRequestRepository : Repository<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
