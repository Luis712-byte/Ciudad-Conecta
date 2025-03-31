using Microsoft.AspNetCore.Mvc;
using ProyectoReportes.Models;
using ProyectoReportes.Data;
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
    public async Task<List<IncidentDto>> GetAllReportsAsync()
    {
        return await _reportService.GetAllReportsAsync();
    }
}
