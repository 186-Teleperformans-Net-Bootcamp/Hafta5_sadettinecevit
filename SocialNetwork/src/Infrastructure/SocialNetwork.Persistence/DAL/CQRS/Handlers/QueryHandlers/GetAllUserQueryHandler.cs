using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQueryRequest, GetAllUserQueryResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;
        public GetAllUserQueryHandler(IDistributedCache distributedCache, UserManager<User> userManager)
        {
            _distributedCache = distributedCache;
            _userManager = userManager;
        }

        public async Task<GetAllUserQueryResponse> Handle(GetAllUserQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllUserQueryResponse getAllUserQueryResponse = new GetAllUserQueryResponse();

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

            getAllUserQueryResponse.MaxPage = (int)Math.Ceiling(userManager.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllUserQueryResponse.ListUserQueryResponse = userManager.Select(u =>
            new UserQueryResponseDTO
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
