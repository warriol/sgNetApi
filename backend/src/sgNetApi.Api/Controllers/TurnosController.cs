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
public class TurnosController : ControllerBase
{
    private readonly AppDbContext _context;

    public TurnosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var turnos = await _context.Turnos.AsNoTracking().OrderBy(t => t.Nombre).ToListAsync();
        return Ok(turnos.Select(t => new TurnoDto
        {
            IdTurno = t.IdTurno,
            Nombre = t.Nombre,
            TipoTurno = t.TipoTurno,
            HoraInicio = t.HoraInicio,
            HoraFin = t.HoraFin,
            Descripcion = t.Descripcion,
            Habilitado = t.Habilitado
        }));
    }

    [HttpGet("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var turno = await _context.Turnos.FindAsync(id);
        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        return Ok(new TurnoDto
        {
            IdTurno = turno.IdTurno,
            Nombre = turno.Nombre,
            TipoTurno = turno.TipoTurno,
            HoraInicio = turno.HoraInicio,
            HoraFin = turno.HoraFin,
            Descripcion = turno.Descripcion,
            Habilitado = turno.Habilitado
        });
    }

    [HttpPost]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Crear([FromBody] CrearTurnoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest(new { mensaje = "El nombre del turno es obligatorio." });

        var tipoValido = new[] { "4x6", "3x8", "2x12", "1x24" };
        if (!tipoValido.Contains(dto.TipoTurno))
            return BadRequest(new { mensaje = "El tipo de turno debe ser uno de: 4x6, 3x8, 2x12 o 1x24." });

        if (dto.HoraFin <= dto.HoraInicio)
            return BadRequest(new { mensaje = "La hora de finalización debe ser mayor que la hora de inicio." });

        var turno = new Turno
        {
            Nombre = dto.Nombre.Trim(),
            TipoTurno = dto.TipoTurno,
            HoraInicio = dto.HoraInicio,
            HoraFin = dto.HoraFin,
            Descripcion = dto.Descripcion,
            Habilitado = dto.Habilitado
        };

        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Turno creado correctamente.", id = turno.IdTurno });
    }

    [HttpPut("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] CrearTurnoDto dto)
    {
        var turno = await _context.Turnos.FindAsync(id);
        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest(new { mensaje = "El nombre del turno es obligatorio." });

        if (dto.HoraFin <= dto.HoraInicio)
            return BadRequest(new { mensaje = "La hora de finalización debe ser mayor que la hora de inicio." });

        var tipoValido = new[] { "4x6", "3x8", "2x12", "1x24" };
        if (!tipoValido.Contains(dto.TipoTurno))
            return BadRequest(new { mensaje = "El tipo de turno debe ser uno de: 4x6, 3x8, 2x12 o 1x24." });

        turno.Nombre = dto.Nombre.Trim();
        turno.TipoTurno = dto.TipoTurno;
        turno.HoraInicio = dto.HoraInicio;
        turno.HoraFin = dto.HoraFin;
        turno.Descripcion = dto.Descripcion;
        turno.Habilitado = dto.Habilitado;

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Turno actualizado correctamente." });
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var turno = await _context.Turnos.Include(t => t.Dependencias).FirstOrDefaultAsync(t => t.IdTurno == id);
        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        if (turno.Dependencias.Any())
        {
            turno.Habilitado = false;
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "El turno fue deshabilitado porque está siendo usado por dependencias." });
        }

        _context.Turnos.Remove(turno);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
