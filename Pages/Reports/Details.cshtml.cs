using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
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

        public Incident? Incident { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Incident = await _context.Incidents.FindAsync(id);
            if (Incident == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
