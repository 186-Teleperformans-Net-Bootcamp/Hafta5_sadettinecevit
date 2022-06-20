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
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQueryRequest, PaginingResponse<List<GetAllGroupQueryResponse>>>
    {
        private readonly IGroupRepository _repo;
        private readonly IDistributedCache _distributedCache;

        public GetAllGroupQueryHandler(IDistributedCache distributedCache, GroupRepository repo)
        {
            _distributedCache = distributedCache;
            _repo = repo;
        }

        public async Task<PaginingResponse<List<GetAllGroupQueryResponse>>> Handle(GetAllGroupQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllGroupQueryResponse>> getAllGroupQueryResponse = new PaginingResponse<List<GetAllGroupQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("groups").Result;

            List<Group> groups = null;

            if (cachedBytes == null)
            {
                groups = _repo.GetAsync().Result;

                string jsonText = JsonSerializer.Serialize(groups);
                _distributedCache.SetAsync("groups", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                groups = JsonSerializer.Deserialize<List<Group>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                groups = groups.Where(x => x.Name.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                groups = groups.Where(x => x.Name == request.Name).ToList();
            }

            getAllGroupQueryResponse.Total = groups.Count();
            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllGroupQueryResponse.Limit = request.Limit;
            getAllGroupQueryResponse.Page = request.Page;
            getAllGroupQueryResponse.TotalPage = (int)Math.Ceiling(getAllGroupQueryResponse.Total / (double)getAllGroupQueryResponse.Limit);
            getAllGroupQueryResponse.HasPrevious = getAllGroupQueryResponse.Page != 1;
            getAllGroupQueryResponse.HasNext = getAllGroupQueryResponse.Page != getAllGroupQueryResponse.TotalPage;

            int skip = (getAllGroupQueryResponse.Page - 1) * getAllGroupQueryResponse.Limit;
            int take = getAllGroupQueryResponse.Limit;

            getAllGroupQueryResponse.Response = groups.Select(g =>
            new GetAllGroupQueryResponse
            {
                Id = g.Id,
                Name = g.Name
            }).Skip(skip).Take(take).ToList();

            return getAllGroupQueryResponse;
        }
    }
}
