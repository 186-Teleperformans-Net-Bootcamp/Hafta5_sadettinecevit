using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllFriendQueryResponse
    {
        public int MaxPage { get; set; }
        public List<FriendQueryResponseDTO> ListFriendQueryResponse { get; set; }
    }
}
