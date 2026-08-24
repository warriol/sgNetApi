using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sgNetApi.Api.Authorization;
using sgNetApi.Domain.DTOs;
using sgNetApi.Domain.Entities;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todos los roles registrados junto con sus permisos asociados.
    /// </summary>
    [HttpGet]
    [RequirePermission("roles.administrar")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var roles = await _context.Roles
            .AsNoTracking()
            .Include(r => r.RolPermisos)
                .ThenInclude(rp => rp.Permiso)
            .Select(r => new RolDetalleDto
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                Permisos = r.RolPermisos.Select(rp => new PermisoDto
                {
                    IdPermiso = rp.Permiso.IdPermiso,
                    Nombre = rp.Permiso.Nombre,
                    Descripcion = rp.Permiso.Descripcion
                }).ToList()
            })
            .ToListAsync();

        return Ok(roles);
    }

    /// <summary>
    /// Crea un nuevo Rol asignándole un conjunto de permisos.
    /// </summary>
    [HttpPost]
    [RequirePermission("roles.administrar")]
    public async Task<IActionResult> Crear([FromBody] CrearRolDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.Nombre.Length > 50)
            return BadRequest(new { mensaje = "El nombre del rol es obligatorio y no puede superar 50 caracteres." });

        var permisos = await _context.Permisos
            .Where(p => dto.IdsPermisos.Contains(p.IdPermiso))
            .Select(p => p.IdPermiso)
            .ToListAsync();
        if (permisos.Count != dto.IdsPermisos.Distinct().Count())
            return BadRequest(new { mensaje = "Uno o más permisos seleccionados no existen." });

        if (await _context.Roles.AnyAsync(r => r.Nombre.ToLower() == dto.Nombre.ToLower()))
        {
            return BadRequest(new { mensaje = "Ya existe un rol con ese nombre." });
        }

        var nuevoRol = new Rol { Nombre = dto.Nombre.Trim(), Descripcion = dto.Descripcion?.Trim() };
        _context.Roles.Add(nuevoRol);
        await _context.SaveChangesAsync();

        if (dto.IdsPermisos != null && dto.IdsPermisos.Any())
        {
            foreach (var idPermiso in dto.IdsPermisos)
            {
                _context.Set<RolPermiso>().Add(new RolPermiso
                {
                    IdRol = nuevoRol.IdRol,
                    IdPermiso = idPermiso
                });
            }
            await _context.SaveChangesAsync();
        }

        return CreatedAtAction(nameof(ObtenerTodos), new { id = nuevoRol.IdRol }, new { mensaje = "Rol creado con éxito.", idRol = nuevoRol.IdRol });
    }

    /// <summary>
    /// Actualiza el nombre de un Rol y sincroniza la lista de sus permisos.
    /// </summary>
    [HttpPut("{idRol}")]
    [RequirePermission("roles.administrar")]
    public async Task<IActionResult> Actualizar(int idRol, [FromBody] EditarRolDto dto)
    {
        var rol = await _context.Roles
            .Include(r => r.RolPermisos)
            .FirstOrDefaultAsync(r => r.IdRol == idRol);

        if (rol == null) return NotFound(new { mensaje = "Rol no encontrado." });

        if (string.IsNullOrWhiteSpace(dto.Nombre) || dto.Nombre.Length > 50)
            return BadRequest(new { mensaje = "El nombre del rol es obligatorio y no puede superar 50 caracteres." });

        var permisos = await _context.Permisos
            .Where(p => dto.IdsPermisos.Contains(p.IdPermiso))
            .Select(p => p.IdPermiso)
            .ToListAsync();
        if (permisos.Count != dto.IdsPermisos.Distinct().Count())
            return BadRequest(new { mensaje = "Uno o más permisos seleccionados no existen." });

        rol.Nombre = dto.Nombre.Trim();
        rol.Descripcion = dto.Descripcion?.Trim();

        // Limpiar permisos anteriores y asignar la nueva selección
        _context.Set<RolPermiso>().RemoveRange(rol.RolPermisos);

        if (dto.IdsPermisos != null && dto.IdsPermisos.Any())
        {
            foreach (var idPermiso in dto.IdsPermisos)
            {
                _context.Set<RolPermiso>().Add(new RolPermiso
                {
                    IdRol = rol.IdRol,
                    IdPermiso = idPermiso
                });
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Rol y permisos actualizados correctamente." });
    }
}