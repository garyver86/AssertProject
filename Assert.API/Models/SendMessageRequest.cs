namespace Assert.API.Models
{
    public class SendMessageRequest
    {
        public int BookId { get; set; }
        public int MessageTypeId { get; set; }
        public string Body { get; set; }
    }
}
