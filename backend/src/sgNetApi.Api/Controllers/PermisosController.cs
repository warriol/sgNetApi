using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sgNetApi.Api.Authorization;
using sgNetApi.Domain.DTOs;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermisosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PermisosController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna el catálogo completo de permisos disponibles en el sistema.
    /// </summary>
    [HttpGet]
    [RequirePermission("roles.administrar")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var permisos = await _context.Permisos
            .AsNoTracking()
            .Select(p => new PermisoDto
            {
                IdPermiso = p.IdPermiso,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion
            })
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return Ok(permisos);
    }
}