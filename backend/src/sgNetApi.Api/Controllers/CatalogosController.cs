using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sgNetApi.Api.Authorization;
using sgNetApi.Domain.Entities;
using sgNetApi.Infrastructure.Data;

namespace sgNetApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class CatalogosController : ControllerBase
{
    private readonly AppDbContext _context;

    public CatalogosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(new
        {
            nacionalidades = await _context.Nacionalidades.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(),
            estadosCiviles = await _context.EstadosCiviles.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(),
            profesiones = await _context.Profesiones.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(),
            grados = await _context.Grados.AsNoTracking().OrderBy(x => x.Numero).ToListAsync(),
            escalafones = await _context.Escalafones.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(),
            unidadesEjecutoras = await _context.UnidadesEjecutoras.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(),
            dependencias = await _context.Dependencias.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync()
        });
    }

    [HttpPost("{tipo}")]
    [RequirePermission("admin.catalogos.gestion")]
    public async Task<IActionResult> Crear(string tipo, [FromBody] CatalogoCrudDto dto)
    {
        var entidad = CrearEntidad(tipo, dto);
        if (entidad == null) return BadRequest(new { mensaje = "Tipo de catálogo no válido." });
        _context.Add(entidad);
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Catálogo creado correctamente." });
    }

    [HttpPut("{tipo}/{id:int}")]
    [RequirePermission("admin.catalogos.gestion")]
    public async Task<IActionResult> Actualizar(string tipo, int id, [FromBody] CatalogoCrudDto dto)
    {
        var entidad = await BuscarEntidad(tipo, id);
        if (entidad == null) return NotFound(new { mensaje = "Registro no encontrado." });
        if (!ActualizarEntidad(tipo, entidad, dto)) return BadRequest(new { mensaje = "Tipo de catálogo no válido." });
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Catálogo actualizado correctamente." });
    }

    [HttpDelete("{tipo}/{id:int}")]
    [RequirePermission("admin.catalogos.gestion")]
    public async Task<IActionResult> Eliminar(string tipo, int id)
    {
        var entidad = await BuscarEntidad(tipo, id);
        if (entidad == null) return NotFound(new { mensaje = "Registro no encontrado." });
        _context.Remove(entidad);
        try { await _context.SaveChangesAsync(); return NoContent(); }
        catch (DbUpdateException) { return Conflict(new { mensaje = "No se puede eliminar porque existen registros relacionados." }); }
    }

    private object? CrearEntidad(string tipo, CatalogoCrudDto dto) => Normalizar(tipo) switch
    {
        "nacionalidades" => new Nacionalidad { Nombre = dto.Nombre, CodigoIso = dto.CodigoIso ?? string.Empty, EsUruguaya = dto.EsUruguaya },
        "estadosciviles" => new EstadoCivil { Nombre = dto.Nombre },
        "profesiones" => new Profesion { Nombre = dto.Nombre },
        "grados" => new Grado { Numero = dto.Numero ?? 0, Texto = dto.Texto ?? string.Empty, Abreviatura = dto.Abreviatura ?? string.Empty },
        "escalafones" => new Escalafon { Nombre = dto.Nombre, Abreviatura = dto.Abreviatura ?? string.Empty },
        "unidadesejecutoras" => new UnidadEjecutora { Nombre = dto.Nombre, Siglas = dto.Siglas ?? string.Empty },
        "dependencias" => new Dependencia { IdUuee = dto.IdUuee ?? 0, Nombre = dto.Nombre, Siglas = dto.Siglas ?? string.Empty },
        _ => null
    };

    private async Task<object?> BuscarEntidad(string tipo, int id) => Normalizar(tipo) switch
    {
        "nacionalidades" => await _context.Nacionalidades.FindAsync(id), "estadosciviles" => await _context.EstadosCiviles.FindAsync(id),
        "profesiones" => await _context.Profesiones.FindAsync(id), "grados" => await _context.Grados.FindAsync(id),
        "escalafones" => await _context.Escalafones.FindAsync(id), "unidadesejecutoras" => await _context.UnidadesEjecutoras.FindAsync(id),
        "dependencias" => await _context.Dependencias.FindAsync(id), _ => null
    };

    private static bool ActualizarEntidad(string tipo, object entidad, CatalogoCrudDto dto) => Normalizar(tipo) switch
    {
        "nacionalidades" when entidad is Nacionalidad x => ActualizarNacionalidad(x, dto),
        "estadosciviles" when entidad is EstadoCivil x => ActualizarNombre(x, dto.Nombre), "profesiones" when entidad is Profesion x => ActualizarNombre(x, dto.Nombre),
        "grados" when entidad is Grado x => ActualizarGrado(x, dto), "escalafones" when entidad is Escalafon x => ActualizarEscalafon(x, dto),
        "unidadesejecutoras" when entidad is UnidadEjecutora x => ActualizarUuee(x, dto), "dependencias" when entidad is Dependencia x => ActualizarDependencia(x, dto), _ => false
    };

    private static bool ActualizarNombre(object entidad, string nombre) { if (entidad is EstadoCivil e) e.Nombre = nombre; else if (entidad is Profesion p) p.Nombre = nombre; else return false; return true; }
    private static bool ActualizarNacionalidad(Nacionalidad x, CatalogoCrudDto d) { x.Nombre = d.Nombre; x.CodigoIso = d.CodigoIso ?? string.Empty; x.EsUruguaya = d.EsUruguaya; return true; }
    private static bool ActualizarGrado(Grado x, CatalogoCrudDto d) { x.Numero = d.Numero ?? 0; x.Texto = d.Texto ?? string.Empty; x.Abreviatura = d.Abreviatura ?? string.Empty; return true; }
    private static bool ActualizarEscalafon(Escalafon x, CatalogoCrudDto d) { x.Nombre = d.Nombre; x.Abreviatura = d.Abreviatura ?? string.Empty; return true; }
    private static bool ActualizarUuee(UnidadEjecutora x, CatalogoCrudDto d) { x.Nombre = d.Nombre; x.Siglas = d.Siglas ?? string.Empty; return true; }
    private static bool ActualizarDependencia(Dependencia x, CatalogoCrudDto d) { x.IdUuee = d.IdUuee ?? x.IdUuee; x.Nombre = d.Nombre; x.Siglas = d.Siglas ?? string.Empty; return true; }
    private static string Normalizar(string tipo) => tipo.Replace("_", string.Empty).Replace("-", string.Empty).ToLowerInvariant();
}

public class CatalogoCrudDto
{
    public string Nombre { get; set; } = string.Empty; public string? CodigoIso { get; set; } public bool EsUruguaya { get; set; }
    public int? Numero { get; set; } public string? Texto { get; set; } public string? Abreviatura { get; set; }
    public int? IdUuee { get; set; } public string? Siglas { get; set; }
}
