using System.ComponentModel.DataAnnotations;

namespace Verbum.Identity.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
