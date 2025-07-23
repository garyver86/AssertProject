namespace Assert.Application.DTOs.Responses
{
    public class CheckInOutPolicyDTO
    {
        public TimeOnly? CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }
        public TimeOnly? MaxCheckInTime { get; set; }
        public bool? FlexibleCheckIn { get; set; }
        public bool? FlexibleCheckOut { get; set; }
        public decimal? LateCheckInFee { get; set; }
        public decimal? LateCheckOutFee { get; set; }
        public string? Instructions { get; set; }
        public string? CheckInTime_Str { get; set; }
        public string? CheckOutTime_Str { get; set; }
        public string? MaxCheckInTime_Str { get; set; }
    }
}