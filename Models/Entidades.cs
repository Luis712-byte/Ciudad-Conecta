using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoReportes.Models
{
    // Entidad para la autenticación (login)
    [Table("CT_Accounts")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public UserProfile? Profile { get; set; }

    }

    // Entidad para la información complementaria del usuario
    [Table("CT_UserProfiles")]
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        public Account Account { get; set; } = new Account();

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [StringLength(250)]
        public string? ImageUrl { get; set; }
    }

    // Entidad para gestionar incidencias
    [Table("CT_Incidents")]
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        // Usamos OccurredAt en lugar de CreatedAt
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        public int? ReportedByAccountId { get; set; }

        [ForeignKey(nameof(ReportedByAccountId))]
        public Account? ReportedBy { get; set; }
    }
}
