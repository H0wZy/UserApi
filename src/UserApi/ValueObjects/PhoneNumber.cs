using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using UserApi.Shared;

namespace UserApi.ValueObjects;

[Owned]
public sealed partial record PhoneNumber
{
    private const int MinDigits = 10;
    private const int MaxDigits = 13;

    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static Result<PhoneNumber> Create(string? plainPhoneNumber)
    {
        if (string.IsNullOrWhiteSpace(plainPhoneNumber))
            return Result<PhoneNumber>.Fail("Número de telefone não pode ser vazio.");

        var normalizedPhoneNumber = RemoveFormatting(plainPhoneNumber);

        var validation = IsValid(normalizedPhoneNumber);

        return validation.IsSuccess
            ? Result<PhoneNumber>.Ok(new PhoneNumber(normalizedPhoneNumber))
            : Result<PhoneNumber>.Fail(validation.Errors!);
    }

    private static Result<string> IsValid(string phoneNumber)
    {
        var errors = new List<string>();

        if (phoneNumber.Length is < MinDigits or > MaxDigits)
            errors.Add($"Número de telefone deve ter entre {MinDigits} a {MaxDigits} números.");

        return errors.Count > 0
            ? Result<string>.Fail(errors)
            : Result<string>.Ok(phoneNumber);
    }

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex NonDigitsRegex();

    private static string RemoveFormatting(string value) => NonDigitsRegex().Replace(value, "");

    public override string ToString() => Value;
}