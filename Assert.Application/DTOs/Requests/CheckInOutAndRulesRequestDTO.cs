namespace Assert.Application.DTOs.Requests;

public class CheckInOutAndRulesRequestDTO
{
    public string CheckInTime { get; set; } = string.Empty;
    public string MaxCheckInTime { get; set; } = string.Empty;
    public string CheckOutTime { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public List<int> RuleIds { get; set; } = new();

}
