namespace Assert.Application.DTOs.Responses
{
    public class ComplaintReasonDTO
    {
        public int ComplaintReasonId { get; set; }

        public string ComplaintReasonCode { get; set; } = null!;

        public string ReasonDescription { get; set; } = null!;

        public bool? RequiresFreeText { get; set; }

        public int? SeverityLevel { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
