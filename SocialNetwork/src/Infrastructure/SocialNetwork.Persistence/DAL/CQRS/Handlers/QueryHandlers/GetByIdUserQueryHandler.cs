using MediatR;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQueryRequest, GetByIdUserQueryResponse>
    {
        private readonly UserManager<User> _userManager;
        
        //Distributed Cache kullanılabilir.
        public GetByIdUserQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetByIdUserQueryResponse> Handle(GetByIdUserQueryRequest request, CancellationToken cancellationToken)
        {
            User result = _userManager.FindByIdAsync(request.Id).Result;
            GetByIdUserQueryResponse getByIdUserQueryResponse = new GetByIdUserQueryResponse()
            {
                Email = result.Email,
                Id = result.Id,
                LastName = result.LastName,
                Name = result.Name,
                UserName = result.UserName
            };

            return getByIdUserQueryResponse;
        }
    }
}
