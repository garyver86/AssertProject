using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class CalendarDayDto
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int? BlockType { get; set; }
        public string BlockReason { get; set; }
        public int? BookId { get; set; }
        public int MinimumStay { get; set; }
        public int MaximumStay { get; set; }
        public IEnumerable<CalendarDiscountDto> Discounts { get; set; }
        public string BlockTypeName { get; set; }
    }

    public class CalendarDiscountDto
    {
        public bool IsDiscount { get; set; }
        public decimal DiscountCalculated { get; set; }
        public decimal Porcentage { get; set; }
    }
}
