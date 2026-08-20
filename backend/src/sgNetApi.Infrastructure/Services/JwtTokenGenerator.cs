using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;

namespace sgNetApi.Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerarToken(Usuario usuario, List<string> roles, List<string> permisos)
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
            ?? "ClaveSecretaSuperSeguraSGNetApi2026_Uruguay!";
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "sgNetApi";
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "sgNetClient";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims principales del usuario
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Ci.ToString()),
            new(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
            new(ClaimTypes.Email, usuario.Correo),
            new("NombreUsuario", usuario.NombreUsuario)
        };

        // Agregar los roles como claims estándar
        foreach (var rol in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }

        // Agregar los permisos consolidados como claims personalizados
        foreach (var permiso in permisos)
        {
            claims.Add(new Claim("permiso", permiso));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8), // El token expira en 8 horas
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}