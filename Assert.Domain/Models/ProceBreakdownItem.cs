using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class PriceBreakdownItem
    {
        public string Type { get; set; } // "BASE_PRICE", "DISCOUNT", "CLEANING_FEE", etc.
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; } // Para descuentos
        public DateTime? Date { get; set; } // Para cobros por día específico
    }
}
