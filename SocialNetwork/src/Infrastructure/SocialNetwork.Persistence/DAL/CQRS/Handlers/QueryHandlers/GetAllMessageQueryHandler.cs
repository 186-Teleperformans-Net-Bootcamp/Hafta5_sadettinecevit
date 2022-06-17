using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllMessageQueryHandler : IRequestHandler<GetAllMessageQueryRequest, List<GetAllMessageQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllMessageQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllMessageQueryResponse>> Handle(GetAllMessageQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllMessageQueryResponse> getAllMessageQueryResponse = new List<GetAllMessageQueryResponse>();

            getAllMessageQueryResponse = _context.Messages.Select(m => 
            new GetAllMessageQueryResponse
            {
                Id = m.Id,
                FromUser = m.FromUser,
                ImageURL = m.ImageURL,
                MessageText = m.MessageText,
                ToUsers = m.ToUsers,
                Type = m.Type,
                VideoURL = m.VideoURL
            }).ToList();

            return getAllMessageQueryResponse;
        }
    }
}
