namespace user_api.cs.Dto;

public record UpdateUserDto
(
    string? Username,
    string? Email,
    string? FirstName,
    string? LastName
);