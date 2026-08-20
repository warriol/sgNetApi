using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // La versión 1.6.22 utiliza Microsoft.OpenApi.Models
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;
using sgNetApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using sgNetApi.Api.Authorization;
using sgNetApi.Api.Middlewares;
using sgNetApi.Infrastructure.Workers;

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar variables de entorno desde .env
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "../../.env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

// 2. Configurar Conexión a PostgreSQL
var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "sgnet_db";
var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "sgnet_user";
var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "sgnet_password";

var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. Registrar Servicios de Infraestructura
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<DataSeeder>();
// Registrar el evaluador y proveedor dinámico de políticas de permisos
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
// Registrar el servicio en segundo plano para depuración automática
builder.Services.AddHostedService<AuditoriaCleanupWorker>();

// 4. Configurar Autenticación JWT Bearer
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "ClaveSecretaSuperSeguraSGNetApi2026_Uruguay!";
var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "sgNetApi",
        ValidateAudience = true,
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "sgNetClient",
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 5. Configurar SwaggerGen con esquema JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "sgNetApi",
        Version = "v1",
        Description = "API del Sistema de Gestión de Información"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT devuelto por /api/Auth/login."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 6. Ejecutar DataSeeder al arrancar
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

// Registrar Middleware de Auditoría y Excepciones al inicio del pipeline HTTP
app.UseMiddleware<AuditAndExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "sgNetApi v1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();