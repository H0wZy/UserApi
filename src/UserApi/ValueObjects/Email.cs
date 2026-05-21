using Microsoft.EntityFrameworkCore;
using UserApi.Shared;

namespace UserApi.ValueObjects;

[Owned]
public sealed record Email
{
    private const int MaxLength = 255;

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string? plainEmail)
    {
        var normalized = plainEmail?.Trim().ToLowerInvariant() ?? string.Empty;
        var validation = IsValid(normalized);
        return validation.IsFailure
            ? Result<Email>.Fail(validation.Errors!)
            : Result<Email>.Ok(new Email(normalized));
    }

    private static Result<string> IsValid(string email)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(email))
            errors.Add("O email não pode ser vazio.");

        if (email.Length > MaxLength)
            errors.Add($"O email não pode ter mais de {MaxLength} caracteres.");

        var atCount = email.Count(c => c == '@');

        switch (atCount)
        {
            case 0:
                errors.Add("O email deve conter um '@' separando o nome de usuário e o domínio.");
                break;
            case > 1:
                errors.Add("O email deve conter apenas um '@'.");
                break;
        }

        var parts = email.Split('@');

        var localPart = parts.Length > 0
            ? parts[0]
            : string.Empty;

        var domainPart = parts.Length > 1
            ? parts[1]
            : string.Empty;

        if (atCount == 1 && string.IsNullOrWhiteSpace(localPart))
            errors.Add("O email deve conter um nome de usuário antes do '@'.");

        if (!string.IsNullOrWhiteSpace(localPart))
        {
            if (localPart.StartsWith('.') || localPart.EndsWith('.'))
                errors.Add("A parte antes do '@' não pode começar ou terminar com ponto.");

            if (localPart.Contains(".."))
                errors.Add("A parte antes do '@' não pode conter pontos consecutivos.");
        }

        if (atCount == 1 && string.IsNullOrWhiteSpace(domainPart))
            errors.Add("O email deve conter um domínio após o '@'.");

        if (!string.IsNullOrWhiteSpace(domainPart))
        {
            if (!domainPart.Contains('.'))
                errors.Add("O domínio do email deve conter pelo menos um ponto separador.");

            if (domainPart.StartsWith('.') || domainPart.EndsWith('.'))
                errors.Add("O domínio do email não pode começar ou terminar com um ponto.");
        }

        return errors.Count > 0
            ? Result<string>.Fail(errors)
            : Result<string>.Ok(email);
    }

    public override string ToString() => Value;
}