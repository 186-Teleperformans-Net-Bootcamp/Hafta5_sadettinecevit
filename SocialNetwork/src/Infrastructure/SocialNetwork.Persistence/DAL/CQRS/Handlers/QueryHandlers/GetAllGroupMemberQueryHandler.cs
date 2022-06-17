using MediatR;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllGroupMemberQueryHandler : IRequestHandler<GetAllGroupMemberQueryRequest, List<GetAllGroupMemberQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllGroupMemberQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllGroupMemberQueryResponse>> Handle(GetAllGroupMemberQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllGroupMemberQueryResponse> getAllGroupMemberQueryResponse = new List<GetAllGroupMemberQueryResponse>();

            getAllGroupMemberQueryResponse = _context.GroupMembers.Select(g => 
            new GetAllGroupMemberQueryResponse
            {
                Id=g.Id,
                User = g.User,
                Group = g.Group
            }).ToList();

            return getAllGroupMemberQueryResponse;
        }
    }
}
