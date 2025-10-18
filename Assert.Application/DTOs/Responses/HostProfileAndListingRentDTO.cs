using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses;

public class HostProfileAndListingRentDTO
{
    public int HostId { get; set; }
    public string HostName { get; set; } = string.Empty;
    public string HostLastName { get; set; } = string.Empty;
    public string HostPhotoUrl { get; set; } = string.Empty;
    public string HostAboutMe { get; set; } = string.Empty;
    public string HostOccupation { get; set; } = string.Empty;

    public long ListingRentId { get; set; }
    public string ListingPhotoUrl { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
    public decimal AverageRating { get; set; }
}
