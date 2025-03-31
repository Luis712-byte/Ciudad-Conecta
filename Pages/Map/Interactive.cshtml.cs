using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoReportes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoReportes.Pages.Map
{
    public class InteractiveModel : PageModel
    {
        private readonly ReportService _reportService;

        public List<IncidentDto> Reports { get; set; }

        public InteractiveModel(ReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Reports = await _reportService.GetAllReportsAsync();
            return Page();
        }
    }
}
