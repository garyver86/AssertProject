using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCurrency
{
    public int CurrencyId { get; set; }

    public string? Code { get; set; }

    public string? Symbol { get; set; }

    public string? Name { get; set; }

    public string? CountryCode { get; set; }

    public virtual ICollection<TSpecialDatePrice> TSpecialDatePrices { get; set; } = new List<TSpecialDatePrice>();

    public virtual ICollection<TbBook> TbBooks { get; set; } = new List<TbBook>();

    public virtual ICollection<TlListingPrice> TlListingPrices { get; set; } = new List<TlListingPrice>();

    public virtual ICollection<TlListingSpecialDatePrice> TlListingSpecialDatePrices { get; set; } = new List<TlListingSpecialDatePrice>();

    public virtual ICollection<TpEstimatedRentalIncome> TpEstimatedRentalIncomes { get; set; } = new List<TpEstimatedRentalIncome>();

    public virtual ICollection<TuUserListingRentPayment> TuUserListingRentPayments { get; set; } = new List<TuUserListingRentPayment>();
}
