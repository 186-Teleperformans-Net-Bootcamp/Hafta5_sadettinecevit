using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Dto;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;
using SocialNetwork.Persistence.UnitOfWork;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllCommentQueryHandler : IRequestHandler<GetAllCommentQueryRequest, GetAllCommentQueryResponse>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCommentQueryHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<GetAllCommentQueryResponse> Handle(GetAllCommentQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllCommentQueryResponse getAllCommentQueryResponse = new GetAllCommentQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("comments").Result;

            List<Comment> context = null;

            if(cachedBytes == null)
            {
                context = _unitOfWork.CommentRepository.GetAsync().Result;

                string jsonText = JsonSerializer.Serialize(context);
                _distributedCache.SetAsync("comments", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                context = JsonSerializer.Deserialize<List<Comment>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                context = context.Where(x =>
                    x.CommentText.Contains(request.Keyword) ||
                    x.FromUser.Name.Contains(request.Keyword) || // bu şekilde diğer property'leri de ekleyebiliriz.
                    x.ToUser.Name.Contains(request.Keyword)).ToList();
            }

            if(!string.IsNullOrWhiteSpace(request.ToUserName) || !string.IsNullOrWhiteSpace(request.FromUserName))
            {
                context = context.Where(x => x.FromUser.Name.Contains(request.ToUserName) || x.ToUser.Name.Contains(request.ToUserName)).ToList();
            }

            getAllCommentQueryResponse.MaxPage = (int)Math.Ceiling(context.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllCommentQueryResponse.ListCommentQueryResponse = context.Select(c => 
            new CommentQueryResponseDTO
             {
                 CommentText = c.CommentText,
                 FromUser = c.FromUser,
                 ToUser = c.ToUser,
                 IsPrivate = c.IsPrivate,
                 Id = c.Id
             }).Skip(skip).Take(take).ToList();

            return getAllCommentQueryResponse;
        }
    }
}
