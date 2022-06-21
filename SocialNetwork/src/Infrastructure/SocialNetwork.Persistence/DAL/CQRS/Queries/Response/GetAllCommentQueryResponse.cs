using SocialNetwork.Application.Dto;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllCommentQueryResponse
    {
        public int MaxPage { get; set; }
        public List<CommentQueryResponseDTO> ListCommentQueryResponse { get; set; }
    }
}
