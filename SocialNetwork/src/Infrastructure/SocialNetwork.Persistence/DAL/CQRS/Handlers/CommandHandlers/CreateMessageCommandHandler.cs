using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommandRequest, CreateMessageCommandResponse>
    {
        private readonly IMessageRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public CreateMessageCommandHandler(IDistributedCache distributedCache, MessageRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<CreateMessageCommandResponse> Handle(CreateMessageCommandRequest createMessageCommandRequest, CancellationToken cancellationToken)
        {
            CreateMessageCommandResponse createMessageCommandResponse = new CreateMessageCommandResponse();

            EntityEntry<Message> result = await _repo.Add(
                new Message
                {
                    FromUser = createMessageCommandRequest.FromUser,
                    ImageURL = createMessageCommandRequest.ImageURL,
                    MessageText = createMessageCommandRequest.MessageText,
                    TimeToSent = DateTime.Now,
                    ToUsers = createMessageCommandRequest.ToUsers,
                    Type = createMessageCommandRequest.Type,
                    VideoURL = createMessageCommandRequest.VideoURL
                });

            createMessageCommandResponse.IsSuccess = result.State == EntityState.Added;
            createMessageCommandResponse.Message = result.Entity;

            if(createMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messages");
            }

            return createMessageCommandResponse;
        }
    }
}
