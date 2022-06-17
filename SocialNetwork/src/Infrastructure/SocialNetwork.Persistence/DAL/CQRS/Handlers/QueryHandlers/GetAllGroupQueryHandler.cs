using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQueryRequest, List<GetAllGroupQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllGroupQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllGroupQueryResponse>> Handle(GetAllGroupQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllGroupQueryResponse> getAllGroupQueryResponse = new List<GetAllGroupQueryResponse>();

            getAllGroupQueryResponse = _context.Groups.Select(g => 
            new GetAllGroupQueryResponse
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();

            return getAllGroupQueryResponse;
        }
    }
}
