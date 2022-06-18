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
    public class GetAllFriendRequestQueryHandler : IRequestHandler<GetAllFriendRequestQueryRequest, PaginingResponse<List<GetAllFriendRequestQueryResponse>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public GetAllFriendRequestQueryHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<PaginingResponse<List<GetAllFriendRequestQueryResponse>>> Handle(GetAllFriendRequestQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllFriendRequestQueryResponse>> getAllFriendRequestQueryResponse = new PaginingResponse<List<GetAllFriendRequestQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("friendRequest").Result;

            List<FriendRequest> context = null;

            if (cachedBytes == null)
            {
                context = _context.FriendRequests.ToList();

                string jsonText = JsonSerializer.Serialize(context);
                _distributedCache.SetAsync("friendRequest", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                context = JsonSerializer.Deserialize<List<FriendRequest>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                context = context.Where(x =>
                    x.FromUser.Name.Contains(request.Keyword) ||
                    x.ToUser.Name.Contains(request.Keyword) || // bu şekilde diğer property'leri de ekleyebiliriz.
                    x.ToUser.UserName.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.ToUserName) || !string.IsNullOrWhiteSpace(request.FromUserName))
            {
                context = context.Where(x => x.FromUser.Name.Contains(request.ToUserName) || x.ToUser.Name.Contains(request.ToUserName)).ToList();
            }

            getAllFriendRequestQueryResponse.Total = context.Count();
            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllFriendRequestQueryResponse.Limit = request.Limit;
            getAllFriendRequestQueryResponse.Page = request.Page;
            getAllFriendRequestQueryResponse.Total = _context.FriendRequests.Count();
            getAllFriendRequestQueryResponse.TotalPage = (int)Math.Ceiling(getAllFriendRequestQueryResponse.Total / (double)getAllFriendRequestQueryResponse.Limit);
            getAllFriendRequestQueryResponse.HasPrevious = getAllFriendRequestQueryResponse.Page != 1;
            getAllFriendRequestQueryResponse.HasNext = getAllFriendRequestQueryResponse.Page != getAllFriendRequestQueryResponse.TotalPage;

            int skip = (getAllFriendRequestQueryResponse.Page - 1) * getAllFriendRequestQueryResponse.Limit;
            int take = getAllFriendRequestQueryResponse.Limit;

            getAllFriendRequestQueryResponse.Response = context.Select(f =>
            new GetAllFriendRequestQueryResponse
            {
                Id = f.Id,
                FromUser = f.FromUser,
                Response = f.Response,
                ResponseTime = f.ResponseTime,
                ToUser = f.ToUser
            }).Skip(skip).Take(take).ToList();

            return getAllFriendRequestQueryResponse;
        }
    }
}
