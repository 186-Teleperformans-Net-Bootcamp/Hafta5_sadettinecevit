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
    public class GetAllUpdatedMessageQueryHandler : IRequestHandler<GetAllUpdatedMessageQueryRequest, GetAllUpdatedMessageQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;

        public GetAllUpdatedMessageQueryHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<GetAllUpdatedMessageQueryResponse> Handle(GetAllUpdatedMessageQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllUpdatedMessageQueryResponse getAllUpdatedMessageQueryResponse = new GetAllUpdatedMessageQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("updatedMessages").Result;

            List<UpdatedMessage> context = null;

            if (cachedBytes == null)
            {
                context = _unitOfWork.UpdatedMessageRepository.GetAsync().Result;

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

            getAllUpdatedMessageQueryResponse.MaxPage = (int)Math.Ceiling(context.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllUpdatedMessageQueryResponse.ListUpdatedMessageQueryResponse = context.Select(u =>
            new UpdatedMessageQueryResponseDTO
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
