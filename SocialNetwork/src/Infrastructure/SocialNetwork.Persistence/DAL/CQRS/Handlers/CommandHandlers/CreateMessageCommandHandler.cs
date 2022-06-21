using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommandRequest, CreateMessageCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public CreateMessageCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateMessageCommandResponse> Handle(CreateMessageCommandRequest createMessageCommandRequest, CancellationToken cancellationToken)
        {
            CreateMessageCommandResponse createMessageCommandResponse = new CreateMessageCommandResponse();

            EntityEntry<Message> result = null;
            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            try
            {
                result = await _unitOfWork.MessageRepository.Add(
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
                createMessageCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            createMessageCommandResponse.Message = result?.Entity;

            if (createMessageCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("messages");
            }

            return createMessageCommandResponse;
        }
    }
}
