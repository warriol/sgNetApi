namespace sgNetApi.Domain.DTOs;

public class CambiarPasswordDto
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordActual { get; set; } = string.Empty;
    public string PasswordNueva { get; set; } = string.Empty;
}

public class ResetearPasswordAdminDto
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordNueva { get; set; } = string.Empty;
}