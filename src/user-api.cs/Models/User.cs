using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using user_api.cs.Enum;
using user_api.cs.ValueObjects;

namespace user_api.cs.Models;

[Table("users")]
public class User : AccountEntity
{
    [Column("username"), MaxLength(255)] public string Username { get; set; } = string.Empty;
    public required Email Email { get; set; }
    [Column("first_name"), MaxLength(255)] public string FirstName { get; set; } = string.Empty;
    [Column("last_name"), MaxLength(255)] public string LastName { get; set; } = string.Empty;
    [NotMapped] public string FullName => $"{FirstName} {LastName}";
    // TODO: Criar VO PhoneNumber
    // [Column("phone_number")] public PhoneNumber? PhoneNumber { get; set; }
    [Column("user_type")] public UserType UserType { get; init; }
    public required Cpf Cpf { get; init; }
    // TODO: Criar VO Cnpj
    // [Column("cnpj")] public Cnpj? Cnpj { get; init; }
    public required Password Password { get; set; }
    [Column("birth_date")] public required DateOnly BirthDate { get; init; }
}