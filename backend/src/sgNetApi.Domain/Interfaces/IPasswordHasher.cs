namespace sgNetApi.Domain.Interfaces;

public interface IPasswordHasher
{
    /// <summary>
    /// Genera un Salt aleatorio de 64 bytes y calcula el Hash HMACSHA512 de la contraseña.
    /// </summary>
    void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

    /// <summary>
    /// Verifica si una contraseña en texto plano coincide con el Hash y Salt almacenados en la base de datos.
    /// </summary>
    bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}