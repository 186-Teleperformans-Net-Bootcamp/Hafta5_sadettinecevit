using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllUpdatedMessageQueryResponse
    {
        public int MaxPage { get; set; }
        public List<UpdatedMessageQueryResponseDTO> ListUpdatedMessageQueryResponse { get; set; }
    }
}
