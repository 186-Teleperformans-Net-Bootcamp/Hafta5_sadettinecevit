using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllUpdatedMessageQueryHandler : IRequestHandler<GetAllUpdatedMessageQueryRequest, List<GetAllUpdatedMessageQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllUpdatedMessageQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllUpdatedMessageQueryResponse>> Handle(GetAllUpdatedMessageQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllUpdatedMessageQueryResponse> getAllUpdatedMessageQueryResponse = new List<GetAllUpdatedMessageQueryResponse>();
            
            getAllUpdatedMessageQueryResponse = _context.UpdatedMessages.Select(u => 
            new GetAllUpdatedMessageQueryResponse
            {
                Id=u.Id,
                Message = u.Message,
                OldImageURL = u.OldImageURL,
                OldMessageText = u.OldMessageText,
                OldMessageType = u.OldMessageType,
                OldVideoURL = u.OldVideoURL,
                SendTime = u.SendTime
            }).ToList();

            return getAllUpdatedMessageQueryResponse;
        }
    }
}
