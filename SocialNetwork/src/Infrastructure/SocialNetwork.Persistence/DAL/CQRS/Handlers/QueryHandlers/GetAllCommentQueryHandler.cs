using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllCommentQueryHandler : IRequestHandler<GetAllCommentQueryRequest, PaginingResponse<List<GetAllCommentQueryResponse>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public GetAllCommentQueryHandler(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public async Task<PaginingResponse<List<GetAllCommentQueryResponse>>> Handle(GetAllCommentQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllCommentQueryResponse>> getAllCommentQueryResponse = new PaginingResponse<List<GetAllCommentQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("comments").Result;

            List<Comment> context = null;

            if(cachedBytes == null)
            {
                context = _context.Comments.ToList();

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

            getAllCommentQueryResponse.Total = context.Count();
            getAllCommentQueryResponse.Limit = request.Limit;
            getAllCommentQueryResponse.Page = request.Page;
            getAllCommentQueryResponse.TotalPage = (int)Math.Ceiling(getAllCommentQueryResponse.Total / (double)getAllCommentQueryResponse.Limit);
            getAllCommentQueryResponse.HasPrevious = getAllCommentQueryResponse.Page != 1;
            getAllCommentQueryResponse.HasNext = getAllCommentQueryResponse.Page != getAllCommentQueryResponse.TotalPage;

            int skip = (getAllCommentQueryResponse.Page - 1) * getAllCommentQueryResponse.Limit;
            int take = getAllCommentQueryResponse.Limit;

            getAllCommentQueryResponse.Response = context.Select(c => 
            new GetAllCommentQueryResponse
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
