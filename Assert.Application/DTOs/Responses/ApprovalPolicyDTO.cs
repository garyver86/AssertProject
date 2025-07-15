namespace Assert.Application.DTOs.Responses
{
    public class ApprovalPolicyDTO
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsRecommended { get; set; }

        public bool? Status { get; set; }
        public int ApprovalPolicyTypeId { get; set; }
    }
}
