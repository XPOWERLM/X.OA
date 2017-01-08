using System;
using System.Security.Cryptography;

namespace X.OA.Common.Utility
{
    public static class PasswordUtility
    {
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        public const int PBKDF2_ITERATIONS = 48424;

        public const int ITERATION_INDEX = 0;
        public const int SALT_INDEX = 1;
        public const int PBKDF2_INDEX = 2;

        /// <summary>
        /// Create a salted PBKDF2 hash of the password
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>Iterations:Salt:Hash</returns>
        public static string CreateHash(string password)
        {
            // Create a byte array to hold the random value
            byte[] salt = new byte[SALT_BYTE_SIZE];
            // Generate a random salt
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(salt);
            }

            // Hash the password and encode the parameters
            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);

            // Format : Iterations:Salt:Hash
            return $"{PBKDF2_ITERATIONS}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Validate password
        /// </summary>
        /// <param name="password">Plain password.</param>
        /// <param name="hash">Iterations:Salt:Hash</param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string hash)
        {
            string[] split = hash.Split(':');
            int iterations = int.Parse(split[ITERATION_INDEX]);
            byte[] salt =Convert.FromBase64String(split[SALT_INDEX]);
            byte[] dbPassword =Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, iterations, SALT_BYTE_SIZE);
            return SlowEquals(dbPassword, testHash);
        }

        /// <summary>
        /// Compute the PBKDF2-SHA1 hash of a password
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="hashSize">The length of the hash to generate, in bytes</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int hashSize)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(hashSize);
            }
        }

        /// <summary>
        /// Compares two byte arrays in length-contant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns></returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)a[i] ^ b[i];
            return diff == 0;
        }

    }
}
