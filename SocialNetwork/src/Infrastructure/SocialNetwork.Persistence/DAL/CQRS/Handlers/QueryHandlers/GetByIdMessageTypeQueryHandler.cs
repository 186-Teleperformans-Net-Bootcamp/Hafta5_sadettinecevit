using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdMessageTypeQueryHandler : IRequestHandler<GetByIdMessageTypeQueryRequest, GetByIdMessageTypeQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdMessageTypeQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdMessageTypeQueryResponse> Handle(GetByIdMessageTypeQueryRequest request, CancellationToken cancellationToken)
        {
            MessageType result = _context.MessageTypes.FirstOrDefault(c => c.Id == request.Id);
            GetByIdMessageTypeQueryResponse getByIdMessageTypeQueryResponse = new GetByIdMessageTypeQueryResponse()
            {

            };

            return getByIdMessageTypeQueryResponse;
        }
    }
}
