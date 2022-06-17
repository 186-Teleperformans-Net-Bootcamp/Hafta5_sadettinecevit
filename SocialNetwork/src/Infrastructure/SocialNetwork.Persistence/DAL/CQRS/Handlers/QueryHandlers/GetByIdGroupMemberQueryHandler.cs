using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdGroupMemberQueryHandler : IRequestHandler<GetByIdGroupMemberQueryRequest, GetByIdGroupMemberQueryResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetByIdGroupMemberQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetByIdGroupMemberQueryResponse> Handle(GetByIdGroupMemberQueryRequest request, CancellationToken cancellationToken)
        {
            GroupMember result = _context.GroupMembers.FirstOrDefault(c => c.Id == request.Id);
            GetByIdGroupMemberQueryResponse getByIdGroupMemberQueryResponse = new GetByIdGroupMemberQueryResponse()
            {
                Id = result.Id,
                User = result.User,
                Group = result.Group
            };

            return getByIdGroupMemberQueryResponse;
        }
    }
}
