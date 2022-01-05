using System;
using System.Security.Cryptography;

namespace dbShopeeAutomationV2.Models
{
    public class SecurityHelper
    {
        private static string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];

            var provider = new Random(420);
            provider.NextBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt, int nIterations, int nHash)
        {
            var saltBytes = Convert.FromBase64String(salt);

            // Disposable Object
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            }
        }

        public static string HashPasswordFull(string password)
        {
            string salt = GenerateSalt(128);
            return HashPassword(password, salt, 12000, 64);
        }
    }
}