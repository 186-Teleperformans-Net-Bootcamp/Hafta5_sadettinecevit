using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllMessageQueryResponse
    {
        public int MaxPage { get; set; }
        public List<MessageQueryResponseDTO> ListMessageQueryResponse { get; set; }
    }
}
