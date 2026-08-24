namespace sgNetApi.Domain.Interfaces;

public interface IPasswordHasher
{
    /// <summary>
    /// Genera un Salt aleatorio de 64 bytes y calcula el Hash HMACSHA512 de la contraseña.
    /// </summary>
    string CrearPasswordHash(string password);

    /// <summary>
    /// Verifica si una contraseña en texto plano coincide con el Hash y Salt almacenados en la base de datos.
    /// </summary>
    bool VerificarPasswordHash(string password, string passwordHash);
}