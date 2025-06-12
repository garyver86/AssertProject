using Assert.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class CalendarResultPaginatedDTO
    {
        public List<CalendarDayDto> CalendarDays { get; set; }
        public PaginationMetadata Pagination { get; set; }
    }
}
