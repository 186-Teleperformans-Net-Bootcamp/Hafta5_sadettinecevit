namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetByIdUserQueryResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
