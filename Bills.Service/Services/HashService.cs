using Bills.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Service.Services
{
    public class HashService : IHashService
    {
        public byte[] GenerateSalt(int size = 128)
        {
            var salt = new byte[size];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public byte[] ComputeHash(string password, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(salt))
            {
                return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            var computedHash = ComputeHash(enteredPassword, storedSalt);
            return computedHash.SequenceEqual(storedHash);
        }
    }
}
