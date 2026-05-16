using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using user_api.cs.Enum;
using user_api.cs.ValueObjects;

namespace user_api.cs.Models;

[Table("users")]
public class User : AccountEntity
{
    [Column("username"), MaxLength(255)] public string Username { get; set; } = string.Empty;
    [Column("email"), MaxLength(255)] public string Email { get; set; } = string.Empty;
    [Column("first_name"), MaxLength(255)] public string FirstName { get; set; } = string.Empty;
    [Column("last_name"), MaxLength(255)] public string LastName { get; set; } = string.Empty;
    [NotMapped] public string FullName => $"{FirstName} {LastName}";
    [Column("user_type")] public UserType UserType { get; private set; }
    [Column("cpf")] public required Cpf Cpf { get; set; }
    // TODO: CNPJ do Usuário
    // [Column("cnpj")] public Cnpj? Cnpj { get; private set; }
    public required Password Password { get; set; }
    [Column("birth_date")] public required DateOnly BirthDate { get; set; }
    [Column("accepted_terms")] public bool AcceptedTerms { get; set; }
    [Column("accepted_terms_at")] public DateTime? AcceptedTermsAt { get; set; }

    public User() { }

    public User(UserType userType, Cpf cpf)
    {
        UserType = userType;
        Cpf = cpf;
    }
}