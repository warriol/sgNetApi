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
public class DependenciasController : ControllerBase
{
    private readonly AppDbContext _context;

    public DependenciasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var dependencias = await _context.Dependencias
            .AsNoTracking()
            .Include(d => d.UnidadEjecutora)
            .Include(d => d.Direccion)
            .Include(d => d.Turno)
            .Include(d => d.Funcionarios)
            .Select(d => new DependenciaDto
            {
                IdDependencia = d.IdDependencia,
                IdUuee = d.IdUuee,
                NombreUuee = d.UnidadEjecutora.Nombre,
                Nombre = d.Nombre,
                Siglas = d.Siglas,
                IdDireccion = d.IdDireccion,
                NombreUsuarioJefe = d.NombreUsuarioJefe,
                NombreUsuarioSegundoJefe = d.NombreUsuarioSegundoJefe,
                IdTurno = d.IdTurno,
                NombreTurno = d.Turno != null ? d.Turno.Nombre : null,
                FuncionariosAsignados = d.Funcionarios.Count,
                Completada = d.Nombre != string.Empty && d.Siglas != string.Empty && d.IdUuee > 0
                    && d.Funcionarios.Count >= 2
                    && !string.IsNullOrWhiteSpace(d.NombreUsuarioJefe)
                    && !string.IsNullOrWhiteSpace(d.NombreUsuarioSegundoJefe)
                    && d.Direccion != null
                    && d.Turno != null && d.Turno.Habilitado
            })
            .ToListAsync();

