namespace Assert.Application.DTOs.Requests;

public class BasicListingRentData : BasicListingRentDataBase
{   
    public List<int>? AspectTypeIdList { get; set; } = new();
}

public class BasicListingRentDataBase
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
