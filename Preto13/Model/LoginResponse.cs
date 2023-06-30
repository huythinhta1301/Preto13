namespace Preto13.Model
{
    public class LoginResponse
    {
        public string? message { get; set; }
        public bool status { get; set; } = false;
        public string code { get; set; } = "-1001";
        public string token { get; set; }
    }
}
