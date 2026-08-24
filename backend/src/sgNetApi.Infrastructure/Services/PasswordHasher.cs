using BCrypt.Net;
using sgNetApi.Domain.Interfaces;

namespace sgNetApi.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public string CrearPasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    public bool VerificarPasswordHash(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}