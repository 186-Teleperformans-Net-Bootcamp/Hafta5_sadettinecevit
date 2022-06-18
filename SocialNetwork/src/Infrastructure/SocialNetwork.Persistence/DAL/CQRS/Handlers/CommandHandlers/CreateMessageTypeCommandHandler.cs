using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateMessageTypeCommandHandler : IRequestHandler<CreateMessageTypeCommandRequest, CreateMessageTypeCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public CreateMessageTypeCommandHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<CreateMessageTypeCommandResponse> Handle(CreateMessageTypeCommandRequest createMessageTypeCommandRequest, CancellationToken cancellationToken)
        {
            CreateMessageTypeCommandResponse createMessageTypeCommandResponse = new CreateMessageTypeCommandResponse();

            var result = _context.MessageTypes.Add(
                new MessageType
                {
                    Type = createMessageTypeCommandRequest.Type
                });

            createMessageTypeCommandResponse.IsSuccess = result.State == EntityState.Added;
            createMessageTypeCommandResponse.MessageType = result.Entity;

            if(createMessageTypeCommandResponse.IsSuccess)
            {
                _context.SaveChangesAsync();
                _distributedCache.Remove("messageTypes");
            }

            return createMessageTypeCommandResponse;
        }
    }
}
