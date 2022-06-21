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
    public class GetAllFriendQueryHandler : IRequestHandler<GetAllFriendQueryRequest, GetAllFriendQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public GetAllFriendQueryHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllFriendQueryResponse> Handle(GetAllFriendQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllFriendQueryResponse getAllFriendQueryResponse = new GetAllFriendQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("friends").Result;

            List<Friend> context = null;

            if (cachedBytes == null)
            {
                context = _unitOfWork.FriendRepository.GetAsync().Result;

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

            getAllFriendQueryResponse.MaxPage = (int)Math.Ceiling(context.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllFriendQueryResponse.ListFriendQueryResponse = context.Select(f => 
            new FriendQueryResponseDTO
            {
                Id = f.Id,
                FriendUser = f.FriendUser,
                User = f.User
            }).Skip(skip).Take(take).ToList();

            return getAllFriendQueryResponse;
        }
    }
}
