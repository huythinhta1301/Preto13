using Newtonsoft.Json.Linq;
using Preto13.Utils;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Preto13.Model.DTO
{
    public class USER_REGISTER_DTO
    {
        [Required(ErrorMessage = "USERNAME IS REQUIRED")]
        [RegularExpression(RegexPattern.UsernamePattern, ErrorMessage = "Invalid username format")]
        public string? username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(RegexPattern.PhonePattern, ErrorMessage = "Invalid phone format")]
        public string? phone { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(RegexPattern.EmailPattern, ErrorMessage = "Invalid email format")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("password", ErrorMessage = "Passwords do not match")]
        public string? confirmPassword { get; set; }
    }
}
