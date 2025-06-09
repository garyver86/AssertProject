using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpProperty
{
    public long PropertyId { get; set; }

    public long? ListingRentId { get; set; }

    public int? PropertySubtypeId { get; set; }

    public string? ExternalReference { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? CityId { get; set; }

    public int? CountyId { get; set; }

    public int? StateId { get; set; }

    public int? CountryId { get; set; }

    public string? CityName { get; set; }

    public string? CountyName { get; set; }

    public string? StateName { get; set; }

    public string? CountryName { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? ZipCode { get; set; }

    public virtual TCity? City { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual TpPropertySubtype? PropertySubtype { get; set; }

    public virtual TState? State { get; set; }

    public virtual ICollection<TpPropertyAddress> TpPropertyAddresses { get; set; } = new List<TpPropertyAddress>();
}
