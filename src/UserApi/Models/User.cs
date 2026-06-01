using System.ComponentModel.DataAnnotations.Schema;
using UserApi.ValueObjects;

namespace UserApi.Models;

[Table("users")]
public class User : AccountEntity
{
    public required Username Username { get; set; }
    public required Email Email { get; set; }
    public required Name Name { get; set; }
    [NotMapped] public string FullName => Name.FullName;
    public PhoneNumber? PhoneNumber { get; set; }
    public required Cpf Cpf { get; init; }
    // TODO: Criar VO Cnpj
    // [Column("cnpj")] public Cnpj? Cnpj { get; init; }
    public required Password Password { get; set; }
    [Column("birth_date")] public required DateOnly BirthDate { get; init; }
}