using MediatR;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQueryRequest, List<GetAllUserQueryResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public GetAllUserQueryHandler(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<GetAllUserQueryResponse>> Handle(GetAllUserQueryRequest request, CancellationToken cancellationToken)
        {
            List<GetAllUserQueryResponse> getAllUserQueryResponse = new List<GetAllUserQueryResponse>();

            getAllUserQueryResponse = _userManager.Users.Select(u => 
            new GetAllUserQueryResponse
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Name = u.Name,
                LastName = u.LastName
            }).ToList();

            return getAllUserQueryResponse;
        }
    }
}
