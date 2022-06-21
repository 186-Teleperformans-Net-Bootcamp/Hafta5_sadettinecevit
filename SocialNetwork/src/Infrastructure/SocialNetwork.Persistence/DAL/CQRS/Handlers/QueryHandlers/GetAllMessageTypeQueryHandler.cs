using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SocialNetwork.Persistence.Repository;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Application.Dto;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllMessageTypeQueryHandler : IRequestHandler<GetAllMessageTypeQueryRequest, GetAllMessageTypeQueryResponse>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMessageTypeQueryHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<GetAllMessageTypeQueryResponse> Handle(GetAllMessageTypeQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllMessageTypeQueryResponse getAllMessageTypeQueryResponse = new GetAllMessageTypeQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("messageTypes").Result;

            List<MessageType> context = null;

            if (cachedBytes == null)
            {
                context = await _unitOfWork.MessageTypeRepository.GetAsync();

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

            getAllMessageTypeQueryResponse.MaxPage = (int)Math.Ceiling(context.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllMessageTypeQueryResponse.ListMessageTypeQueryResponse = context.Select(m =>
            new MessageTypeQueryResponseDTO
            {
                Id = m.Id,
                Type = m.Type
            }).Skip(skip).Take(take).ToList();

            return getAllMessageTypeQueryResponse;
        }
    }
}
