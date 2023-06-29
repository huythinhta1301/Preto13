namespace Preto13.Utils
{
    public class RegexPattern
    {
        public const string UsernamePattern = @"^[a-zA-Z0-9]+$";
        public const string EmailPattern = @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]+$";
        public const string PhonePattern = @"^\d{10}$";
    }
}
