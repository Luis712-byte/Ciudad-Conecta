using System.ComponentModel.DataAnnotations;

namespace ProyectoReportes.Models.DTOs
{
    public class LoginRequest
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
