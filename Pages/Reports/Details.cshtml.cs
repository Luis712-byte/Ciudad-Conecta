using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoReportes.Pages.Reports
{
    [Authorize]  // Solo usuarios autenticados pueden acceder a esta página
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Incident> Incidents { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToPage("/Account/Login");
            }

            Incidents = await _context.Incidents
                .Where(i => i.ReportedByUsername == userName && i.Status == "Pendiente")
                .OrderByDescending(i => i.OccurredAt)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = Request.Form["id"];
            var action = Request.Form["action"];

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(int.Parse(id));
            if (incident == null)
            {
                return NotFound();
            }

            if (action == "Resolve")
            {
                incident.Status = "Resuelto";
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "El reporte se marcó como resuelto correctamente.";
            }
            else if (action == "Delete")
            {
                _context.Incidents.Remove(incident);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "El reporte fue eliminado correctamente.";
            }

            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            Incidents = await _context.Incidents
                .Where(i => i.ReportedByUsername == userName && i.Status == "Pendiente")
                .OrderByDescending(i => i.OccurredAt)
                .ToListAsync();

            return Page();
        }

    }
}
