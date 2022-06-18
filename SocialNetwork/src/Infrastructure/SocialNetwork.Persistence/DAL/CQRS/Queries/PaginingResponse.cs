namespace SocialNetwork.Persistence.DAL.CQRS.Queries
{
    public class PaginingResponse<T> : PaginingRequest where T : class, new()
    {
        public int Total { get; set; }
        public int TotalPage { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public T Response { get; set; }
    }
}
