using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests;

public class PricesAndDiscountRequest
{
    public decimal? NightlyPrice { get; set; }
    public decimal? WeekendNightlyPrice { get; set; }
    public int CurrencyId { get; set; }
    public List<DiscountPrices>? DiscountPrices { get; set; }
}

public class DiscountPrices
{
    /// <summary>
    /// week - month
    /// </summary>
    public string DiscountCode { get; set; } = string.Empty;

    /// <summary>
    /// id of discount type
    /// </summary>
    public int DiscountId { get; set; }

    /// <summary>
    /// deciamal total amount of discount
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// decimal percentage of discount
    /// </summary>
    public decimal Percentage { get; set; }
}
