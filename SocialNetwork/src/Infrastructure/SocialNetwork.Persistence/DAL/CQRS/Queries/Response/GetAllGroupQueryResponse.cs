using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllGroupQueryResponse
    {
        public int MaxPage { get; set; }
        public List<GroupQueryResponseDTO> ListGroupQueryResponse { get; set; }
    }
}
