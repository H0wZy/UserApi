using System.ComponentModel.DataAnnotations;
using UserApi.Enum;

namespace UserApi.Dto;

public record CreateUserDto(
    string Username,
    string Email,
    [MinLength(3), MaxLength(255)] string FirstName,
    [MinLength(3), MaxLength(255)] string LastName,
    string Password,
    string Cpf,
    [Required] DateOnly BirthDate,
    bool AcceptedTerms,
    [EnumDataType(typeof(UserType))] UserType UserType
);
