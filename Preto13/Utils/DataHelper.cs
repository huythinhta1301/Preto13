using Preto13.Config;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Preto13.Utils
{
    public class DataHelper
    {
        
        public void encryptPassword(string password, out byte[] pwdHash, out byte[] pwdSalt) 
        { 
            using(var hmac = new HMACSHA512())
            {
                pwdHash = hmac.Key;
                pwdSalt = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public bool IsMatch(string value, string pattern)
        {
            return Regex.IsMatch(value, pattern);
        }

        public string checkRegexValid(string inputName)
        {
            switch(inputName)
            {
                case "username":
                    return @"^[a-zA-Z0-9]+$";
                case "email":
                    return @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]+$";
                case "phone":
                    return @"^\d{10}$";
                case "password":
                    return @"^(?=.*[A-Z])[a-zA-Z0-9]{6,18}$";
                default:
                    return string.Empty;
            }
        }
    }
}
