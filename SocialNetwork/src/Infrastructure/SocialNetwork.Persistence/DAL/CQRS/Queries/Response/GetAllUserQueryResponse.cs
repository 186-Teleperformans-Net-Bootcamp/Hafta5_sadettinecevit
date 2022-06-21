using SocialNetwork.Application.Dto;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllUserQueryResponse
    {
        public int MaxPage { get; set; }
        public List<UserQueryResponseDTO> ListUserQueryResponse { get; set; }

    }
}
