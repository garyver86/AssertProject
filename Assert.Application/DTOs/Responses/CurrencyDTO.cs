namespace Assert.Application.DTOs.Responses;

public class CurrencyDTO
{
    public int CurrencyId { get; set; }

    public string? Code { get; set; }

    public string? Symbol { get; set; }

    public string? Name { get; set; }

    public string? CountryCode { get; set; }
}
