using System.ComponentModel.DataAnnotations.Schema;

namespace Assert.Domain.Entities
{
    public partial class TpProperty
    {
        [NotMapped]
        public double DistanceMeters { get; set; }
    }
}
