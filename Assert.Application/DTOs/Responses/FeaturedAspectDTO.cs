namespace Assert.Application.DTOs.Responses
{
    public class FeaturedAspectDTO
    {
        public string? Value { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? FeaturedAspectTypeId { get; set; }
    }
}