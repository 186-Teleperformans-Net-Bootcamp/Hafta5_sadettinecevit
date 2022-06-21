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
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQueryRequest, GetAllGroupQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;

        public GetAllGroupQueryHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllGroupQueryResponse> Handle(GetAllGroupQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllGroupQueryResponse getAllGroupQueryResponse = new GetAllGroupQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("groups").Result;

            List<Group> groups = null;

            if (cachedBytes == null)
            {
                groups = _unitOfWork.GroupRepository.GetAsync().Result;

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

            getAllGroupQueryResponse.MaxPage = (int)Math.Ceiling(groups.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllGroupQueryResponse.ListGroupQueryResponse = groups.Select(g =>
            new GroupQueryResponseDTO
            {
                Id = g.Id,
                Name = g.Name
            }).Skip(skip).Take(take).ToList();

            return getAllGroupQueryResponse;
        }
    }
}
