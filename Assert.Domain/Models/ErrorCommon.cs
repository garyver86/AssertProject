namespace Assert.Domain.Models
{
    public class ErrorCommon
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public ErrorCommon(string message)
        {
            message = message;
        }
        public ErrorCommon()
        {

        }
    }
}
