namespace Assert.Domain.Models
{
    public class SearchFilters
    {
        /// <summary>
        /// Fecha de check-in (formato: YYYY-MM-DD).
        /// </summary>
        public DateTime? CheckInDate { get; set; }

        /// <summary>
        /// Fecha de check-out (formato: YYYY-MM-DD).
        /// </summary>
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// Precio mínimo por noche.
        /// </summary>
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Precio máximo por noche.
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Número máximo de huéspedes.
        /// </summary>
        public int? Guests { get; set; }

        /// <summary>
        /// Número mínimo de habitaciones.
        /// </summary>
        public int? Bedrooms { get; set; }

        /// <summary>
        /// Número mínimo de camas.
        /// </summary>
        public int? Beds { get; set; }

        /// <summary>
        /// Id del tipo de propiedad.
        /// </summary>
        public int PropertyTypeId { get; set; }

        /// <summary>
        /// Lista de IDs de comodidades (por ejemplo, 1 para Wi-Fi, 2 para Piscina).
        /// </summary>
        public List<int> AmenityIds { get; set; } = new List<int>();

        /// <summary>
        /// Latitud de referencia para búsqueda por cercanía.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Longitud de referencia para búsqueda por cercanía.
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Radio de búsqueda en metros (solo aplica si se proporcionan latitud y longitud).
        /// </summary>
        public double? Radius { get; set; }
        /// <summary>
        /// Id de la ciudad donde se encuentra la propiedad.
        /// </summary>
        public long? CityId { get; set; }
        /// <summary>
        /// Id de la ciudad donde se encuentra la propiedad.
        /// </summary>
        public long? CountyId { get; set; }
        /// <summary>
        /// Id de la ciudad donde se encuentra la propiedad.
        /// </summary>
        public long? StateId { get; set; }

        /// <summary>
        /// id del país donde se encuentra la propiedad.
        /// </summary>
        public long CountryId { get; set; }
        public int? Bathrooms { get; set; }
        public RulesFilter? Rules { get; set; }
    }
    public class RulesFilter
    {
        public bool? AllowedPets { get; set; }
        public bool? AllowedSmook { get; set; }
        public bool? AllowedEvents { get; set; }
    }
}
