using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoReportes.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Incident> Incidents { get; set; } = new List<Incident>();

        public async Task OnGetAsync()
        {
            Incidents = await _context.Incidents.ToListAsync();
        }
    }
}
