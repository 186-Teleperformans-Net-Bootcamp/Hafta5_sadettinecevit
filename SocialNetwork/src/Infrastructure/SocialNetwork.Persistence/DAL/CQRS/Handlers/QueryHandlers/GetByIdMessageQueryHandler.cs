using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdMessageQueryHandler : IRequestHandler<GetByIdMessageQueryRequest, GetByIdMessageQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdMessageQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdMessageQueryResponse> Handle(GetByIdMessageQueryRequest request, CancellationToken cancellationToken)
        {
            Message result = _context.Messages.FirstOrDefault(c => c.Id == request.Id);
            GetByIdMessageQueryResponse getByIdMessageQueryResponse = new GetByIdMessageQueryResponse()
            {
                Id = result.Id,
                FromUser = result.FromUser,
                ImageURL = result.ImageURL,
                MessageText = result.MessageText,
                ToUsers = result.ToUsers,
                Type = result.Type,
                VideoURL = result.VideoURL
            };

            return getByIdMessageQueryResponse;
        }
    }
}
