namespace Assert.Application.DTOs.Responses;

public class AddionalFeeTypeDTO
{
    public int AdditionalFeeId { get; set; }

    public string FeeCode { get; set; } = null!;

    public string DeeDescription { get; set; } = null!;

    //public string CalculationType { get; set; } = null!;

    //public decimal FeeValue { get; set; }
}
