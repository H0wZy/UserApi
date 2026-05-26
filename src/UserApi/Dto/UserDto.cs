namespace UserApi.Dto;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string FullName,
    string? PhoneNumber,
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