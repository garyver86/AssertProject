using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses;

public class BookCancellationDTO
{
    public long BookCacellationId { get; set; }

    public long? BookId { get; set; }

    public long? ListingRentId { get; set; }

    public int? UserId { get; set; }

    public string? CancellationTypeCode { get; set; }

    public int? CancellationReasonId { get; set; }

    public string? MessageToGuest { get; set; }

    public string? MessageToHost { get; set; }

    public string? MessageToAssert { get; set; }

    public DateTime? CreatedAt { get; set; }

    public List<BookCancellationReasonDTO>? CancellationFlow { get; set; }
}
