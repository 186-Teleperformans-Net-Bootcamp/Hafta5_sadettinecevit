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
    //Distributed Cache kullanılabilir. 
    //Hatta daha faydalı olacaktır.
    public class GetByIdUpdatedMessageQueryHandler : IRequestHandler<GetByIdUpdatedMessageQueryRequest, GetByIdUpdatedMessageQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdUpdatedMessageQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetByIdUpdatedMessageQueryResponse> Handle(GetByIdUpdatedMessageQueryRequest request, CancellationToken cancellationToken)
        {
            UpdatedMessage result = _unitOfWork.UpdatedMessageRepository.GetByIdAsync(request.Id).Result;
            GetByIdUpdatedMessageQueryResponse getByIdUpdatedMessageQueryResponse = new GetByIdUpdatedMessageQueryResponse()
            {
                UpdatedMessageQueryResponse = new()
                {
                    Id = result.Id,
                    Message = result.Message,
                    OldImageURL = result.OldImageURL,
                    OldMessageText = result.OldMessageText,
                    OldMessageType = result.OldMessageType,
                    OldVideoURL = result.OldVideoURL,
                    SendTime = result.SendTime
                }
            };

            return getByIdUpdatedMessageQueryResponse;
        }
    }
}
