using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.Repository
{
    public class UpdatedMessageRepository : Repository<UpdatedMessage>, IUpdatedMessageRepository
    {
        public UpdatedMessageRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
