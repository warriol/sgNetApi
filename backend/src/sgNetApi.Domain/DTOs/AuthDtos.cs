namespace sgNetApi.Domain.DTOs;

public class LoginRequestDto
{
    public long Ci { get; set; }
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
    public long Ci { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Permisos { get; set; } = new();
}