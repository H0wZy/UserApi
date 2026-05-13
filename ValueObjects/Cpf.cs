using System.Text.RegularExpressions;
using user_api.cs.Shared;

namespace user_api.cs.ValueObjects;

public sealed partial record Cpf
{
    public string Value { get; } = string.Empty;

    public static Result<Cpf> Create(string value)
    {
        value = RemoveFormatting(value);
        return !IsValid(value) ? Result<Cpf>.Fail("Cpf inválido!") : Result<Cpf>.Ok(new Cpf(value));
    }

    private Cpf(string value)
    {
        Value = value;
    }

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex NonDigitsRegex();
    private static string RemoveFormatting(string value) => NonDigitsRegex().Replace(value, "");

    private static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = RemoveFormatting(cpf);

        if (cpf.Length is not 11) return false;
        if (cpf.All(c => c == cpf[0])) return false;

        var numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        // Primeiro dígito
        var sum = 0;
        for (var i = 0; i < 9; i++) sum += numbers[i] * (10 - i);
        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;
        if (numbers[9] != firstDigit) return false;

        // Segundo dígito
        sum = 0;
        for (var i = 0; i < 10; i++) sum += numbers[i] * (11 - i);
        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;
        return numbers[10] == secondDigit;
    }

    public override string ToString()
    {
        return ConvertToFormatted(Value);
    }

    private static string ConvertToFormatted(string cpf)
    {
        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }
}