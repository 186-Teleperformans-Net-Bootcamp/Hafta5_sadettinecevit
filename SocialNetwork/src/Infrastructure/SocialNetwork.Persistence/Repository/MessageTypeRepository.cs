using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.Repository
{
    public class MessageTypeRepository : Repository<MessageType>, IMessageTypeRepository
    {
        public MessageTypeRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
