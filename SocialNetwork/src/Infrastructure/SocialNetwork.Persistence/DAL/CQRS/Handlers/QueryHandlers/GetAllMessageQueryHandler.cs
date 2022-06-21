using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Dto;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllMessageQueryHandler : IRequestHandler<GetAllMessageQueryRequest, GetAllMessageQueryResponse>
    {
        private readonly IUnitOfWork _repo;
        private readonly IDistributedCache _distributedCache;
        public GetAllMessageQueryHandler(IDistributedCache distributedCache, IUnitOfWork repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<GetAllMessageQueryResponse> Handle(GetAllMessageQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllMessageQueryResponse getAllMessageQueryResponse = new GetAllMessageQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("messages").Result;

            List<Message> messages = null;

            if (cachedBytes == null)
            {
                messages = _repo.MessageRepository.GetAsync().Result;

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

            getAllMessageQueryResponse.MaxPage = (int)Math.Ceiling(messages.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllMessageQueryResponse.ListMessageQueryResponse = messages.Select(m => 
            new MessageQueryResponseDTO
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
