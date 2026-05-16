using Microsoft.EntityFrameworkCore;
using user_api.cs.Shared;

namespace user_api.cs.ValueObjects;

[Owned]
public sealed record Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string? plainEmail)
    {
        var normalized = plainEmail?.Trim().ToLowerInvariant() ?? string.Empty;
        var errors = Validate(normalized);
        return errors.Count > 0
            ? Result<Email>.Fail(errors)
            : Result<Email>.Ok(new Email(normalized));
    }

    private static List<string> Validate(string email)
    {
        var errors = new List<string>();
        var atIndex = email.IndexOf('@');
        var domain = email[(atIndex + 1)..];

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("O email não pode ser vazio.");
            return errors;
        }

        if (email.AsSpan().ContainsAny('\r', '\n', ' '))
            errors.Add("O email não pode conter espaços em branco ou quebra de linhas.");

        if (atIndex <= 0 && atIndex != email.Length - 1 && atIndex != email.LastIndexOf('@'))
        {
            errors.Add("O email deve conter um '@' separando o nome de usuário e o domínio.");
            return errors;
        }

        if (!domain.Contains('.'))
            errors.Add("O domínio do email deve conter pelo menos um ponto separador.");

        if (domain.StartsWith('.') || domain.EndsWith('.'))
            errors.Add("O domínio do email não pode começar ou terminar com um ponto.");

        return errors;
    }
}