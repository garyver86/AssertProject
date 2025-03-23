using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TRentPriceSuggestion
{
    public int RentPriceSuggestionId { get; set; }

    public int? YearFrom { get; set; }

    public int? YearTo { get; set; }

    public int? SizeFrom { get; set; }

    public int? SizeTo { get; set; }

    public int? AmentiesCount { get; set; }

    public decimal? RentPrice { get; set; }

    public decimal? SecurityDepositPrice { get; set; }

    public int? CurrencyId { get; set; }

    public string? CurrencyCode { get; set; }
}
