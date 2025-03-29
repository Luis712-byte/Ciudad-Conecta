using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoReportes.Data;
using ProyectoReportes.Models.DTOs;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoReportes.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterRequest RegisterRequest { get; set; } = new RegisterRequest();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage); 
                }
                return Page();
            }

            var existingUser = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == RegisterRequest.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "El nombre de usuario ya está en uso.");
                return Page();
            }

            if (RegisterRequest == null)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Inténtalo de nuevo.");
                return Page();
            }

            var newUser = new ProyectoReportes.Models.Account
            {
                Username = RegisterRequest.Username,
                Email = RegisterRequest.Email,
                PasswordHash = HashPassword(RegisterRequest.Password)
            };

            _context.Accounts.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
