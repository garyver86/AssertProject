using System.Security.Cryptography;
using System.Text;

namespace Assert.Infrastructure.Utils
{
    public class UtilsMgr
    {
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
