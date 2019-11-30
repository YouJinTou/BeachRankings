using BR.Core.Tools;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;
using System.Security.Cryptography;

namespace BR.Iam.Tools
{
    public static class Hasher
    {
        public static DbHash GetHash(string password)
        {
            Validator.ThrowIfNullOrWhiteSpace(password, "Password is empty.");

            var salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = new DbHash
            {
                Hash = GetHashedPassword(password, salt),
                Salt = salt
            };

            return hash;
        }

        public static bool IsValidPassword(byte[] oldHash, string password, byte[] salt)
        {
            var hashedPassword = GetHashedPassword(password, salt);

            return oldHash.SequenceEqual(hashedPassword);
        }

        private static byte[] GetHashedPassword(string password, byte[] salt)
        {
            var pbkdf2 = KeyDerivation.Pbkdf2(
               password: password,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: 10000,
               numBytesRequested: 256 / 8);

            return pbkdf2;
        }
    }
}
