using Assert.Domain.Entities;
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
    }
}
