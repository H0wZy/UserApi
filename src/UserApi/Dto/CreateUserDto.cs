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
    [EnumDataType(typeof(AccType))] AccType Type,
    [Required] DateOnly BirthDate,
    bool AcceptedTerms
);