using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ListingRentCalendarDTO
    {
        public long ListingRentId { get; set; }

        public int OwnerUserId { get; set; }

        public int? ListingStatusId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? StepsCount { get; set; }

        public decimal? MinimunRentalPerDay { get; set; }

        public int? CancelationPolicyTypeId { get; set; }

        public int? ApprovalPolicyTypeId { get; set; }

        public int? ApprovalRequestDays { get; set; }

        public int? AccomodationTypeId { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        public int? MaxGuests { get; set; }

        public bool? AllDoorsLocked { get; set; }

        public int? Beds { get; set; }

        public DateTime? ListingRentConfirmationDate { get; set; }

        public bool? ExternalCameras { get; set; }

        public bool? PresenceOfWeapons { get; set; }

        public bool? NoiseDesibelesMonitor { get; set; }

        public int? MinimunStay { get; set; }

        public int? MaximumStay { get; set; }

        public int? MinimumNotice { get; set; }

        public TimeOnly? MinimumNoticeHour { get; set; }

        public int? PreparationDays { get; set; }

        public int? AvailabilityWindowMonth { get; set; }

        public string? CheckInDays { get; set; }

        public string? CheckOutDays { get; set; }

        public int? PrivateBathroom { get; set; }

        public int? PrivateBathroomLodging { get; set; }

        public int? SharedBathroom { get; set; }

        public decimal? AvgReviews { get; set; }
        public PriceDTO? Price { get; set; }
        public List<BookDTO>? Reservations { get; set; }
        public List<CalendarDayDto>? CalendarDays { get; set; }
        public PropertyDTO? Property { get; set; }
        public List<PhotoDTO> Photos { get; set; }
    }
}
