using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProyectoReportes.Data;
using ProyectoReportes.Models;
using ProyectoReportes.Models.DTOs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Accounts.AnyAsync(a => a.Email == request.Email))
            return "El correo ya está registrado.";

        var account = new Account
        {
            Email = request.Email,
            Username = request.Username ?? "",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            Profile = new UserProfile
            {
                FullName = request.FullName ?? "",
                Cedula = request.Cedula ?? "",
                PhoneNumber = request.PhoneNumber ?? "",
                DateOfBirth = request.DateOfBirth
            }
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return "Cuenta registrada correctamente";
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var account = await _context.Accounts.FindAsync(request.Username);
        if (account == null)
            return "El correo o contraseña son incorrectos.";
        if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash))
            return "El correo o contraseña son incorrectos.";

        return GenerateJwtToken(account);
    }

    private string GenerateJwtToken(Account account)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", account.AccountId.ToString())
        };

        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
