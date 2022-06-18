using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQueryRequest, PaginingResponse<List<GetAllUserQueryResponse>>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;
        public GetAllUserQueryHandler(IDistributedCache distributedCache, UserManager<User> userManager)
        {
            _distributedCache = distributedCache;
            _userManager = userManager;
        }

        public async Task<PaginingResponse<List<GetAllUserQueryResponse>>> Handle(GetAllUserQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllUserQueryResponse>> getAllUserQueryResponse = new PaginingResponse<List<GetAllUserQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("users").Result;

            List<User> userManager = null;

            if (cachedBytes == null)
            {
                userManager = _userManager.Users.ToList();

                string jsonText = JsonSerializer.Serialize(userManager);
                _distributedCache.SetAsync("users", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                userManager = JsonSerializer.Deserialize<List<User>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                userManager = userManager.Where(x => x.Email.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.Name) || !string.IsNullOrWhiteSpace(request.LastName))
            {
                userManager = userManager.Where(x => x.Name == request.Name || x.LastName == request.LastName).ToList();
            }

            getAllUserQueryResponse.Total = userManager.Count();
            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllUserQueryResponse.Limit = request.Limit;
            getAllUserQueryResponse.Page = request.Page;
            getAllUserQueryResponse.Total = _userManager.Users.Count();
            getAllUserQueryResponse.TotalPage = (int)Math.Ceiling(getAllUserQueryResponse.Total / (double)getAllUserQueryResponse.Limit);
            getAllUserQueryResponse.HasPrevious = getAllUserQueryResponse.Page != 1;
            getAllUserQueryResponse.HasNext = getAllUserQueryResponse.Page != getAllUserQueryResponse.TotalPage;

            int skip = (getAllUserQueryResponse.Page - 1) * getAllUserQueryResponse.Limit;
            int take = getAllUserQueryResponse.Limit;

            getAllUserQueryResponse.Response = userManager.Select(u =>
            new GetAllUserQueryResponse
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Name = u.Name,
                LastName = u.LastName
            }).Skip(skip).Take(take).ToList();

            return getAllUserQueryResponse;
        }
    }
}
