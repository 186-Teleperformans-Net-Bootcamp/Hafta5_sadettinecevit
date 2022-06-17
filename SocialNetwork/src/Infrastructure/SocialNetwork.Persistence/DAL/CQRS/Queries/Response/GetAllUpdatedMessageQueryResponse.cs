using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllUpdatedMessageQueryResponse
    {
        public string Id { get; set; }
        public Message Message { get; set; }
        public MessageType OldMessageType { get; set; }
        public DateTime SendTime { get; set; }
        public string OldMessageText { get; set; }
        public string OldImageURL { get; set; }
        public string OldVideoURL { get; set; }
    }
}
