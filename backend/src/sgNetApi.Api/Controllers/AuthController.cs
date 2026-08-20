using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;
using sgNetApi.Api.Authorization;

namespace sgNetApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordService _passwordService;

    public AuthController(
        AppDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordService passwordService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordService = passwordService;
    }

    /// <summary>
    /// Permite al usuario autenticado cambiar su propia contraseña.
    /// </summary>
    [HttpPost("cambiar-password")]
    [Authorize]
    public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDto dto)
    {
        var (exito, mensaje) = await _passwordService.CambiarPasswordAsync(dto);
        if (!exito)
            return BadRequest(new { mensaje });

        return Ok(new { mensaje });
    }

    /// <summary>
    /// Permite a un Administrador resetear la contraseña de un usuario.
    /// </summary>
    [HttpPost("resetear-password")]
    [Authorize]
    [RequirePermission("admin.usuarios.editar")]
    public async Task<IActionResult> ResetearPassword([FromBody] ResetearPasswordAdminDto dto)
    {
        string adminCi = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
        var (exito, mensaje) = await _passwordService.ResetearPasswordPorAdminAsync(dto, adminCi);

        if (!exito)
            return BadRequest(new { mensaje });

        return Ok(new { mensaje });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        // 1. Buscar el usuario por Cédula de Identidad
        var usuario = await _context.Usuarios
            .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                    .ThenInclude(r => r.RolPermisos)
                        .ThenInclude(rp => rp.Permiso)
            .Include(u => u.UsuarioPermisos)
                .ThenInclude(up => up.Permiso)
            .FirstOrDefaultAsync(u => u.Ci == dto.Ci);

        if (usuario == null)
        {
            return Unauthorized(new { mensaje = "Cédula o contraseña incorrecta." });
        }

        // 2. Validar estado de la cuenta
        if (!usuario.Habilitado)
        {
            return StatusCode(403, new { mensaje = "La cuenta está deshabilitada por intentos fallidos o decisión administrativa." });
        }

        if (usuario.ExpiradoPorInactividad)
        {
            return StatusCode(403, new { mensaje = "La cuenta ha expirado por inactividad (>30 días). Debe solicitar una nueva contraseña." });
        }

        // 3. Validar Hash de la Contraseña
        bool esValido = _passwordHasher.VerificarPasswordHash(dto.Password, usuario.PasswordHash, usuario.PasswordSalt);

        if (!esValido)
        {
            usuario.IntentosFallidos++;

            // Regla de Negocio: 3 intentos fallidos deshabilitan la cuenta
            if (usuario.IntentosFallidos >= 3)
            {
                usuario.Habilitado = false;
                _context.HistorialesUsuarios.Add(new HistorialUsuario
                {
                    Fecha = DateTime.UtcNow,
                    TipoAccion = "BLOQUEO_INTENTOS",
                    Observaciones = "Cuenta bloqueada automáticamente tras 3 intentos fallidos de inicio de sesión.",
                    RealizadoPor = "SISTEMA",
                    UsuarioCi = usuario.Ci
                });
            }

            await _context.SaveChangesAsync();
            return Unauthorized(new { mensaje = "Cédula o contraseña incorrecta." });
        }

        // 4. Resetear contador de intentos e ingresar acceso exitoso
        usuario.IntentosFallidos = 0;
        usuario.UltimoAcceso = DateTime.UtcNow;

        _context.HistorialesUsuarios.Add(new HistorialUsuario
        {
            Fecha = DateTime.UtcNow,
            TipoAccion = "LOGIN_EXITOSO",
            Observaciones = "Inicio de sesión correcto.",
            RealizadoPor = usuario.Ci.ToString(),
            UsuarioCi = usuario.Ci
        });

        await _context.SaveChangesAsync();

        // 5. Consolidar Roles y Permisos (Sin duplicados)
        var roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList();

        var permisosDeRoles = usuario.UsuarioRoles
            .SelectMany(ur => ur.Rol.RolPermisos)
            .Select(rp => rp.Permiso.Nombre);

        var permisosDirectos = usuario.UsuarioPermisos
            .Select(up => up.Permiso.Nombre);

        var permisosTotales = permisosDeRoles.Union(permisosDirectos).Distinct().ToList();

        // 6. Generar el Token JWT
        var token = _jwtTokenGenerator.GenerarToken(usuario, roles, permisosTotales);

        return Ok(new LoginResponseDto
        {
            Token = token,
            Expiracion = DateTime.UtcNow.AddHours(8),
            Ci = usuario.Ci,
            NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
            Correo = usuario.Correo,
            Roles = roles,
            Permisos = permisosTotales
        });
    }
}