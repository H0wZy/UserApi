namespace user_api.cs.Dto;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string FullName,
    DateOnly BirthDate,
    bool IsDisabled,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? LastLoginAt,
    DateTime? LastLogoutAt
);