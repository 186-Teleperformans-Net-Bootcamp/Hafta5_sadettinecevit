using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetByIdFriendQueryResponse
    {
        public FriendQueryResponseDTO FriendQueryResponse { get; set; }
    }
}
