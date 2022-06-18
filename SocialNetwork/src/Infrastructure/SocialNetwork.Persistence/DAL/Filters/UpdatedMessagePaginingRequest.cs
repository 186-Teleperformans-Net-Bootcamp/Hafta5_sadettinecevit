using SocialNetwork.Persistence.DAL.CQRS.Queries;

namespace SocialNetwork.Persistence.DAL.Filters
{
    public class UpdatedMessagePaginingRequest : PaginingRequest
    {
        //arttırılabilir.
        public DateTime? SendTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
