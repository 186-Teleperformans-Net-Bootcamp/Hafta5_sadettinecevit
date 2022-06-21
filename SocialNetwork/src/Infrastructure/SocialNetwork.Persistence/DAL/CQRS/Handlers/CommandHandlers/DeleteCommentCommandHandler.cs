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
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommandRequest, DeleteCommentCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public DeleteCommentCommandHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommandRequest deleteCommentCommandRequest, CancellationToken cancellationToken)
        {
            DeleteCommentCommandResponse deleteCommentCommandResponse = new DeleteCommentCommandResponse();

            Comment comment = _unitOfWork.CommentRepository.GetAsync().Result.FirstOrDefault(c => c.Id == deleteCommentCommandRequest.Id);
            EntityEntry<Comment> result = null;

            using IDbContextTransaction retVal = await _unitOfWork.BeginTansactionAsync();

            try
            {
                result = _unitOfWork.CommentRepository.Delete(comment).Result;
                deleteCommentCommandResponse.IsSuccess = retVal.CommitAsync().IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                await retVal.RollbackAsync();
            }

            if (deleteCommentCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("comments");
            }

            return deleteCommentCommandResponse;
        }
    }
}
