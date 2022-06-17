using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
