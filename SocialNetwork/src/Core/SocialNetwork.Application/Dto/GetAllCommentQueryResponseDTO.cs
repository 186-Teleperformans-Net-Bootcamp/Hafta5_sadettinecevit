using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Application.Dto
{
    public class GetAllCommentQueryResponseDTO
    {
        public string Id { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public bool IsPrivate { get; set; }
        public string CommentText { get; set; }
    }

}
