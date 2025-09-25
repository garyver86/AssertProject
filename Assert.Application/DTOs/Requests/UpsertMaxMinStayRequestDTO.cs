namespace Assert.Application.DTOs.Requests;

public class UpsertMaxMinStayRequestDTO
{
    public long ListingRentId { get; set; }
    public bool SetMinStay { get; set; }
    public int MinStay { get; set; }
    public bool SetMaxStay { get; set; }
    public int MaxStay { get; set; }
}
