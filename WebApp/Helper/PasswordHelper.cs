using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApp.Helper
{
    public class PasswordHelper
    {
        private string salt;
        private string password;

        public bool PasswordVertify(string password, string password_db, string salt)
        {

            string hash = HashPassword(password, salt);

            return hash == password_db;
        }

        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combine = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combine, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combine, saltBytes.Length, passwordBytes.Length);
            HashAlgorithm hashAlgorithm = new SHA256Managed();
            byte[] hashBytes = hashAlgorithm.ComputeHash(combine);
            return Convert.ToBase64String(hashBytes);
        }
    }
}