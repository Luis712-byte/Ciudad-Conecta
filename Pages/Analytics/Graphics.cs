using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoReportes.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoReportes.Pages.Analytics
{
    public class GraphicsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GraphicsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, int> ReportCounts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ReportCounts = await _context.Incidents
                .GroupBy(i => i.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Status, g => g.Count);

            return Page();
        }
    }
}
