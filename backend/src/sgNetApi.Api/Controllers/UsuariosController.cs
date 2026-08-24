using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;
using sgNetApi.Infrastructure.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
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
            .Include(u => u.Dependencia).ThenInclude(d => d!.UnidadEjecutora)
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
                FechaCreacion = u.FechaCreacion,
                UltimoAcceso = u.UltimoAcceso,
                FechaNacimiento = u.FechaNacimiento,
                IdNacionalidad = u.IdNacionalidad,
                IdEstadoCivil = u.IdEstadoCivil,
                IdProfesion = u.IdProfesion,
                IdGrado = u.IdGrado,
                IdEscalafon = u.IdEscalafon,
                IdDependencia = u.IdDependencia,
                Grado = u.Grado.Texto,
                Escalafon = u.Escalafon.Nombre,
                UnidadEjecutora = u.Dependencia!.UnidadEjecutora.Nombre,
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
    public async Task<IActionResult> ObtenerPorCi(string ci)
    {
        var u = await _context.Usuarios
            .Include(u => u.Grado)
            .Include(u => u.Escalafon)
            .Include(u => u.Dependencia).ThenInclude(d => d!.UnidadEjecutora)
            .Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol)
            .Include(u => u.UsuarioPermisos).ThenInclude(up => up.Permiso)
                .FirstOrDefaultAsync(x => x.NombreUsuario == ci);

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
            FechaCreacion = u.FechaCreacion,
            UltimoAcceso = u.UltimoAcceso,
            FechaNacimiento = u.FechaNacimiento,
            IdNacionalidad = u.IdNacionalidad,
            IdEstadoCivil = u.IdEstadoCivil,
            IdProfesion = u.IdProfesion,
            IdGrado = u.IdGrado,
            IdEscalafon = u.IdEscalafon,
            IdDependencia = u.IdDependencia,
            Grado = u.Grado.Texto,
            Escalafon = u.Escalafon.Nombre,
            UnidadEjecutora = u.Dependencia!.UnidadEjecutora.Nombre,
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
        var nacionalidad = await _context.Nacionalidades.FindAsync(dto.IdNacionalidad);
        if (nacionalidad == null)
            return BadRequest(new { mensaje = "La nacionalidad indicada no existe." });

        if (nacionalidad.EsUruguaya)
        {
            if (!dto.Ci.HasValue || dto.Ci.Value < 0 || dto.Ci.Value > 999999999)
                return BadRequest(new { mensaje = "La CI uruguaya debe tener exactamente 9 dígitos." });

            dto.NombreUsuario = dto.Ci.Value.ToString("D9");
        }
        else if (string.IsNullOrWhiteSpace(dto.NombreUsuario) || !Regex.IsMatch(dto.NombreUsuario, "^[A-Za-z0-9]+$"))
        {
            return BadRequest(new { mensaje = "El usuario extranjero debe ser alfanumérico." });
        }

        if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == dto.NombreUsuario))
            return BadRequest(new { mensaje = "Ya existe un usuario registrado con esa Cédula de Identidad." });

        if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
            return BadRequest(new { mensaje = "Ya existe un usuario registrado con ese correo electrónico." });

        // Si no se provee contraseña, se asigna la Cédula de Identidad como clave por defecto
        string passwordInicial = string.IsNullOrWhiteSpace(dto.Password) ? dto.NombreUsuario : dto.Password;
        string hash = _passwordHasher.CrearPasswordHash(passwordInicial);

        var usuario = new Usuario
        {
            Ci = dto.Ci,
            NombreUsuario = dto.NombreUsuario,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Correo = dto.Correo,
            Celular = dto.Celular,
            FechaNacimiento = dto.FechaNacimiento,
            IdNacionalidad = dto.IdNacionalidad,
            IdEstadoCivil = dto.IdEstadoCivil,
            IdProfesion = dto.IdProfesion,
            PasswordHash = hash,
            Habilitado = true,
            ExpiradoPorInactividad = false,
            FechaCreacion = DateTime.UtcNow,
            IdGrado = dto.IdGrado,
            IdEscalafon = dto.IdEscalafon,
            IdDependencia = dto.IdDependencia
        };

        _context.Usuarios.Add(usuario);

        // Asignar Roles
        foreach (var idRol in dto.IdsRoles)
        {
            _context.Set<UsuarioRol>().Add(new UsuarioRol { NombreUsuario = usuario.NombreUsuario, IdRol = idRol });
        }

        // Asignar Permisos Directos
        foreach (var idPermiso in dto.IdsPermisosDirectos)
        {
            _context.Set<UsuarioPermiso>().Add(new UsuarioPermiso { NombreUsuario = usuario.NombreUsuario, IdPermiso = idPermiso });
        }

        // Registrar Historial
        string adminActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
        _context.HistorialesUsuarios.Add(new HistorialUsuario
        {
            Fecha = DateTime.UtcNow,
            TipoAccion = "CREACION_USUARIO",
            Observaciones = $"Usuario creado por el administrador CI: {adminActual}",
            RealizadoPor = adminActual,
            UsuarioNombreUsuario = usuario.NombreUsuario
        });

        // Guardar primer hash en historial de contraseñas
        _context.HistorialesPasswords.Add(new HistorialPassword
        {
            PasswordHash = hash,
            FechaCreacion = DateTime.UtcNow,
            UsuarioNombreUsuario = usuario.NombreUsuario
        });

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorCi), new { ci = usuario.NombreUsuario }, new { mensaje = "Usuario creado exitosamente.", nombreUsuario = usuario.NombreUsuario });
    }

    /// <summary>
    /// Modificar datos y asignaciones de un usuario existente.
    /// </summary>
    [HttpPut("{ci}")]
    [RequirePermission("admin.usuarios.leer")]
    public async Task<IActionResult> Editar(string ci, [FromBody] EditarUsuarioDto dto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.UsuarioRoles)
            .Include(u => u.UsuarioPermisos)
                .FirstOrDefaultAsync(u => u.NombreUsuario == ci);

        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        usuario.Nombre = dto.Nombre;
        usuario.Apellido = dto.Apellido;
        usuario.Correo = dto.Correo;
        usuario.Celular = dto.Celular;
        usuario.FechaNacimiento = dto.FechaNacimiento;
        usuario.IdNacionalidad = dto.IdNacionalidad;
        usuario.IdEstadoCivil = dto.IdEstadoCivil;
        usuario.IdProfesion = dto.IdProfesion;
        usuario.IdGrado = dto.IdGrado;
        usuario.IdEscalafon = dto.IdEscalafon;
        usuario.IdDependencia = dto.IdDependencia;

        // Actualizar Roles (Reemplazar asignaciones anteriores)
        _context.Set<UsuarioRol>().RemoveRange(usuario.UsuarioRoles);
        foreach (var idRol in dto.IdsRoles)
        {
            _context.Set<UsuarioRol>().Add(new UsuarioRol { NombreUsuario = usuario.NombreUsuario, IdRol = idRol });
        }

        // Actualizar Permisos Directos
        _context.Set<UsuarioPermiso>().RemoveRange(usuario.UsuarioPermisos);
        foreach (var idPermiso in dto.IdsPermisosDirectos)
        {
            _context.Set<UsuarioPermiso>().Add(new UsuarioPermiso { NombreUsuario = usuario.NombreUsuario, IdPermiso = idPermiso });
        }

        string adminActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
        _context.HistorialesUsuarios.Add(new HistorialUsuario
        {
            Fecha = DateTime.UtcNow,
            TipoAccion = "EDICION_USUARIO",
            Observaciones = $"Datos actualizados por administrador CI: {adminActual}",
            RealizadoPor = adminActual,
            UsuarioNombreUsuario = usuario.NombreUsuario
        });

        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Usuario actualizado correctamente." });
    }

    /// <summary>
    /// Cambiar el estado de Habilitado/Deshabilitado de un usuario (Bloqueo/Desbloqueo).
    /// </summary>
    [HttpPatch("{ci}/estado")]
    [RequirePermission("admin.usuarios.leer")]
    public async Task<IActionResult> CambiarEstado(string ci, [FromBody] CambiarEstadoUsuarioDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == ci);
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
            UsuarioNombreUsuario = usuario.NombreUsuario
        });

        await _context.SaveChangesAsync();

        return Ok(new { mensaje = $"El estado del usuario ha sido cambiado a: {estadoTexto}." });
    }

    /// <summary>
    /// Actualiza de forma atómica los roles y permisos directos asignados a un usuario.
    /// </summary>
    [HttpPut("{ci}/roles-permisos")]
    [RequirePermission("admin.usuarios.editar")]
    public async Task<IActionResult> ActualizarRolesYPermisos(string ci, [FromBody] AsignarRolesPermisosUsuarioDto dto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.UsuarioRoles)
            .Include(u => u.UsuarioPermisos)
            .FirstOrDefaultAsync(u => u.NombreUsuario == ci);

        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        // 1. Sincronizar Roles
        _context.Set<UsuarioRol>().RemoveRange(usuario.UsuarioRoles);
        if (dto.IdsRoles != null && dto.IdsRoles.Any())
        {
            foreach (var idRol in dto.IdsRoles)
            {
                _context.Set<UsuarioRol>().Add(new UsuarioRol
                {
                        NombreUsuario = ci,
                    IdRol = idRol
                });
            }
        }

        // 2. Sincronizar Permisos Directos (excepciones)
        _context.Set<UsuarioPermiso>().RemoveRange(usuario.UsuarioPermisos);
        if (dto.IdsPermisosDirectos != null && dto.IdsPermisosDirectos.Any())
        {
            foreach (var idPermiso in dto.IdsPermisosDirectos)
            {
                _context.Set<UsuarioPermiso>().Add(new UsuarioPermiso
                {
                        NombreUsuario = ci,
                    IdPermiso = idPermiso
                });
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Roles y permisos del usuario actualizados correctamente." });
    }
}