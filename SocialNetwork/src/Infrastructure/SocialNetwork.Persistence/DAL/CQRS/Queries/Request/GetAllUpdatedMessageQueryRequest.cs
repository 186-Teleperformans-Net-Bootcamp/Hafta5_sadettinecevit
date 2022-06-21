using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetAllUpdatedMessageQueryRequest : IRequest<GetAllUpdatedMessageQueryResponse>
    {
        public int Limit { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string Keyword { get; set; } = string.Empty;
        //arttırılabilir.
        public DateTime? SendTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
