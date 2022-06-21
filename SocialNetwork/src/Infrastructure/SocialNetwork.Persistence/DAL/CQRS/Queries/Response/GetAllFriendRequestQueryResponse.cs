using SocialNetwork.Application.Dto;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllFriendRequestQueryResponse
    {
        public int MaxPage { get; set; }
        public List<FriendRequestQueryResponseDTO> ListFriendRequestQueryResponse { get; set; }
    }
}
