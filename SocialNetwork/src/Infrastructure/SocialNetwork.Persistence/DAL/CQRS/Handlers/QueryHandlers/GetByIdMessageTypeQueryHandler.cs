using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdMessageTypeQueryHandler : IRequestHandler<GetByIdMessageTypeQueryRequest, GetByIdMessageTypeQueryResponse>
    {
        private readonly IMessageTypeRepository _repo;

        public GetByIdMessageTypeQueryHandler(MessageTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetByIdMessageTypeQueryResponse> Handle(GetByIdMessageTypeQueryRequest request, CancellationToken cancellationToken)
        {
            MessageType result = _repo.GetByIdAsync(request.Id).Result;
            
            GetByIdMessageTypeQueryResponse getByIdMessageTypeQueryResponse = new GetByIdMessageTypeQueryResponse()
            {
                Id = result.Id,
                Type = result.Type
            };

            return getByIdMessageTypeQueryResponse;
        }
    }
}
