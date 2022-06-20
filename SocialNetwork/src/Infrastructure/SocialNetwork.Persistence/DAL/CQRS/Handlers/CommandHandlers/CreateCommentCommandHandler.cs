using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        private readonly ICommentRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public CreateCommentCommandHandler(CommentRepository repo, IDistributedCache distributedCache)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest createCommentCommandRequest, CancellationToken cancellationToken)
        {
            CreateCommentCommandResponse createCommentCommandResponse = new CreateCommentCommandResponse();

            var result = await _repo.Add(
                new Comment
                {
                    FromUser = createCommentCommandRequest.FromUser,
                    ToUser = createCommentCommandRequest.ToUser,
                    CommentText = createCommentCommandRequest.CommentText,
                    CommentTime = DateTime.Now,
                    IsPrivate = createCommentCommandRequest.IsPrivate
                });

            createCommentCommandResponse.IsSuccess = result.State == EntityState.Added;
            createCommentCommandResponse.Comment = result.Entity;

            if(createCommentCommandResponse.IsSuccess)
            {
                await _distributedCache.RemoveAsync("comments");
            }

            return createCommentCommandResponse;
        }
    }
}
