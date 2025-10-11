using Assert.Domain.Entities;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Assert.Infrastructure.Utils
{
    public class UtilsMgr
    {
        public static decimal CalculateAverageCalification(Decimal? AverageReviews, ICollection<TlListingReview> reviews)
        {
            if (AverageReviews > 0)
            {
                return AverageReviews.Value;
            }

            if (reviews == null || !reviews.Any())
            {
                return 0; // Or return 0, or throw an exception, depending on your needs
            }

            //  Important: Cast to decimal to ensure decimal arithmetic
            return (decimal?)reviews.Average(y => (double)y.Calification) ?? 0;
        }

        public static string GetHash512(string text)
        {
            string hash = null;
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
            return hash;
        }

        public static string GetOTPCode()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString();
        }

        public static string LoadOtpTemplate(string user, string otpCode)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Assert.Infrastructure.External.Notifications.Templates.otpNotification.html";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"No se encontró el recurso embebido: {resourceName}");

            using var reader = new StreamReader(stream);
            var html = reader.ReadToEnd();

            return html?
                .Replace("{user}", WebUtility.HtmlEncode(user))
                .Replace("{otpCode}", WebUtility.HtmlEncode(otpCode)) ?? $"Estimado {user},\n\nUtilice el siguiente codigo en la plataforma Assert: {otpCode}";
        }

        public static string? GetTimeElapsed(DateTime? authorizationDateTime, DateTime? requestDateTime)
        {
            try
            {
                if (!authorizationDateTime.HasValue || !requestDateTime.HasValue)
                    return null;

                if (authorizationDateTime.Value <= requestDateTime.Value)
                    return "00:00";

                TimeSpan timeElapsed = authorizationDateTime.Value - requestDateTime.Value;

                // Calcular horas y minutos
                int totalHours = (int)timeElapsed.TotalHours;
                int minutes = timeElapsed.Minutes;

                return $"{totalHours:00}:{minutes:00}";
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
