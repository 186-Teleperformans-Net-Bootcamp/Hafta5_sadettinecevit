using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Dto;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllGroupMemberQueryHandler : IRequestHandler<GetAllGroupMemberQueryRequest, GetAllGroupMemberQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        public GetAllGroupMemberQueryHandler(IDistributedCache distributedCache, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllGroupMemberQueryResponse> Handle(GetAllGroupMemberQueryRequest request, CancellationToken cancellationToken)
        {
            GetAllGroupMemberQueryResponse getAllGroupMemberQueryResponse = new GetAllGroupMemberQueryResponse();

            byte[] cachedBytes = _distributedCache.GetAsync("groupMembers").Result;

            List<GroupMember> context = null;

            if (cachedBytes == null)
            {
                context = _unitOfWork.GroupMemberRepository.GetAsync().Result;

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

            getAllGroupMemberQueryResponse.MaxPage = (int)Math.Ceiling(context.Count() / (double)request.Limit);

            int skip = (request.Page - 1) * request.Limit;
            int take = request.Limit;

            getAllGroupMemberQueryResponse.ListGroupMemberQueryResponse = context.Select(g =>
            new GroupMemberQueryResponseDTO
            {
                Id = g.Id,
                User = g.User,
                Group = g.Group
            }).Skip(skip).Take(take).ToList();

            return getAllGroupMemberQueryResponse;
        }
    }
}
