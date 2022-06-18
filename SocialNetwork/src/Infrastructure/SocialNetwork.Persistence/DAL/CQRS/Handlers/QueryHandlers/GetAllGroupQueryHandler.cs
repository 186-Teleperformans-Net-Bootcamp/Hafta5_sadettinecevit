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
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQueryRequest, PaginingResponse<List<GetAllGroupQueryResponse>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public GetAllGroupQueryHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<PaginingResponse<List<GetAllGroupQueryResponse>>> Handle(GetAllGroupQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllGroupQueryResponse>> getAllGroupQueryResponse = new PaginingResponse<List<GetAllGroupQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("groups").Result;

            List<Group> context = null;

            if (cachedBytes == null)
            {
                context = _context.Groups.ToList();

                string jsonText = JsonSerializer.Serialize(context);
                _distributedCache.SetAsync("groups", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                context = JsonSerializer.Deserialize<List<Group>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                context = context.Where(x => x.Name.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                context = context.Where(x => x.Name == request.Name).ToList();
            }

            getAllGroupQueryResponse.Total = context.Count();
            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllGroupQueryResponse.Limit = request.Limit;
            getAllGroupQueryResponse.Page = request.Page;
            getAllGroupQueryResponse.Total = _context.Groups.Count();
            getAllGroupQueryResponse.TotalPage = (int)Math.Ceiling(getAllGroupQueryResponse.Total / (double)getAllGroupQueryResponse.Limit);
            getAllGroupQueryResponse.HasPrevious = getAllGroupQueryResponse.Page != 1;
            getAllGroupQueryResponse.HasNext = getAllGroupQueryResponse.Page != getAllGroupQueryResponse.TotalPage;

            int skip = (getAllGroupQueryResponse.Page - 1) * getAllGroupQueryResponse.Limit;
            int take = getAllGroupQueryResponse.Limit;

            getAllGroupQueryResponse.Response = context.Select(g =>
            new GetAllGroupQueryResponse
            {
                Id = g.Id,
                Name = g.Name
            }).Skip(skip).Take(take).ToList();

            return getAllGroupQueryResponse;
        }
    }
}
