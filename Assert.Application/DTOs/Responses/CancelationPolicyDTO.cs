namespace Assert.Application.DTOs.Responses
{
    public class CancelationPolicyDTO
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Detail1 { get; set; }

        public int? RefoundUpToPercentage { get; set; }

        public int? RefoundUpToDays { get; set; }

        public string? Detail2 { get; set; }

        public int? DiscountPercentage { get; set; }

        public int? WithinDays { get; set; }

        public bool? Status { get; set; }
    }
}