using System;
using System.Security.Cryptography;

namespace yotelollevo.Security
{
    public static class PasswordHasher
    {
        public static void CreateHash(string password, out byte[] hash, out byte[] salt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(64);
            }
        }

        public static bool Verify(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (storedHash == null || storedSalt == null) return false;

            byte[] computed;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 100_000, HashAlgorithmName.SHA256))
            {
                computed = pbkdf2.GetBytes(64);
            }

            return FixedTimeEquals(computed, storedHash);
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
    }
}
