using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProyectoReportes.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

string? rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (rawConnectionString is null)
{
    throw new Exception("La cadena de conexión no está configurada.");
}

string? dbPassword = Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD");
if (string.IsNullOrEmpty(dbPassword))
{
    if (!builder.Environment.IsDevelopment())
    {
        throw new Exception("La contraseña de la base de datos no está configurada.");
    }
    else
    {
        dbPassword = "valor_de_prueba";
    }
}

string connectionString = rawConnectionString.Replace("{PASSWORD}", dbPassword);
// Console.WriteLine($"Connection String: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddRazorPages();
builder.Services.AddControllers();builder.Services.AddScoped<ReportService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

string? keyJWT = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
if (string.IsNullOrEmpty(keyJWT))
{
    if (!builder.Environment.IsDevelopment())
    {
        throw new Exception("La contraseña de la base de datos no está configurada.");
    }
    else
    {
        keyJWT = "valor_de_prueba";
    }
}

string jwtConnectionString = rawConnectionString.Replace("{JWT}", keyJWT);
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJWT));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = signingKey
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.Run();
