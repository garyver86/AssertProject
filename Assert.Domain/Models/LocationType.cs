using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public enum LocationType
    {
        Country,
        State,
        County,
        City
    }

    public class LocationSuggestion
    {
        public string Id { get; set; } // Puede ser una combinación de IDs o un ID único generado
        public string Name { get; set; } // El nombre principal (ej. "Lima", "Miraflores", "Bolivia")
        public string Description { get; set; } // Texto secundario (ej. "Perú", "Vecindario", "Ciudad", "Departamento de Santa Cruz, Bolivia")
        public LocationType Type { get; set; } // Para determinar el icono (país, ciudad, vecindario)
        public string TypeDesc { get; set; } // Para determinar el icono (país, ciudad, vecindario)
        public int EntityId { get; set; } // El ID real de la entidad (CityId, StateId, CountryId, etc.)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
