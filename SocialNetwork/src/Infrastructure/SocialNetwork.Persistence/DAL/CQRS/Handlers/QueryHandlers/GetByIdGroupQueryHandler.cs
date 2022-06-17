using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdGroupQueryHandler : IRequestHandler<GetByIdGroupQueryRequest, GetByIdGroupQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdGroupQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdGroupQueryResponse> Handle(GetByIdGroupQueryRequest request, CancellationToken cancellationToken)
        {
            Group result = _context.Groups.FirstOrDefault(g => g.Id == request.Id);
            GetByIdGroupQueryResponse getByIdGroupQueryResponse = new GetByIdGroupQueryResponse()
            {
                Id = result.Id,
                Name = result.Name
            };

            return getByIdGroupQueryResponse;
        }
    }
}
