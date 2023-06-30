using System.ComponentModel.DataAnnotations;

namespace Preto13.Model.DTO
{
    public class USER_LOGIN_DTO
    {
        [Required(ErrorMessage ="LOGIN IDENTIFY REQUIRED")]
        public string? LoginIdentify { get; set; }
        [Required(ErrorMessage = "PASSWORD REQUIRED")]
        public string Password { get; set; }
    }
}
