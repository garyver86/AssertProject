using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Entities
{
    public partial class TlListingRent
    {
        [NotMapped]
        public bool isFavorite { get; set; }
        [NotMapped]
        public DateTime? historyDate { get; set; }
    }
}
