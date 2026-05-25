using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using UserApi.Shared;

namespace UserApi.ValueObjects;

[Owned]
public sealed partial record Name
{
    private const string NameField = "Nome";
    private const string SurnameField = "Sobrenome";
    private const int MinLength = 3;
    private const int MaxLength = 50;

    public string FirstName { get; } = string.Empty;
    public string LastName { get; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";

    private Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<Name> Create(string? firstName, string? lsatName)
    {
        var normalizedFirstName = firstName?.Trim() ?? string.Empty;
        var normalizedLastName = lsatName?.Trim() ?? string.Empty;

        var errors = new List<string>();

        ValidadePart(normalizedFirstName, NameField, errors);
        ValidadePart(normalizedLastName, SurnameField, errors);

        return errors.Count > 0
            ? Result<Name>.Fail(errors)
            : Result<Name>.Ok(new Name(normalizedFirstName, normalizedLastName));
    }

    private static void ValidadePart(string value, string field, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add($"{field} não pode ser vázio.");
            return;
        }

        if (value.Length is < MinLength or > MaxLength)
            errors.Add($"{field} deve conter entre {MinLength} e {MaxLength} caracteres.");

        var validRegex = AllowedChars.IsMatch(value);

        if (!validRegex)
            errors.Add($"{field} deve conter apenas letras maiúsculas, minusculas e com acentos.");
    }

    public override string ToString() => FullName;

    [GeneratedRegex(@"^[a-zA-ZÀ-ÿ\s]+$")] private static partial Regex AllowedChars { get; }
}