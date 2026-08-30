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
public class DireccionesController : ControllerBase
{
    private readonly AppDbContext _context;

    public DireccionesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var direcciones = await _context.Direcciones.AsNoTracking().OrderBy(d => d.Pais).ToListAsync();
        return Ok(direcciones.Select(d => new DireccionDto
        {
            IdDireccion = d.IdDireccion,
            TipoDireccion = d.TipoDireccion,
            Pais = d.Pais,
            Departamento = d.Departamento,
            Localidad = d.Localidad,
            Calle = d.Calle,
            Cruce1 = d.Cruce1,
            Cruce2 = d.Cruce2,
            Numero = d.Numero,
            Apartamento = d.Apartamento,
            Manzana = d.Manzana,
            Solar = d.Solar,
            Ruta = d.Ruta,
            Km = d.Km,
            Latitud = d.Latitud,
            Longitud = d.Longitud,
            CodigoPostal = d.CodigoPostal,
            Telefono = d.Telefono
        }));
    }

    [HttpGet("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var direccion = await _context.Direcciones.FindAsync(id);
        if (direccion == null)
            return NotFound(new { mensaje = "Dirección no encontrada." });

        return Ok(Mapear(direccion));
    }

    [HttpPost]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Crear([FromBody] CrearDireccionDto dto)
    {
        var validacion = ValidarDireccion(dto);
        if (!string.IsNullOrWhiteSpace(validacion))
            return BadRequest(new { mensaje = validacion });

        var direccion = new Direccion
        {
            TipoDireccion = dto.TipoDireccion,
            Pais = dto.Pais.Trim(),
            Departamento = dto.Departamento.Trim(),
            Localidad = dto.Localidad.Trim(),
            Calle = dto.Calle,
            Cruce1 = dto.Cruce1,
            Cruce2 = dto.Cruce2,
            Numero = dto.Numero,
            Apartamento = dto.Apartamento,
            Manzana = dto.Manzana,
            Solar = dto.Solar,
            Ruta = dto.Ruta,
            Km = dto.Km,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            CodigoPostal = dto.CodigoPostal,
            Telefono = dto.Telefono
        };

        _context.Direcciones.Add(direccion);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Dirección creada correctamente.", id = direccion.IdDireccion });
    }

    [HttpPut("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] CrearDireccionDto dto)
    {
        var direccion = await _context.Direcciones.FindAsync(id);
        if (direccion == null)
            return NotFound(new { mensaje = "Dirección no encontrada." });

        var validacion = ValidarDireccion(dto);
        if (!string.IsNullOrWhiteSpace(validacion))
            return BadRequest(new { mensaje = validacion });

        direccion.TipoDireccion = dto.TipoDireccion;
        direccion.Pais = dto.Pais.Trim();
        direccion.Departamento = dto.Departamento.Trim();
        direccion.Localidad = dto.Localidad.Trim();
        direccion.Calle = dto.Calle;
        direccion.Cruce1 = dto.Cruce1;
        direccion.Cruce2 = dto.Cruce2;
        direccion.Numero = dto.Numero;
        direccion.Apartamento = dto.Apartamento;
        direccion.Manzana = dto.Manzana;
        direccion.Solar = dto.Solar;
        direccion.Ruta = dto.Ruta;
        direccion.Km = dto.Km;
        direccion.Latitud = dto.Latitud;
        direccion.Longitud = dto.Longitud;
        direccion.CodigoPostal = dto.CodigoPostal;
        direccion.Telefono = dto.Telefono;

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Dirección actualizada correctamente." });
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("admin.dependencias.gestion")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var direccion = await _context.Direcciones.FirstOrDefaultAsync(d => d.IdDireccion == id);
        if (direccion == null)
            return NotFound(new { mensaje = "Dirección no encontrada." });

        var dependenciaVinculada = await _context.Dependencias.AnyAsync(d => d.IdDireccion == id);
        if (dependenciaVinculada)
            return Conflict(new { mensaje = "La dirección está asociada a una dependencia y no puede eliminarse." });

        _context.Direcciones.Remove(direccion);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static string? ValidarDireccion(CrearDireccionDto dto)
    {
        if (dto.TipoDireccion < 1 || dto.TipoDireccion > 5)
            return "Debe seleccionar un tipo de dirección válido.";

        if (dto.TipoDireccion == 1 && (!dto.Latitud.HasValue || !dto.Longitud.HasValue))
            return "Complete los campos obligatorios para el tipo de dirección seleccionado.";

        if (dto.TipoDireccion == 2 && (string.IsNullOrWhiteSpace(dto.Ruta) || !dto.Km.HasValue))
            return "Complete los campos obligatorios para el tipo de dirección seleccionado.";

        if (dto.TipoDireccion == 3 && (string.IsNullOrWhiteSpace(dto.Calle) || string.IsNullOrWhiteSpace(dto.Numero)))
            return "Complete los campos obligatorios para el tipo de dirección seleccionado.";

        if (dto.TipoDireccion == 4 && (string.IsNullOrWhiteSpace(dto.Calle) || string.IsNullOrWhiteSpace(dto.Cruce1) || string.IsNullOrWhiteSpace(dto.Cruce2)))
            return "Complete los campos obligatorios para el tipo de dirección seleccionado.";

        if (dto.TipoDireccion == 5 && (string.IsNullOrWhiteSpace(dto.Calle) || string.IsNullOrWhiteSpace(dto.Manzana) || string.IsNullOrWhiteSpace(dto.Solar)))
            return "Complete los campos obligatorios para el tipo de dirección seleccionado.";

        return null;
    }

    private static DireccionDto Mapear(Direccion direccion) => new()
    {
        IdDireccion = direccion.IdDireccion,
        TipoDireccion = direccion.TipoDireccion,
        Pais = direccion.Pais,
        Departamento = direccion.Departamento,
        Localidad = direccion.Localidad,
        Calle = direccion.Calle,
        Cruce1 = direccion.Cruce1,
        Cruce2 = direccion.Cruce2,
        Numero = direccion.Numero,
        Apartamento = direccion.Apartamento,
        Manzana = direccion.Manzana,
        Solar = direccion.Solar,
        Ruta = direccion.Ruta,
        Km = direccion.Km,
        Latitud = direccion.Latitud,
        Longitud = direccion.Longitud,
        CodigoPostal = direccion.CodigoPostal,
        Telefono = direccion.Telefono
    };
}
