namespace sgNetApi.Domain.Entities;

public class Nacionalidad
{
    public int IdNacionalidad { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CodigoIso { get; set; } = string.Empty;
    public bool EsUruguaya { get; set; }
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}

public class EstadoCivil
{
    public int IdEstadoCivil { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class Profesion
{
    public int IdProfesion { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class Grado
{
    public int IdGrado { get; set; }
    public int Numero { get; set; }
    public string Texto { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class Escalafon
{
    public int IdEscalafon { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class UnidadEjecutora
{
    public int IdUuee { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
}

public class Dependencia
{
    public int IdDependencia { get; set; }
    
    public int IdUuee { get; set; }
    public UnidadEjecutora UnidadEjecutora { get; set; } = null!;
    public string Nombre { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
}