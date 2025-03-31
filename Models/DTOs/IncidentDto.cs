using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoReportes.Models
{
    public class IncidentDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Required]
        public string Status { get; set; } = "Pendiente";

        [Required]
        public int? ReportedByAccountId { get; set; }

        public string ReportedByUsername { get; set; } = string.Empty;
    }
}
