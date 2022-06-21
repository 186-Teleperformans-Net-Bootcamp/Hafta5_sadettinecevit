using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdMessageTypeQueryHandler : IRequestHandler<GetByIdMessageTypeQueryRequest, GetByIdMessageTypeQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdMessageTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetByIdMessageTypeQueryResponse> Handle(GetByIdMessageTypeQueryRequest request, CancellationToken cancellationToken)
        {
            MessageType result = _unitOfWork.MessageTypeRepository.GetByIdAsync(request.Id).Result;
            
            GetByIdMessageTypeQueryResponse getByIdMessageTypeQueryResponse = new GetByIdMessageTypeQueryResponse()
            {
                MessageTypeQueryResponse = new()
                {
                    Id = result.Id,
                    Type = result.Type
                }
            };

            return getByIdMessageTypeQueryResponse;
        }
    }
}
