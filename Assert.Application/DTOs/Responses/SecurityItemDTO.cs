namespace Assert.Application.DTOs.Responses
{
    public class SecurityItemDTO
    {
        public bool? Value { get; set; }
        public string? ValueString { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? IconLink { get; set; }
        public string? Description { get; set; }
        public int SecurityItemTypeId { get; set; }
    }
}