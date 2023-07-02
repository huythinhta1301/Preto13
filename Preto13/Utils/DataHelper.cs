using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Linq;
using Preto13.Config;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BCryptNet = BCrypt.Net.BCrypt;
namespace Preto13.Utils
{
    public class DataHelper
    {

        public void encryptPassword(string password, out byte[] pwdHash, out byte[] pwdSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                pwdHash = hmac.Key;
                pwdSalt = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public string GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return Convert.ToBase64String(salt);
        }

        public byte[] GetHash(string password, string salt)
        {
            byte[] hash = Encoding.UTF8.GetBytes(String.Concat(salt,password));
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashedData = sha256.ComputeHash(hash);
            return hashedData;
        }
        public bool CompareHashedPassword(string password, string hashedData, string salt)
        {
            string data = Convert.ToBase64String(GetHash(password, salt));
            return data.Equals(hashedData);
        }
        public bool VerifyPasswordHash(string password, byte[] pwdHash, byte[] pwdSalt)
        {
            using(var hmac = new HMACSHA512(pwdSalt)) 
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(pwdHash);
            }
        }
        //public static string EncryptPassword(string password)
        //{
        //    return BCryptNet.HashPassword(password);
        //}

        //public static bool VerifyPassword(string password, string hashedPassword)
        //{
        //    return BCryptNet.Verify(password, hashedPassword);
        //}
        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Compare the computed hash with the stored hash
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false; // Password does not match
                    }
                }

                return true; // Password matches
            }
        }
        public string pwdByteToString(byte[] password)
        {
            return Convert.ToBase64String(password);
        }

        //public string encryptString(string content)
        //{
        //    string secret_key = ConfigHelper.GetAppSetting("jwtKey");
        //}
        public string CreateJWT(JObject data)
        {
            string secret_key = ConfigHelper.GetAppSetting("jwtKey");
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(data, secret_key);
            return token.ToString();
        }

        public bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public bool IsMatch(string value, string pattern)
        {
            return Regex.IsMatch(value, pattern);
        }

    }
}
