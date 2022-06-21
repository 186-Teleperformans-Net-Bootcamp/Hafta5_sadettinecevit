using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllGroupMemberQueryResponse
    {
        public int MaxPage { get; set; }
        public List<GroupMemberQueryResponseDTO> ListGroupMemberQueryResponse { get; set; }
    }
}
