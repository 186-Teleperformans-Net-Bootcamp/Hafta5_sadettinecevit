using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllGroupMemberQueryHandler : IRequestHandler<GetAllGroupMemberQueryRequest, PaginingResponse<List<GetAllGroupMemberQueryResponse>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public GetAllGroupMemberQueryHandler(IDistributedCache distributedCache, ApplicationDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<PaginingResponse<List<GetAllGroupMemberQueryResponse>>> Handle(GetAllGroupMemberQueryRequest request, CancellationToken cancellationToken)
        {
            PaginingResponse<List<GetAllGroupMemberQueryResponse>> getAllGroupMemberQueryResponse = new PaginingResponse<List<GetAllGroupMemberQueryResponse>>();

            byte[] cachedBytes = _distributedCache.GetAsync("groupMembers").Result;

            List<GroupMember> context = null;

            if (cachedBytes == null)
            {
                context = _context.GroupMembers.ToList();

                string jsonText = JsonSerializer.Serialize(context);
                _distributedCache.SetAsync("groupMembers", Encoding.UTF8.GetBytes(jsonText));
            }
            else
            {
                string jsonText = Encoding.UTF8.GetString(cachedBytes);
                context = JsonSerializer.Deserialize<List<GroupMember>>(jsonText);
            }

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                context = context.Where(x => x.User.Name.Contains(request.Keyword)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.GroupName) || !string.IsNullOrWhiteSpace(request.UserName))
            {
                context = context.Where(x => x.User.UserName == request.UserName || x.Group.Name == request.GroupName).ToList();
            }

            getAllGroupMemberQueryResponse.Total = context.Count();
            //gelen sayfa ve limit değerleri kontrol edilecek.
            getAllGroupMemberQueryResponse.Limit = request.Limit;
            getAllGroupMemberQueryResponse.Page = request.Page;
            getAllGroupMemberQueryResponse.Total = _context.GroupMembers.Count();
            getAllGroupMemberQueryResponse.TotalPage = (int)Math.Ceiling(getAllGroupMemberQueryResponse.Total / (double)getAllGroupMemberQueryResponse.Limit);
            getAllGroupMemberQueryResponse.HasPrevious = getAllGroupMemberQueryResponse.Page != 1;
            getAllGroupMemberQueryResponse.HasNext = getAllGroupMemberQueryResponse.Page != getAllGroupMemberQueryResponse.TotalPage;

            int skip = (getAllGroupMemberQueryResponse.Page - 1) * getAllGroupMemberQueryResponse.Limit;
            int take = getAllGroupMemberQueryResponse.Limit;

            getAllGroupMemberQueryResponse.Response = context.Select(g =>
            new GetAllGroupMemberQueryResponse
            {
                Id = g.Id,
                User = g.User,
                Group = g.Group
            }).Skip(skip).Take(take).ToList();

            return getAllGroupMemberQueryResponse;
        }
    }
}
