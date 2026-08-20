using sgNetApi.Domain.DTOs;

namespace sgNetApi.Domain.Interfaces;

public interface IPasswordService
{
    /// <summary>
    /// Cambia la contraseña de un usuario autenticado verificando su clave actual y el historial de las últimas 5 claves.
    /// </summary>
    Task<(bool Exito, string Mensaje)> CambiarPasswordAsync(CambiarPasswordDto dto);

    /// <summary>
    /// Permite a un Administrador resetear la contraseña de un usuario (validando también que no repita las últimas 5).
    /// </summary>
    Task<(bool Exito, string Mensaje)> ResetearPasswordPorAdminAsync(ResetearPasswordAdminDto dto, string adminCi);
}