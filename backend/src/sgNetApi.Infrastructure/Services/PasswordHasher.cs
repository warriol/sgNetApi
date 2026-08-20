using System.Security.Cryptography;
using System.Text;
using sgNetApi.Domain.Interfaces;

namespace sgNetApi.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        // Se utiliza HMACSHA512 para generar una clave criptográfica con un Salt aleatorio
        using var hmac = new HMACSHA512();
        
        // La propiedad Key de HMACSHA512 se utiliza como el Salt individual por usuario
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        // Se inicializa el HMACSHA512 usando el mismo Salt almacenado del usuario
        using var hmac = new HMACSHA512(passwordSalt);
        
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        // Comparación segura byte a byte para prevenir ataques de temporización (Timing Attacks)
        return CryptographicOperations.FixedTimeEquals(computedHash, passwordHash);
    }
}