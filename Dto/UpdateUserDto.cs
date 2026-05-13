using System.ComponentModel.DataAnnotations;

namespace user_api.cs.Dto;

public record UpdateUserDto
(
    [MinLength(3), MaxLength(50)] string? Username,
    [EmailAddress] string? Email,
    [MinLength(3), MaxLength(255)] string? FirstName,
    [MinLength(3), MaxLength(255)] string? LastName
);