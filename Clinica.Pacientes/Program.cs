using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Falta Jwt:Key");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = "role",
            NameClaimType = "sub"
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Datos en memoria (sin base de datos)
var pacientes = new List<Paciente>
{
    new(1, "Juan Pérez",   "45678912", "999111222"),
    new(2, "María Gómez",  "41236547", "999333444"),
    new(3, "Pedro Quispe", "70123456", "999555666"),
    new(4, "Ana Flores",   "48965231", "999777888")
};

app.MapGet("/api/pacientes", () => Results.Ok(pacientes))
   .RequireAuthorization();

app.MapGet("/api/pacientes/{id:int}", (int id) =>
    pacientes.FirstOrDefault(p => p.Id == id) is { } p
        ? Results.Ok(p)
        : Results.NotFound())
   .RequireAuthorization();

app.Run();

record Paciente(int Id, string Nombre, string Dni, string Telefono);