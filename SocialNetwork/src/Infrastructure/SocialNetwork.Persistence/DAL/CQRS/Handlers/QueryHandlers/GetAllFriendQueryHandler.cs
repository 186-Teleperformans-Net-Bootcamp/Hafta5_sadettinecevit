using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllFriendQueryHandler : IRequestHandler<GetAllFriendQueryRequest, PaginingResponse<List<GetAllFriendQueryResponse>>>
    {
        private readonly IFriendRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public GetAllFriendQueryHandler(IDistributedCache distributedCache, FriendRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<PaginingResponse<List<GetAllFriendQueryResponse>>> Handle(GetAllFriendQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllFriendQueryResponse>> getAllFriendQueryResponse = new PaginingResponse<List<GetAllFriendQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("friends").Result;

            List<Friend> context = null;

            if (cachedBytes == null)
            {
                context = _repo.GetAsync().Result;

                string jsonText = JsonSerializer.Serialize(context);
                _distributedCache.SetAsync("friends", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                context = JsonSerializer.Deserialize<List<Friend>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                context = context.Where(x =>
                    x.FriendUser.UserName.Contains(request.Keyword) ||
                    x.FriendUser.Name.Contains(request.Keyword) || // bu şekilde diğer property'leri de ekleyebiliriz.
                    x.FriendUser.LastName.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.FriendUserName) || !string.IsNullOrWhiteSpace(request.UserName))
            {
                context = context.Where(x => x.FriendUser.Name == request.FriendUserName || x.User.Name == request.UserName).ToList();
            }

            getAllFriendQueryResponse.Total = context.Count();
            // bu kısım kendini tekrar ediyor bir metot olabilirdi.
            getAllFriendQueryResponse.Limit = request.Limit;
            getAllFriendQueryResponse.Page = request.Page;
            getAllFriendQueryResponse.TotalPage = (int)Math.Ceiling(getAllFriendQueryResponse.Total / (double)getAllFriendQueryResponse.Limit);
            getAllFriendQueryResponse.HasPrevious = getAllFriendQueryResponse.Page != 1;
            getAllFriendQueryResponse.HasNext = getAllFriendQueryResponse.Page != getAllFriendQueryResponse.TotalPage;

            int skip = (getAllFriendQueryResponse.Page - 1) * getAllFriendQueryResponse.Limit;
            int take = getAllFriendQueryResponse.Limit;

            getAllFriendQueryResponse.Response = context.Select(f => 
            new GetAllFriendQueryResponse
            {
                Id = f.Id,
                FriendUser = f.FriendUser,
                User = f.User
            }).Skip(skip).Take(take).ToList();

            return getAllFriendQueryResponse;
        }
    }
}
