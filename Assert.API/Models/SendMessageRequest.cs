namespace Assert.API.Models
{
    public class SendMessageRequest
    {
        public object BookId { get; internal set; }
        public object MessageTypeId { get; internal set; }
        public object Body { get; internal set; }
    }
}
