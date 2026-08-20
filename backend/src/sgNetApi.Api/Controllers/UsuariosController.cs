using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;
using System.Security.Claims;
using sgNetApi.Api.Authorization;

namespace sgNetApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requiere Token JWT para todas las operaciones
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UsuariosController(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Listar todos los usuarios con sus catálogos asociados.
    /// </summary>
    [HttpGet]
    [RequirePermission("admin.usuarios.leer")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.Grado)
            .Include(u => u.Escalafon)
            .Include(u => u.UnidadEjecutora)
            .Include(u => u.Dependencia)
            .Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol)
            .Include(u => u.UsuarioPermisos).ThenInclude(up => up.Permiso)
            .Select(u => new UsuarioDetalleDto
            {
                Ci = u.Ci,
                NombreUsuario = u.NombreUsuario,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo,
                Celular = u.Celular,
                Habilitado = u.Habilitado,
                ExpiradoPorInactividad = u.ExpiradoPorInactividad,
                Creado = u.Creado,
                UltimoAcceso = u.UltimoAcceso,
                Grado = u.Grado.Texto,
                Escalafon = u.Escalafon.Nombre,
                UnidadEjecutora = u.UnidadEjecutora.Nombre,
                Dependencia = u.Dependencia.Nombre,
                Roles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList(),
                PermisosDirectos = u.UsuarioPermisos.Select(up => up.Permiso.Nombre).ToList()
            })
            .ToListAsync();

        return Ok(usuarios);
    }

    /// <summary>
    /// Obtener el detalle de un usuario por su Cédula de Identidad.
    /// </summary>
    [HttpGet("{ci}")]
    [RequirePermission("admin.usuarios.leer")]
    public async Task<IActionResult> ObtenerPorCi(long ci)
    {
        var u = await _context.Usuarios
            .Include(u => u.Grado)
            .Include(u => u.Escalafon)
            .Include(u => u.UnidadEjecutora)
            .Include(u => u.Dependencia)
            .Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol)
            .Include(u => u.UsuarioPermisos).ThenInclude(up => up.Permiso)
            .FirstOrDefaultAsync(x => x.Ci == ci);

        if (u == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        var dto = new UsuarioDetalleDto
        {
            Ci = u.Ci,
            NombreUsuario = u.NombreUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Correo = u.Correo,
            Celular = u.Celular,
            Habilitado = u.Habilitado,
            ExpiradoPorInactividad = u.ExpiradoPorInactividad,
            Creado = u.Creado,
            UltimoAcceso = u.UltimoAcceso,
            Grado = u.Grado.Texto,
            Escalafon = u.Escalafon.Nombre,
            UnidadEjecutora = u.UnidadEjecutora.Nombre,
            Dependencia = u.Dependencia.Nombre,
            Roles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList(),
            PermisosDirectos = u.UsuarioPermisos.Select(up => up.Permiso.Nombre).ToList()
        };

        return Ok(dto);
    }

    /// <summary>
    /// Registrar un nuevo usuario en la base de datos.
    /// </summary>
    [HttpPost]
    [RequirePermission("admin.usuarios.crear")]
    public async Task<IActionResult> Crear([FromBody] CrearUsuarioDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Ci == dto.Ci))
            return BadRequest(new { mensaje = "Ya existe un usuario registrado con esa Cédula de Identidad." });

        if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
            return BadRequest(new { mensaje = "Ya existe un usuario registrado con ese correo electrónico." });

        // Si no se provee contraseña, se asigna la Cédula de Identidad como clave por defecto
        string passwordInicial = string.IsNullOrWhiteSpace(dto.Password) ? dto.Ci.ToString() : dto.Password;
        _passwordHasher.CrearPasswordHash(passwordInicial, out byte[] hash, out byte[] salt);

        var usuario = new Usuario
        {
            Ci = dto.Ci,
            NombreUsuario = dto.Ci.ToString(),
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Correo = dto.Correo,
            Celular = dto.Celular,
            PasswordHash = hash,
            PasswordSalt = salt,
            Habilitado = true,
            ExpiradoPorInactividad = false,
            Creado = DateTime.UtcNow,
            IdGrado = dto.IdGrado,
            IdEscalafon = dto.IdEscalafon,
            IdUuee = dto.IdUuee,
            IdDependencia = dto.IdDependencia
        };

        _context.Usuarios.Add(usuario);

        // Asignar Roles
        foreach (var idRol in dto.IdsRoles)
        {
            _context.Set<UsuarioRol>().Add(new UsuarioRol { UsuarioCi = usuario.Ci, IdRol = idRol });
        }

        // Asignar Permisos Directos
        foreach (var idPermiso in dto.IdsPermisosDirectos)
        {
            _context.Set<UsuarioPermiso>().Add(new UsuarioPermiso { UsuarioCi = usuario.Ci, IdPermiso = idPermiso });
        }

        // Registrar Historial
        string adminActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
        _context.HistorialesUsuarios.Add(new HistorialUsuario
        {
            Fecha = DateTime.UtcNow,
            TipoAccion = "CREACION_USUARIO",
            Observaciones = $"Usuario creado por el administrador CI: {adminActual}",
            RealizadoPor = adminActual,
            UsuarioCi = usuario.Ci
        });

        // Guardar primer hash en historial de contraseñas
        _context.HistorialesPasswords.Add(new HistorialPassword
        {
            PasswordHash = hash,
            FechaCreacion = DateTime.UtcNow,
            UsuarioCi = usuario.Ci
        });

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorCi), new { ci = usuario.Ci }, new { mensaje = "Usuario creado exitosamente.", ci = usuario.Ci });
    }

    /// <summary>
    /// Modificar datos y asignaciones de un usuario existente.
    /// </summary>
    [HttpPut("{ci}")]
    [RequirePermission("admin.usuarios.leer")]
    public async Task<IActionResult> Editar(long ci, [FromBody] EditarUsuarioDto dto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.UsuarioRoles)
            .Include(u => u.UsuarioPermisos)
            .FirstOrDefaultAsync(u => u.Ci == ci);

        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        usuario.Nombre = dto.Nombre;
        usuario.Apellido = dto.Apellido;
        usuario.Correo = dto.Correo;
        usuario.Celular = dto.Celular;
        usuario.IdGrado = dto.IdGrado;
        usuario.IdEscalafon = dto.IdEscalafon;
        usuario.IdUuee = dto.IdUuee;
        usuario.IdDependencia = dto.IdDependencia;

        // Actualizar Roles (Reemplazar asignaciones anteriores)
        _context.Set<UsuarioRol>().RemoveRange(usuario.UsuarioRoles);
        foreach (var idRol in dto.IdsRoles)
        {
            _context.Set<UsuarioRol>().Add(new UsuarioRol { UsuarioCi = usuario.Ci, IdRol = idRol });
        }

        // Actualizar Permisos Directos
        _context.Set<UsuarioPermiso>().RemoveRange(usuario.UsuarioPermisos);
        foreach (var idPermiso in dto.IdsPermisosDirectos)
        {
            _context.Set<UsuarioPermiso>().Add(new UsuarioPermiso { UsuarioCi = usuario.Ci, IdPermiso = idPermiso });
        }

        string adminActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
        _context.HistorialesUsuarios.Add(new HistorialUsuario
        {
            Fecha = DateTime.UtcNow,
            TipoAccion = "EDICION_USUARIO",
            Observaciones = $"Datos actualizados por administrador CI: {adminActual}",
            RealizadoPor = adminActual,
            UsuarioCi = usuario.Ci
        });

        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Usuario actualizado correctamente." });
    }

    /// <summary>
    /// Cambiar el estado de Habilitado/Deshabilitado de un usuario (Bloqueo/Desbloqueo).
    /// </summary>
    [HttpPatch("{ci}/estado")]
    [RequirePermission("admin.usuarios.leer")]
    public async Task<IActionResult> CambiarEstado(long ci, [FromBody] CambiarEstadoUsuarioDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Ci == ci);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        usuario.Habilitado = dto.Habilitado;
        if (dto.Habilitado)
        {
            // Al ser re-habilitado por un administrador, se reinicia el contador de intentos
            usuario.IntentosFallidos = 0;
            usuario.ExpiradoPorInactividad = false;
        }

        string adminActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
        string estadoTexto = dto.Habilitado ? "HABILITADO" : "DESHABILITADO";

        _context.HistorialesUsuarios.Add(new HistorialUsuario
        {
            Fecha = DateTime.UtcNow,
            TipoAccion = $"CAMBIO_ESTADO_{estadoTexto}",
            Observaciones = string.IsNullOrWhiteSpace(dto.Observacion) 
                ? $"Estado cambiado a {estadoTexto} por administrador CI: {adminActual}"
                : dto.Observacion,
            RealizadoPor = adminActual,
            UsuarioCi = usuario.Ci
        });

        await _context.SaveChangesAsync();

        return Ok(new { mensaje = $"El estado del usuario ha sido cambiado a: {estadoTexto}." });
    }
}