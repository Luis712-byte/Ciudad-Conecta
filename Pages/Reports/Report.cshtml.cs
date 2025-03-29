using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
using System.Threading.Tasks;

namespace ProyectoReportes.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Incident? Incident { get; set; }

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // This method is intentionally left empty. The page will be displayed with the form for creating a new incident.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Incident != null)
            {
                _context.Incidents.Add(Incident);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Dashboard/Index");
        }
    }
}
