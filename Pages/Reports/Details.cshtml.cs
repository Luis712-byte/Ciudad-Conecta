using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoReportes.Pages.Reports
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Incident> Incidents { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            Incidents = await _context.Incidents
                .Where(i => i.ReportedByAccountId == userId && i.Status == "Pendiente")
                .OrderByDescending(i => i.OccurredAt)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostMarkAsResolvedAsync(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            incident.Status = "Resuelto";
            await _context.SaveChangesAsync();

            return RedirectToPage(new { userId = incident.ReportedByAccountId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { userId = incident.ReportedByAccountId });
        }
    }
}
