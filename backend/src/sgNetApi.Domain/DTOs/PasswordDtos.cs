namespace sgNetApi.Domain.DTOs;

public class CambiarPasswordDto
{
    public long Ci { get; set; }
    public string PasswordActual { get; set; } = string.Empty;
    public string PasswordNueva { get; set; } = string.Empty;
}

public class ResetearPasswordAdminDto
{
    public long CiUsuario { get; set; }
    public string PasswordNueva { get; set; } = string.Empty;
}