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
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllFriendRequestQueryHandler : IRequestHandler<GetAllFriendRequestQueryRequest, GetAllFriendRequestQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public GetAllFriendRequestQueryHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllFriendRequestQueryResponse> Handle(GetAllFriendRequestQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllFriendRequestQueryResponse getAllFriendRequestQueryResponse = new GetAllFriendRequestQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("friendRequest").Result;

            List<FriendRequest> context = null;

            if (cachedBytes == null)
            {
                context = _unitOfWork.FriendRequestRepository.GetAsync().Result;

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

            getAllFriendRequestQueryResponse.MaxPage = (int)Math.Ceiling(context.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllFriendRequestQueryResponse.ListFriendRequestQueryResponse = context.Select(f =>
            new FriendRequestQueryResponseDTO
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