        return Ok(dependencias);
    }

    [HttpGet("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var dependencia = await _context.Dependencias
            .Include(d => d.UnidadEjecutora)
            .Include(d => d.Direccion)
            .Include(d => d.Turno)
            .Include(d => d.Funcionarios)
            .FirstOrDefaultAsync(d => d.IdDependencia == id);

        if (dependencia == null)
            return NotFound(new { mensaje = "Dependencia no encontrada." });

        return Ok(new DependenciaDto
        {
            IdDependencia = dependencia.IdDependencia,
            IdUuee = dependencia.IdUuee,
            NombreUuee = dependencia.UnidadEjecutora.Nombre,
            Nombre = dependencia.Nombre,
            Siglas = dependencia.Siglas,
            IdDireccion = dependencia.IdDireccion,
            NombreUsuarioJefe = dependencia.NombreUsuarioJefe,
            NombreUsuarioSegundoJefe = dependencia.NombreUsuarioSegundoJefe,
            IdTurno = dependencia.IdTurno,
            NombreTurno = dependencia.Turno?.Nombre,
            FuncionariosAsignados = dependencia.Funcionarios.Count,
            Completada = dependencia.Nombre != string.Empty && dependencia.Siglas != string.Empty && dependencia.IdUuee > 0
                && dependencia.Funcionarios.Count >= 2
                && !string.IsNullOrWhiteSpace(dependencia.NombreUsuarioJefe)
                && !string.IsNullOrWhiteSpace(dependencia.NombreUsuarioSegundoJefe)
                && dependencia.Direccion != null
                && dependencia.Turno != null && dependencia.Turno.Habilitado
        });
    }

    [HttpPost]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Crear([FromBody] CrearDependenciaDto dto)
    {
        if (!dto.IdUuee.HasValue)
            return BadRequest(new { mensaje = "Debe seleccionar una Unidad Ejecutora." });

        var unidad = await _context.UnidadesEjecutoras.FindAsync(dto.IdUuee.Value);
        if (unidad == null)
            return BadRequest(new { mensaje = "La Unidad Ejecutora seleccionada no existe." });

        if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Siglas))
            return BadRequest(new { mensaje = "El nombre y las siglas de la dependencia son obligatorios." });

        var dependencia = new Dependencia
        {
            IdUuee = dto.IdUuee.Value,
            Nombre = dto.Nombre.Trim(),
            Siglas = dto.Siglas.Trim(),
            IdDireccion = dto.IdDireccion,
            NombreUsuarioJefe = dto.NombreUsuarioJefe,
            NombreUsuarioSegundoJefe = dto.NombreUsuarioSegundoJefe,
            IdTurno = dto.IdTurno
        };

        _context.Dependencias.Add(dependencia);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Dependencia creada correctamente.", id = dependencia.IdDependencia });
    }

    [HttpPut("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] CrearDependenciaDto dto)
    {
        var dependencia = await _context.Dependencias.FirstOrDefaultAsync(d => d.IdDependencia == id);
        if (dependencia == null)
            return NotFound(new { mensaje = "Dependencia no encontrada." });

        if (!dto.IdUuee.HasValue)
            return BadRequest(new { mensaje = "Debe seleccionar una Unidad Ejecutora." });

        var unidad = await _context.UnidadesEjecutoras.FindAsync(dto.IdUuee.Value);
        if (unidad == null)
            return BadRequest(new { mensaje = "La Unidad Ejecutora seleccionada no existe." });

        dependencia.IdUuee = dto.IdUuee.Value;
        dependencia.Nombre = dto.Nombre.Trim();
        dependencia.Siglas = dto.Siglas.Trim();
        dependencia.IdDireccion = dto.IdDireccion;
        dependencia.NombreUsuarioJefe = dto.NombreUsuarioJefe;
        dependencia.NombreUsuarioSegundoJefe = dto.NombreUsuarioSegundoJefe;
        dependencia.IdTurno = dto.IdTurno;

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Dependencia actualizada correctamente." });
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var dependencia = await _context.Dependencias.Include(d => d.Funcionarios).FirstOrDefaultAsync(d => d.IdDependencia == id);
        if (dependencia == null)
            return NotFound(new { mensaje = "Dependencia no encontrada." });

        if (dependencia.Funcionarios.Any() || dependencia.IdDireccion.HasValue)
            return Conflict(new { mensaje = "No se puede eliminar físicamente una dependencia con usuarios o dirección asociada." });

        _context.Dependencias.Remove(dependencia);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{idDependencia:int}/funcionarios")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ReemplazarFuncionarios(int idDependencia, [FromBody] ReemplazarFuncionariosDependenciaDto dto)
    {
        var dependencia = await _context.Dependencias
            .Include(d => d.Funcionarios)
            .FirstOrDefaultAsync(d => d.IdDependencia == idDependencia);

        if (dependencia == null)
            return NotFound(new { mensaje = "Dependencia no encontrada." });

        foreach (var nombreUsuario in dto.NombresUsuario.Distinct())
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
            if (usuario == null)
                return BadRequest(new { mensaje = $"El usuario {nombreUsuario} no existe." });

            if (usuario.IdDependencia.HasValue && usuario.IdDependencia.Value != idDependencia)
                return Conflict(new { mensaje = "El usuario ya pertenece a otra dependencia." });
        }

        foreach (var usuario in dependencia.Funcionarios.ToList())
        {
            usuario.IdDependencia = null;
        }

        foreach (var nombreUsuario in dto.NombresUsuario.Distinct())
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
            if (usuario != null)
            {
                usuario.IdDependencia = idDependencia;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Funcionarios asignados correctamente." });
    }

    [HttpPut("{idDependencia:int}/jefaturas")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> AsignarJefaturas(int idDependencia, [FromBody] JefaturasDependenciaDto dto)
    {
        var dependencia = await _context.Dependencias
            .Include(d => d.Funcionarios)
            .FirstOrDefaultAsync(d => d.IdDependencia == idDependencia);

        if (dependencia == null)
            return NotFound(new { mensaje = "Dependencia no encontrada." });

        if (string.IsNullOrWhiteSpace(dto.NombreUsuarioJefe) || string.IsNullOrWhiteSpace(dto.NombreUsuarioSegundoJefe))
            return BadRequest(new { mensaje = "Los jefes de la dependencia son obligatorios." });

        if (dto.NombreUsuarioJefe == dto.NombreUsuarioSegundoJefe)
            return BadRequest(new { mensaje = "El jefe y el segundo jefe deben ser diferentes." });

        var jefes = new[] { dto.NombreUsuarioJefe, dto.NombreUsuarioSegundoJefe };
        foreach (var nombreUsuario in jefes)
        {
            var pertenece = dependencia.Funcionarios.Any(u => u.NombreUsuario == nombreUsuario);
            if (!pertenece)
                return BadRequest(new { mensaje = "El jefe seleccionado debe pertenecer a la dependencia." });
        }

        dependencia.NombreUsuarioJefe = dto.NombreUsuarioJefe;
        dependencia.NombreUsuarioSegundoJefe = dto.NombreUsuarioSegundoJefe;
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Jefaturas asignadas correctamente." });
    }

    [HttpPut("{idDependencia:int}/turno")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> AsignarTurno(int idDependencia, [FromBody] TurnoDependenciaDto dto)
    {
        var dependencia = await _context.Dependencias.FirstOrDefaultAsync(d => d.IdDependencia == idDependencia);
        if (dependencia == null)
            return NotFound(new { mensaje = "Dependencia no encontrada." });

        var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.IdTurno == dto.IdTurno && t.Habilitado);
        if (turno == null)
            return BadRequest(new { mensaje = "Debe crear y habilitar al menos un turno antes de asignarlo." });

        dependencia.IdTurno = turno.IdTurno;
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Turno asignado correctamente." });
    }

    [HttpGet("{idDependencia:int}/completitud")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ObtenerCompletitud(int idDependencia)
    {
        var dependencia = await _context.Dependencias
            .Include(d => d.Funcionarios)
            .Include(d => d.Direccion)
            .Include(d => d.Turno)
            .FirstOrDefaultAsync(d => d.IdDependencia == idDependencia);

        if (dependencia == null)
            return NotFound(new { mensaje = "Dependencia no encontrada." });

        var dto = new DependenciaCompletitudDto
        {
            TieneNombreYSiglas = !string.IsNullOrWhiteSpace(dependencia.Nombre) && !string.IsNullOrWhiteSpace(dependencia.Siglas),
            TieneUnidadEjecutora = dependencia.IdUuee > 0,
            TieneAlMenosDosFuncionarios = dependencia.Funcionarios.Count >= 2,
            JefesValidos = !string.IsNullOrWhiteSpace(dependencia.NombreUsuarioJefe)
                && !string.IsNullOrWhiteSpace(dependencia.NombreUsuarioSegundoJefe)
                && dependencia.NombreUsuarioJefe != dependencia.NombreUsuarioSegundoJefe
                && dependencia.Funcionarios.Any(u => u.NombreUsuario == dependencia.NombreUsuarioJefe)
                && dependencia.Funcionarios.Any(u => u.NombreUsuario == dependencia.NombreUsuarioSegundoJefe),
            TieneDireccion = dependencia.IdDireccion.HasValue && dependencia.Direccion != null,
            TieneTurnoHabilitado = dependencia.IdTurno.HasValue && dependencia.Turno != null && dependencia.Turno.Habilitado,
        };

        dto.Completada = dto.TieneNombreYSiglas && dto.TieneUnidadEjecutora && dto.TieneAlMenosDosFuncionarios && dto.JefesValidos
            && dto.TieneDireccion && dto.TieneTurnoHabilitado;

        if (!dto.TieneNombreYSiglas) dto.Pendientes.Add("nombre y siglas");
        if (!dto.TieneUnidadEjecutora) dto.Pendientes.Add("Unidad Ejecutora");
        if (!dto.TieneAlMenosDosFuncionarios) dto.Pendientes.Add("al menos dos funcionarios");
        if (!dto.JefesValidos) dto.Pendientes.Add("jefaturas válidas");
        if (!dto.TieneDireccion) dto.Pendientes.Add("dirección");
        if (!dto.TieneTurnoHabilitado) dto.Pendientes.Add("turno habilitado");

        return Ok(dto);
    }
}
