using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
using ProyectoReportes.Models.DTOs;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProyectoReportes.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public string? GoogleMapsApiKey { get; private set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
            GoogleMapsApiKey = Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY");
        }

        [BindProperty]
        public IncidentDto IncidentDto { get; set; } = new IncidentDto();

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("🚀 OnPostAsync se ha llamado");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState contiene errores:");
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"⚠️ {error.Key}: {err.ErrorMessage}");
                    }
                }
                return Page();
            }


            var (latitude, longitude) = await GetCoordinatesFromAddress(IncidentDto.Address);
            Console.WriteLine($"📍 Coordenadas obtenidas: {latitude}, {longitude}");

            var incident = new Incident
            {
                Description = IncidentDto.Description,
                Address = IncidentDto.Address,
                OccurredAt = DateTime.UtcNow,
                Latitude = latitude,
                Longitude = longitude,
                Status = "Pendiente",
                ReportedByAccountId = 1
            };

            _db.Incidents.Add(incident);
            await _db.SaveChangesAsync();

            Console.WriteLine("✅ Reporte guardado en la base de datos");

            IncidentDto.Latitude = latitude;
            IncidentDto.Longitude = longitude;

            return Page();

        }


        private async Task<(double latitude, double longitude)> GetCoordinatesFromAddress(string address)
        {
            if (string.IsNullOrEmpty(GoogleMapsApiKey))
            {
                throw new Exception("La clave API de Google Maps no está configurada.");
            }

            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={GoogleMapsApiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        var root = doc.RootElement;
                        if (root.GetProperty("status").GetString() == "OK")
                        {
                            var location = root.GetProperty("results")[0].GetProperty("geometry").GetProperty("location");
                            double latitude = location.GetProperty("lat").GetDouble();
                            double longitude = location.GetProperty("lng").GetDouble();
                            return (latitude, longitude);
                        }
                    }
                }
            }
            throw new Exception("No se pudo obtener la ubicación.");
        }
    }
}
