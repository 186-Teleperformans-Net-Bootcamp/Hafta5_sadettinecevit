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
    public class GetAllMessageQueryHandler : IRequestHandler<GetAllMessageQueryRequest, PaginingResponse<List<GetAllMessageQueryResponse>>>
    {
        private readonly IMessageRepository _repo;
        private readonly IDistributedCache _distributedCache;
        public GetAllMessageQueryHandler(IDistributedCache distributedCache, MessageRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<PaginingResponse<List<GetAllMessageQueryResponse>>> Handle(GetAllMessageQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllMessageQueryResponse>> getAllMessageQueryResponse = new PaginingResponse<List<GetAllMessageQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("messages").Result;

            List<Message> messages = null;

            if (cachedBytes == null)
            {
                messages = _repo.GetAsync().Result;

                string jsonText = JsonSerializer.Serialize(messages);
                _distributedCache.SetAsync("messages", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                messages = JsonSerializer.Deserialize<List<Message>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                messages = messages.Where(x => x.MessageText.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.ToUserName) || !string.IsNullOrWhiteSpace(request.FromUserName))
            {
                messages = messages.Where(x => x.FromUser.Name == request.ToUserName && x.ToUsers.Any(k => k.UserName == request.ToUserName)).ToList();
            }

            getAllMessageQueryResponse.Total = messages.Count();
            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllMessageQueryResponse.Limit = request.Limit;
            getAllMessageQueryResponse.Page = request.Page;
            getAllMessageQueryResponse.TotalPage = (int)Math.Ceiling(getAllMessageQueryResponse.Total / (double)getAllMessageQueryResponse.Limit);
            getAllMessageQueryResponse.HasPrevious = getAllMessageQueryResponse.Page != 1;
            getAllMessageQueryResponse.HasNext = getAllMessageQueryResponse.Page != getAllMessageQueryResponse.TotalPage;

            int skip = (getAllMessageQueryResponse.Page - 1) * getAllMessageQueryResponse.Limit;
            int take = getAllMessageQueryResponse.Limit;

            getAllMessageQueryResponse.Response = messages.Select(m => 
            new GetAllMessageQueryResponse
            {
                Id = m.Id,
                FromUser = m.FromUser,
                ImageURL = m.ImageURL,
                MessageText = m.MessageText,
                ToUsers = m.ToUsers,
                Type = m.Type,
                VideoURL = m.VideoURL
            }).Skip(skip).Take(take).ToList();

            return getAllMessageQueryResponse;
        }
    }
}
