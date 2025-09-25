namespace Assert.Application.DTOs.Requests;

public class UpsertMinimumNoticeRequestDTO
{
    public long ListingRentId { get; set; }
    public int MinimumNoticeDay { get; set; }
    public TimeSpan? MinimumNoticeHours { get; set; }
}
