using Microsoft.EntityFrameworkCore;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public PasswordService(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<(bool Exito, string Mensaje)> CambiarPasswordAsync(CambiarPasswordDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Ci == dto.Ci);
        if (usuario == null)
            return (false, "Usuario no encontrado.");

        // 1. Validar la contraseña actual
        bool esActualValida = _passwordHasher.VerificarPasswordHash(dto.PasswordActual, usuario.PasswordHash, usuario.PasswordSalt);
        if (!esActualValida)
            return (false, "La contraseña actual no es correcta.");

        // 2. Procesar el cambio con validación de historial de 5 contraseñas
        return await AplicarCambioPasswordAsync(usuario, dto.PasswordNueva, usuario.Ci.ToString(), "CAMBIO_PASSWORD_USUARIO");
    }

    public async Task<(bool Exito, string Mensaje)> ResetearPasswordPorAdminAsync(ResetearPasswordAdminDto dto, string adminCi)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Ci == dto.CiUsuario);
        if (usuario == null)
            return (false, "Usuario no encontrado.");

        return await AplicarCambioPasswordAsync(usuario, dto.PasswordNueva, adminCi, "RESETEO_PASSWORD_ADMIN");
    }

    private async Task<(bool Exito, string Mensaje)> AplicarCambioPasswordAsync(Usuario usuario, string passwordNueva, string realizadoPor, string tipoAccion)
    {
        // 1. Obtener las últimas 5 contraseñas usadas de la tabla HistorialPassword
        var ultimasCincoClaves = await _context.HistorialesPasswords
            .Where(h => h.UsuarioCi == usuario.Ci)
            .OrderByDescending(h => h.FechaCreacion)
            .Take(5)
            .ToListAsync();

        // 2. Verificar si la contraseña nueva coincide con alguna de las últimas 5
        foreach (var historial in ultimasCincoClaves)
        {
            // Usamos el Salt actual del usuario para verificar el hash almacenado en el historial
            if (_passwordHasher.VerificarPasswordHash(passwordNueva, historial.PasswordHash, usuario.PasswordSalt))
            {
                return (false, "La nueva contraseña no puede coincidir con ninguna de las últimas 5 contraseñas utilizadas.");
            }
        }

        // 3. Generar el nuevo Hash y Salt
        _passwordHasher.CrearPasswordHash(passwordNueva, out byte[] nuevoHash, out byte[] nuevoSalt);

        // Update de los campos en Usuario
        usuario.PasswordHash = nuevoHash;
        usuario.PasswordSalt = nuevoSalt;
        usuario.ExpiradoPorInactividad = false;
        usuario.IntentosFallidos = 0;

        // 4. Registrar la nueva clave en HistorialPassword
        _context.HistorialesPasswords.Add(new HistorialPassword
        {
            PasswordHash = nuevoHash,
            FechaCreacion = DateTime.UtcNow,
            UsuarioCi = usuario.Ci
        });

        // 5. Mapear la acción en HistorialUsuario (Auditoría)
        _context.HistorialesUsuarios.Add(new HistorialUsuario
        {
            Fecha = DateTime.UtcNow,
            TipoAccion = tipoAccion,
            Observaciones = $"Contraseña modificada exitosamente por {realizadoPor}",
            RealizadoPor = realizadoPor,
            UsuarioCi = usuario.Ci
        });

        await _context.SaveChangesAsync();

        return (true, "La contraseña ha sido actualizada correctamente.");
    }
}