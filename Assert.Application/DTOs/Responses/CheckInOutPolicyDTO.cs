namespace Assert.Application.DTOs.Responses
{
    public class CheckInOutPolicyDTO
    {
        public TimeOnly? CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }
        public bool? FlexibleCheckIn { get; set; }
        public bool? FlexibleCheckOut { get; set; }
        public decimal? LateCheckInFee { get; set; }
        public decimal? LateCheckOutFee { get; set; }
        public string? Instructions { get; set; }
    }
}