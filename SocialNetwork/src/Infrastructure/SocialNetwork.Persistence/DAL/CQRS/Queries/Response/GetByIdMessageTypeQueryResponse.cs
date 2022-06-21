using SocialNetwork.Application.Dto;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetByIdMessageTypeQueryResponse
    {
        public MessageTypeQueryResponseDTO MessageTypeQueryResponse { get; set; }
    }
}
