using SocialNetwork.Application.Dto;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllMessageTypeQueryResponse
    {
        public int MaxPage { get; set; }
        public List<MessageTypeQueryResponseDTO> ListMessageTypeQueryResponse { get; set; }
    }
}
