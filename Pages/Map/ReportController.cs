using Microsoft.AspNetCore.Mvc;
using ProyectoReportes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportService.GetAllReportsAsync();
        return Ok(reports);
    }
}
