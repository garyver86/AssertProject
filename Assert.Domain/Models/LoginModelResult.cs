namespace Assert.Domain.Models
{
    public class LoginModelResult
    {
        public int CodeResult { get; set; }
        public string MessageResult { get; set; }
        public int Identifier { get; set; }
        public string Roles { get; set; }
    }
}
