using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllGroupQueryResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
