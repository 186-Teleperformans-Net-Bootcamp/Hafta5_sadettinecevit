using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetAllUserQueryRequest : IRequest<List<GetAllUserQueryResponse>>
    {
    }
}
