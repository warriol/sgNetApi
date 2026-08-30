namespace sgNetApi.Domain.DTOs;

public class DireccionDto
{
    public int IdDireccion { get; set; }
    public int TipoDireccion { get; set; }
    public string Pais { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Localidad { get; set; } = string.Empty;
    public string? Calle { get; set; }
    public string? Cruce1 { get; set; }
    public string? Cruce2 { get; set; }
    public string? Numero { get; set; }
    public string? Apartamento { get; set; }
    public string? Manzana { get; set; }
    public string? Solar { get; set; }
    public string? Ruta { get; set; }
    public decimal? Km { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Telefono { get; set; }
}

public class CrearDireccionDto
{
    public int TipoDireccion { get; set; }
    public string Pais { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Localidad { get; set; } = string.Empty;
    public string? Calle { get; set; }
    public string? Cruce1 { get; set; }
    public string? Cruce2 { get; set; }
    public string? Numero { get; set; }
    public string? Apartamento { get; set; }
    public string? Manzana { get; set; }
    public string? Solar { get; set; }
    public string? Ruta { get; set; }
    public decimal? Km { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Telefono { get; set; }
}
