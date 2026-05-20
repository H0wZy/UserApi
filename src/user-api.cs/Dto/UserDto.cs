namespace user_api.cs.Dto;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string FullName,
    DateOnly BirthDate,
    string Cpf,
    string UserType,
    bool AcceptedTerms,
    DateTime? AcceptedTermsAt,
    bool IsDisabled,
    DateTime? DisabledAt,
    bool IsOnline,
    string? LoginMethod,
    DateTime? LastLoginAt,
    DateTime? LastLogoutAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);