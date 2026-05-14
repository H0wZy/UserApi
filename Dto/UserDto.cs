namespace user_api.cs.Dto;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string FullName,
    DateOnly BirthDate,
    string Cpf,
    string UserType,
    bool IsDisabled,
    bool AcceptedTerms,
    DateTime? AcceptedTermsAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? LastLoginAt,
    DateTime? LastLogoutAt
);