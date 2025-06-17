using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests
{
    public class BulkBlockCalendarDaysRequest
    {
        /// <summary>
        /// ID del listing rent
        /// </summary>
        [Required]
        public int ListingRentId { get; set; }

        /// <summary>
        /// Fechas a bloquear
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "Debe especificar al menos una fecha")]
        public List<DateOnly> Dates { get; set; } = new();

        /// <summary>
        /// Tipo de bloqueo
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int BlockType { get; set; }

        /// <summary>
        /// Razón del bloqueo
        /// </summary>
        [StringLength(500)]
        public string? BlockReason { get; set; }
        public long? BookId { get; set; }
    }

    public class BulkBlockCalendarDaysResponse
    {
        public int TotalDaysBlocked { get; set; }
        public int NewlyBlockedDays { get; set; }
        public int UpdatedDays { get; set; }
        public List<DateTime>? FailedDates { get; set; }
    }
}
