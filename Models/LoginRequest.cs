using System.ComponentModel.DataAnnotations;

namespace APIPoliza.Models
{
    public class LoginRequest
    {
        [Required]
        public string UserPoliza { get; set; } = string.Empty;
        [Required]
        public string PasswordPoliza { get; set; } = string.Empty;
    }
}