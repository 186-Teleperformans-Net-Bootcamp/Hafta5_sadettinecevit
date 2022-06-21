using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdFriendQueryHandler : IRequestHandler<GetByIdFriendQueryRequest, GetByIdFriendQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdFriendQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetByIdFriendQueryResponse> Handle(GetByIdFriendQueryRequest request, CancellationToken cancellationToken)
        {
            Friend result = _unitOfWork.FriendRepository.GetByIdAsync(request.Id).Result;
            GetByIdFriendQueryResponse getByIdFriendQueryResponse = new GetByIdFriendQueryResponse()
            {
                FriendQueryResponse = new()
                {
                    Id = request.Id,
                    FriendUser = result.FriendUser,
                    User = result.User
                }
            };

            return getByIdFriendQueryResponse;
        }
    }
}
