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
public class FuncionarioEquipoPolicialController : ControllerBase
{
    private readonly AppDbContext _context;

    public FuncionarioEquipoPolicialController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [RequirePermission("admin.inventario.gestion")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var asignaciones = await _context.FuncionariosEquipoPolicial
            .AsNoTracking()
            .Select(a => new FuncionarioEquipoPolicialDto
            {
                IdFuncionarioEquipoPolicial = a.IdFuncionarioEquipoPolicial,
                NombreUsuario = a.NombreUsuario,
                IdArmaPolicial = a.IdArmaPolicial,
                IdChalecoAntibalaPolicial = a.IdChalecoAntibalaPolicial,
                IdEsposasPolicial = a.IdEsposasPolicial,
                FechaAsignacion = a.FechaAsignacion,
                FechaDevolucion = a.FechaDevolucion,
                Observaciones = a.Observaciones
            })
            .ToListAsync();

        return Ok(asignaciones);
    }

    [HttpGet("{id:int}")]
    [RequirePermission("admin.inventario.gestion")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var asignacion = await _context.FuncionariosEquipoPolicial.FindAsync(id);
        if (asignacion == null)
            return NotFound(new { mensaje = "Asignación no encontrada." });

        return Ok(new FuncionarioEquipoPolicialDto
        {
            IdFuncionarioEquipoPolicial = asignacion.IdFuncionarioEquipoPolicial,
            NombreUsuario = asignacion.NombreUsuario,
            IdArmaPolicial = asignacion.IdArmaPolicial,
            IdChalecoAntibalaPolicial = asignacion.IdChalecoAntibalaPolicial,
            IdEsposasPolicial = asignacion.IdEsposasPolicial,
            FechaAsignacion = asignacion.FechaAsignacion,
            FechaDevolucion = asignacion.FechaDevolucion,
            Observaciones = asignacion.Observaciones
        });
    }

    [HttpGet("Funcionarios/{nombreUsuario}/equipo-policial")]
    [RequirePermission("admin.inventario.gestion")]
    public async Task<IActionResult> ObtenerEquipoPorFuncionario(string nombreUsuario)
    {
        var asignacion = await _context.FuncionariosEquipoPolicial
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.NombreUsuario == nombreUsuario && a.FechaDevolucion == null);

        if (asignacion == null)
            return NotFound(new { mensaje = "No existe equipo asignado activo para este funcionario." });

        return Ok(new FuncionarioEquipoPolicialDto
        {
            IdFuncionarioEquipoPolicial = asignacion.IdFuncionarioEquipoPolicial,
            NombreUsuario = asignacion.NombreUsuario,
            IdArmaPolicial = asignacion.IdArmaPolicial,
            IdChalecoAntibalaPolicial = asignacion.IdChalecoAntibalaPolicial,
            IdEsposasPolicial = asignacion.IdEsposasPolicial,
            FechaAsignacion = asignacion.FechaAsignacion,
            FechaDevolucion = asignacion.FechaDevolucion,
            Observaciones = asignacion.Observaciones
        });
    }

    [HttpPost]
    [RequirePermission("admin.inventario.gestion")]
    public async Task<IActionResult> Crear([FromBody] CrearFuncionarioEquipoPolicialDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NombreUsuario))
            return BadRequest(new { mensaje = "Debe indicar el usuario." });

        var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.NombreUsuario == dto.NombreUsuario);
        if (!usuarioExiste)
            return BadRequest(new { mensaje = "El usuario no existe." });

        var yaTieneAsignacionActiva = await _context.FuncionariosEquipoPolicial
            .AnyAsync(a => a.NombreUsuario == dto.NombreUsuario && a.FechaDevolucion == null);

        if (yaTieneAsignacionActiva)
            return Conflict(new { mensaje = "El funcionario ya tiene una asignación activa." });

        var armaExiste = await _context.ArmasPoliciales.AnyAsync(a => a.IdArmaPolicial == dto.IdArmaPolicial);
        var chalecoExiste = await _context.ChalecosAntibalaPoliciales.AnyAsync(c => c.IdChalecoAntibalaPolicial == dto.IdChalecoAntibalaPolicial);
        var esposasExisten = await _context.EsposasPoliciales.AnyAsync(e => e.IdEsposasPolicial == dto.IdEsposasPolicial);

        if (!armaExiste || !chalecoExiste || !esposasExisten)
            return BadRequest(new { mensaje = "Debe seleccionar elementos válidos del inventario." });

        var asignacion = new FuncionarioEquipoPolicial
        {
            NombreUsuario = dto.NombreUsuario,
            IdArmaPolicial = dto.IdArmaPolicial,
            IdChalecoAntibalaPolicial = dto.IdChalecoAntibalaPolicial,
            IdEsposasPolicial = dto.IdEsposasPolicial,
            FechaAsignacion = DateTime.UtcNow,
            Observaciones = dto.Observaciones
        };

        _context.FuncionariosEquipoPolicial.Add(asignacion);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Asignación creada correctamente.", id = asignacion.IdFuncionarioEquipoPolicial });
    }

    [HttpPut("{id:int}")]
    [RequirePermission("admin.inventario.gestion")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] CrearFuncionarioEquipoPolicialDto dto)
    {
        var asignacion = await _context.FuncionariosEquipoPolicial.FindAsync(id);
        if (asignacion == null)
            return NotFound(new { mensaje = "Asignación no encontrada." });

        asignacion.NombreUsuario = dto.NombreUsuario;
        asignacion.IdArmaPolicial = dto.IdArmaPolicial;
        asignacion.IdChalecoAntibalaPolicial = dto.IdChalecoAntibalaPolicial;
        asignacion.IdEsposasPolicial = dto.IdEsposasPolicial;
        asignacion.Observaciones = dto.Observaciones;

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Asignación actualizada correctamente." });
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("admin.inventario.gestion")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var asignacion = await _context.FuncionariosEquipoPolicial.FindAsync(id);
        if (asignacion == null)
            return NotFound(new { mensaje = "Asignación no encontrada." });

        _context.FuncionariosEquipoPolicial.Remove(asignacion);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/devolucion")]
    [RequirePermission("admin.inventario.gestion")]
    public async Task<IActionResult> RegistrarDevolucion(int id, [FromBody] DevolucionEquipoDto dto)
    {
        var asignacion = await _context.FuncionariosEquipoPolicial.FindAsync(id);
        if (asignacion == null)
            return NotFound(new { mensaje = "Asignación no encontrada." });

        asignacion.FechaDevolucion = dto.FechaDevolucion ?? DateTime.UtcNow;
        asignacion.Observaciones = dto.Observaciones ?? asignacion.Observaciones;

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Devolución registrada correctamente." });
    }
}
