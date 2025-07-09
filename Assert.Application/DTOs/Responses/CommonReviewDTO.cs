using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses;

public class CommonReviewDTO
{
    public int ReviewId { get; set; }
    public int ListingRentId { get; set; }
    public int BookId { get; set; }
    public int UserIdReviewer { get; set; }
    public DateTime DateTimeReview { get; set; }

    public string ReviewerName { get; set; } = string.Empty;
    public string ReviewerLocation { get; set; } = string.Empty;
    public string ReviewDateName { get; set; } = string.Empty;
    public int StayDuration { get; set; }
    public double Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
}
