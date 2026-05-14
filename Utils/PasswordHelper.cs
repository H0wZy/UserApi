using System.Security.Cryptography;

namespace user_api.cs.Utils;

public static class PasswordHelper
{
    // Utilizado para criptografar senha ao criar usuário ou dar update.
    public static (byte[] hash, byte[] salt) HashPassword(string password)
    {
        var salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);
        return (hash, salt);
    }

    // Utilizado para verificar senha no login ou mudança de senha.
    public static bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        var verifyHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);
        return verifyHash.SequenceEqual(hash);
    }
}