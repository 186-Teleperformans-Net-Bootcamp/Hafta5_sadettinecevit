using MediatR;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.Repository;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.QueryHandlers
{
    public class GetByIdGroupQueryHandler : IRequestHandler<GetByIdGroupQueryRequest, GetByIdGroupQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdGroupQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetByIdGroupQueryResponse> Handle(GetByIdGroupQueryRequest request, CancellationToken cancellationToken)
        {
            Group result = _unitOfWork.GroupRepository.GetByIdAsync(request.Id).Result;
            GetByIdGroupQueryResponse getByIdGroupQueryResponse = new GetByIdGroupQueryResponse()
            {
                GroupQueryResponse = new()
                {
                    Id = result.Id,
                    Name = result.Name
                }
            };

            return getByIdGroupQueryResponse;
        }
    }
}
