namespace SocialNetwork.Infrastructure.Services.MessageQueue
{
    public class MessageQueueType
    {
        public string EventDescription { get; set; } = string.Empty;
        public DateTime EventTime { get; set; } = DateTime.Now;
    }
}
