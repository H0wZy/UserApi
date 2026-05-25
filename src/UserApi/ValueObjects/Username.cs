using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using UserApi.Shared;

namespace UserApi.ValueObjects;

[Owned]
public sealed partial record Username
{
    private const int MinLength = 3;
    private const int MaxLength = 20;

    private static readonly HashSet<string> ReservedWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "administrador",
        "admin",
        "root",
        "superuser",
        "support",
        "api",
        "me",
        "user",
        "users",
        "null",
    };

    public string Value { get; }

    private Username(string value) => Value = value;

    public static Result<Username> Create(string? plainUsername)
    {
        var normalizedUsername = plainUsername?.Trim().ToLowerInvariant() ?? string.Empty;
        var validation = IsValid(normalizedUsername);
        return validation.IsSuccess
            ? Result<Username>.Ok(new Username(normalizedUsername))
            : Result<Username>.Fail(validation.Errors!);
    }

    private static Result<string> IsValid(string username)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(username))
            errors.Add("Nome de usuário não pode ser vazio.");

        if (ReservedWords.Contains(username))
            errors.Add("Nome de usuário contém palavras proíbidas.");

        if (username.Length is < MinLength or > MaxLength)
            errors.Add($"Nome de usuário deve ter entre {MinLength} e {MaxLength} caracteres.");

        var validRegex = AllowedChars.IsMatch(username);
        if (!validRegex)
            errors.Add("Nome de usuário devem ter apenas letras e números.");

        return errors.Count > 0
            ? Result<string>.Fail(errors)
            : Result<string>.Ok(username);
    }

    [GeneratedRegex(@"^[A-Za-z0-9]+$")] private static partial Regex AllowedChars { get; }
}