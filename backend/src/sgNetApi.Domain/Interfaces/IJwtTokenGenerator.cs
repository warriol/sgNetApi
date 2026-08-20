using sgNetApi.Domain.Entities;

namespace sgNetApi.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerarToken(Usuario usuario, List<string> roles, List<string> permisos);
}