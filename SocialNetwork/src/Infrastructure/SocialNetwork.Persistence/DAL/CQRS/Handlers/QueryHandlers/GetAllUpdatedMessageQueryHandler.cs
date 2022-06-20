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
    public class GetAllUpdatedMessageQueryHandler : IRequestHandler<GetAllUpdatedMessageQueryRequest, PaginingResponse<List<GetAllUpdatedMessageQueryResponse>>>
    {
        private readonly IUpdatedMessageRepository _repo;
        private readonly IDistributedCache _distributedCache;

        public GetAllUpdatedMessageQueryHandler(UpdatedMessageRepository repo, IDistributedCache distributedCache)
        {
            _repo = repo;
            _distributedCache = distributedCache;
        }

        public async Task<PaginingResponse<List<GetAllUpdatedMessageQueryResponse>>> Handle(GetAllUpdatedMessageQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllUpdatedMessageQueryResponse>> getAllUpdatedMessageQueryResponse = new PaginingResponse<List<GetAllUpdatedMessageQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("updatedMessages").Result;

            List<UpdatedMessage> context = null;

            if (cachedBytes == null)
            {
                context = _repo.GetAsync().Result;

                string jsonText = JsonSerializer.Serialize(context);
                _distributedCache.SetAsync("updatedMessages", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                context = JsonSerializer.Deserialize<List<UpdatedMessage>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                context = context.Where(x => x.OldMessageText.Contains(request.Keyword)).ToList();
            }

            if (request.UpdateTime.HasValue)
            {
                context = context.Where(x => x.UpdateTime == request.UpdateTime).ToList();
            }
            if(request.SendTime.HasValue)
            {
                context = context.Where(x => x.SendTime == request.SendTime).ToList();
            }

            getAllUpdatedMessageQueryResponse.Total = context.Count();
            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllUpdatedMessageQueryResponse.Limit = request.Limit;
            getAllUpdatedMessageQueryResponse.Page = request.Page;
            getAllUpdatedMessageQueryResponse.TotalPage = (int)Math.Ceiling(getAllUpdatedMessageQueryResponse.Total / (double)getAllUpdatedMessageQueryResponse.Limit);
            getAllUpdatedMessageQueryResponse.HasPrevious = getAllUpdatedMessageQueryResponse.Page != 1;
            getAllUpdatedMessageQueryResponse.HasNext = getAllUpdatedMessageQueryResponse.Page != getAllUpdatedMessageQueryResponse.TotalPage;

            int skip = (getAllUpdatedMessageQueryResponse.Page - 1) * getAllUpdatedMessageQueryResponse.Limit;
            int take = getAllUpdatedMessageQueryResponse.Limit;

            getAllUpdatedMessageQueryResponse.Response = context.Select(u =>
            new GetAllUpdatedMessageQueryResponse
            {
                Id = u.Id,
                Message = u.Message,
                OldImageURL = u.OldImageURL,
                OldMessageText = u.OldMessageText,
                OldMessageType = u.OldMessageType,
                OldVideoURL = u.OldVideoURL,
                SendTime = u.SendTime
            }).Skip(skip).Take(take).ToList();

            return getAllUpdatedMessageQueryResponse;
        }
    }
}
