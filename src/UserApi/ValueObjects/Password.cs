using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using UserApi.Shared;

namespace UserApi.ValueObjects;

[Owned]
public sealed record Password
{
    private const int MinLength = 8;
    private const int MaxLength = 128;
    private const int SaltLength = 16;
    private const int Iterations = 100000;
    private const int OutputLength = 32;

    public byte[] Hash { get; }
    public byte[] Salt { get; }

    private Password(byte[] hash, byte[] salt)
    {
        Hash = hash;
        Salt = salt;
    }

    public static Result<Password> Create(string plainPassword)
    {
        var validation = IsValid(plainPassword);

        if (validation.IsFailure)
            return Result<Password>.Fail(validation.Errors!);

        var salt = new byte[SaltLength];
        RandomNumberGenerator.Fill(salt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, Iterations, HashAlgorithmName.SHA256, OutputLength);

        return Result<Password>.Ok(new Password(hash, salt));
    }

    // Utilizado para validar senha ao criar usuário ou dar update.
    private static Result<string> IsValid(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
            errors.Add("A senha não pode ser vazia.");

        if (!password.Any(char.IsUpper))
            errors.Add("A senha deve conter pelo menos uma letra maiúscula.");

        if (!password.Any(char.IsDigit))
            errors.Add("A senha deve conter pelo menos um número.");

        if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation))
            errors.Add("A senha deve conter pelo menos um caractere especial.");

        if (password.Length is < MinLength or > MaxLength)
            errors.Add($"A senha deve conter entre {MinLength} e {MaxLength} caracteres.");

        return errors.Count > 0 ? Result<string>.Fail(errors) : Result<string>.Ok(password);
    }

    // Utilizado para verificar senha no login ou mudança de senha.
    public bool Verify(string plainPassword)
    {
        var verifyHash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, Salt, Iterations, HashAlgorithmName.SHA256, OutputLength);
        return CryptographicOperations.FixedTimeEquals(verifyHash, Hash);
    }
}