using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses;

public class BookDTO
{
    public long BookId { get; set; }

    public long ListingRentId { get; set; }

    public int UserIdRenter { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal AmountTotal { get; set; }

    public int CurrencyId { get; set; }

    public decimal? MountPerNight { get; set; }

    public decimal? AmountFees { get; set; }

    public string NameRenter { get; set; } = null!;

    public string LastNameRenter { get; set; } = null!;

    public bool? TermsAccepted { get; set; }

    public string? AdditionalInfo { get; set; }

    public int BookStatusId { get; set; }

    public bool? IsApprobal { get; set; }

    public string? ApprovalDetails { get; set; }

    public bool? IsManualApprobal { get; set; }

    public int? DaysToApproval { get; set; }

    public DateTime? InitDate { get; set; }

    public string? PaymentCode { get; set; }

    public string? Pk { get; set; }

    public string? PaymentId { get; set; }

    public decimal? DepositSec { get; set; }

    public string? PickUpLocation { get; set; }

    public decimal? VggFee { get; set; }

    public decimal? VggFeePercent { get; set; }
    public List<CalendarDayDto>? CalendarDays { get; set; }
}
