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
    public class GetAllMessageTypeQueryHandler : IRequestHandler<GetAllMessageTypeQueryRequest, PaginingResponse<List<GetAllMessageTypeQueryResponse>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public GetAllMessageTypeQueryHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<PaginingResponse<List<GetAllMessageTypeQueryResponse>>> Handle(GetAllMessageTypeQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllMessageTypeQueryResponse>> getAllMessageTypeQueryResponse = new PaginingResponse<List<GetAllMessageTypeQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("messageTypes").Result;

            List<MessageType> context = null;

            if (cachedBytes == null)
            {
                context = _context.MessageTypes.ToList();

                string jsonText = JsonSerializer.Serialize(context);
                _distributedCache.SetAsync("messageTypes", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                context = JsonSerializer.Deserialize<List<MessageType>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                context = context.Where(x => x.Type.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                context = context.Where(x => x.Type == request.Type).ToList();
            }

            getAllMessageTypeQueryResponse.Total = context.Count();

            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllMessageTypeQueryResponse.Limit = request.Limit;
            getAllMessageTypeQueryResponse.Page = request.Page;
            getAllMessageTypeQueryResponse.Total = _context.MessageTypes.Count();
            getAllMessageTypeQueryResponse.TotalPage = (int)Math.Ceiling(getAllMessageTypeQueryResponse.Total / (double)getAllMessageTypeQueryResponse.Limit);
            getAllMessageTypeQueryResponse.HasPrevious = getAllMessageTypeQueryResponse.Page != 1;
            getAllMessageTypeQueryResponse.HasNext = getAllMessageTypeQueryResponse.Page != getAllMessageTypeQueryResponse.TotalPage;

            int skip = (getAllMessageTypeQueryResponse.Page - 1) * getAllMessageTypeQueryResponse.Limit;
            int take = getAllMessageTypeQueryResponse.Limit;

            getAllMessageTypeQueryResponse.Response = context.Select(m =>
            new GetAllMessageTypeQueryResponse
            {
                Id = m.Id,
                Type = m.Type
            }).Skip(skip).Take(take).ToList();

            return getAllMessageTypeQueryResponse;
        }
    }
}
