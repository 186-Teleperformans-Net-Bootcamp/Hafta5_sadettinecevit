namespace SocialNetwork.Persistence.DAL.CQRS.Queries
{
    public abstract class PaginingRequest
    {
        public int Limit { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string Keyword { get; set; } = string.Empty;
    }
}
