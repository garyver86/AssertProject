namespace Assert.API
{
    public class JWTConfiguration
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int TimeLife { get; set; }
    }
}
