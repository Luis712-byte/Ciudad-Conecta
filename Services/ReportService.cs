using Microsoft.EntityFrameworkCore;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<IncidentDto>> GetAllReportsAsync()
    {
        var incidents = await _context.Incidents.ToListAsync();

        return incidents.Select(i => new IncidentDto
        {
            Id = i.IncidentId,  
            Description = i.Description,
            Address = i.Address,
            OccurredAt = i.OccurredAt,
            Latitude = i.Latitude,
            Longitude = i.Longitude,
            Status = i.Status,
            ReportedByAccountId = i.ReportedByAccountId ?? 0  
        }).ToList();
    }

}
