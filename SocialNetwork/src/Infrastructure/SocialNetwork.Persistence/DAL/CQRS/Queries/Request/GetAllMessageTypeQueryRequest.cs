using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using SocialNetwork.Persistence.DAL.Filters;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetAllMessageTypeQueryRequest : MessageTypePaginingRequest, IRequest<PaginingResponse<List<GetAllMessageTypeQueryResponse>>>
    {
    }
}
