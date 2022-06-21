using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest createCommentCommandRequest, CancellationToken cancellationToken)
        {
            CreateCommentCommandResponse createCommentCommandResponse = new CreateCommentCommandResponse();
            EntityEntry<Comment> result = null;
            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();
            try
            {
                result = await _unitOfWork.CommentRepository.Add(
                    new Comment
                    {
                        FromUser = createCommentCommandRequest.FromUser,
                        ToUser = createCommentCommandRequest.ToUser,
                        CommentText = createCommentCommandRequest.CommentText,
                        CommentTime = DateTime.Now,
                        IsPrivate = createCommentCommandRequest.IsPrivate
                    });
                createCommentCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            createCommentCommandResponse.Comment = result?.Entity;
            if (createCommentCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("comments");
            }

            return createCommentCommandResponse;
        }
    }
}
