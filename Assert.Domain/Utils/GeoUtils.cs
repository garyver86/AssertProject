using System.Globalization;
using System.Text;

namespace Assert.Domain.Utils
{
    public static class GeoUtils
    {
        private const double EarthRadiusKm = 6371; // Radio de la Tierra en kilómetros

        public static double CalculateDistanceMeters(double lat1, double lon1, double lat2, double lon2)
        {
            //fórmula de Haversine
            const double EarthRadiusMeters = EarthRadiusKm * 1000;

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusMeters * c;
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static List<double> CalculateBoundingBox(double lat, double lon, double radiusMeters)
        {
            const double MetersPerDegreeLat = 111320; // 1 grado de latitud ≈ 111,320 metros
            const double Margin = 1.2; // Margen del 20%

            // Aumentar el radio con el margen
            double adjustedRadiusMeters = radiusMeters * Margin;

            // Convertir el radio a grados de latitud
            double latDegree = adjustedRadiusMeters / MetersPerDegreeLat;

            // Convertir el radio a grados de longitud (ajustado por la latitud)
            double lonDegree = adjustedRadiusMeters / (MetersPerDegreeLat * Math.Cos(ToRadians(lat)));

            // Calcular los límites del bounding box
            double minLat = lat - latDegree;
            double maxLat = lat + latDegree;
            double minLon = lon - lonDegree;
            double maxLon = lon + lonDegree;

            return new List<double>{
                minLat, maxLat, minLon, maxLon
            };
        }

        /// <summary>
        /// Valida si los valores double proporcionados son una latitud y longitud válidas.
        /// </summary>
        /// <param name="latitud">Valor double que representa la latitud.</param>
        /// <param name="longitud">Valor double que representa la longitud.</param>
        /// <returns>True si ambos valores son válidos, False de lo contrario.</returns>
        public static bool ValidateLatitudLongitude(double? latitud, double? longitud)
        {
            return ValidateLatitude(latitud) && ValidateLongitude(longitud);
        }

        /// <summary>
        /// Valida si el valor double proporcionado es una latitud válida.
        /// </summary>
        /// <param name="latitud">Valor double que representa la latitud.</param>
        /// <returns>True si el valor es una latitud válida, False de lo contrario.</returns>
        public static bool ValidateLatitude(double? latitud)
        {
            return latitud >= -90 && latitud <= 90;
        }

        /// <summary>
        /// Valida si el valor double proporcionado es una longitud válida.
        /// </summary>
        /// <param name="longitud">Valor double que representa la longitud.</param>
        /// <returns>True si el valor es una longitud válida, False de lo contrario.</returns>
        public static bool ValidateLongitude(double? longitud)
        {
            return longitud >= -180 && longitud <= 180;
        }
    }
    public static class Tools
    {
        public static string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Trim()
                .ToLowerInvariant()
                .RemoveDiacritics() // Eliminar acentos
                .RemovePunctuation() // Eliminar puntuación
                .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                .Replace("ó", "o").Replace("ú", "u")
                .Replace("-", " ")
                .Replace("  ", " ");
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemovePunctuation(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
        }
    }
}
