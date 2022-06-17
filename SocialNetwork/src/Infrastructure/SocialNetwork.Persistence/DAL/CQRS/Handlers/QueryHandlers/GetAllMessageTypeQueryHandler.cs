using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllMessageTypeQueryHandler : IRequestHandler<GetAllMessageTypeQueryRequest, List<GetAllMessageTypeQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllMessageTypeQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllMessageTypeQueryResponse>> Handle(GetAllMessageTypeQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllMessageTypeQueryResponse> getAllMessageTypeQueryResponse = new List<GetAllMessageTypeQueryResponse>();
            
            getAllMessageTypeQueryResponse = _context.MessageTypes.Select(m => 
            new GetAllMessageTypeQueryResponse
            {
                Id = m.Id,
                Type = m.Type
            }).ToList();

            return getAllMessageTypeQueryResponse;
        }
    }
}
