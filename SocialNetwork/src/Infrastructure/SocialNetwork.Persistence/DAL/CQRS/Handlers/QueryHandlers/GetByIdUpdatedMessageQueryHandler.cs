using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdUpdatedMessageQueryHandler : IRequestHandler<GetByIdUpdatedMessageQueryRequest, GetByIdUpdatedMessageQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdUpdatedMessageQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdUpdatedMessageQueryResponse> Handle(GetByIdUpdatedMessageQueryRequest request, CancellationToken cancellationToken)
        {
            UpdatedMessage result = _context.UpdatedMessages.FirstOrDefault(c => c.Id == request.Id);
            GetByIdUpdatedMessageQueryResponse getByIdUpdatedMessageQueryResponse = new GetByIdUpdatedMessageQueryResponse()
            {
                Id = result.Id,
                Message = result.Message,
                OldImageURL = result.OldImageURL,
                OldMessageText = result.OldMessageText,
                OldMessageType = result.OldMessageType,
                OldVideoURL = result.OldVideoURL,
                SendTime = result.SendTime
            };

            return getByIdUpdatedMessageQueryResponse;
        }
    }
}
