namespace LoggerService.Model
{
    public class MessageQueueType
    {
        public string EventDescription { get; set; } = string.Empty;
        public DateTime EventTime { get; set; } = DateTime.Now;
    }
}
