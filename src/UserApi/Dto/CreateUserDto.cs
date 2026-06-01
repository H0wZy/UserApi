using System.ComponentModel.DataAnnotations;
using UserApi.Enum;

namespace UserApi.Dto;

public record CreateUserDto(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string Password,
    string Cpf,
    [EnumDataType(typeof(UserType))] UserType UserType,
    [EnumDataType(typeof(Role))] Role Role,
    [Required] DateOnly BirthDate,
    bool AcceptedTerms
);