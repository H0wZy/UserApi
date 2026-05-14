using System.ComponentModel.DataAnnotations;
using user_api.cs.Enum;

namespace user_api.cs.Dto;

public record CreateUserDto(
    [Required, MinLength(3), MaxLength(50)] string Username,
    [Required, EmailAddress] string Email,
    [Required, MinLength(3), MaxLength(255)] string FirstName,
    [Required, MinLength(3), MaxLength(255)] string LastName,
    [Required, MinLength(8)] string Password,
    [Required, RegularExpression(@"^\d{11}$")] string Cpf,
    [Required] DateOnly BirthDate,
    [Required] bool AcceptTerms,
    [Required, EnumDataType(typeof(UserType))] UserType UserType
);