using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_api.cs.Models;

[Table("user")]
public class User : AccountEntity
{
    [Column("username"), MaxLength(255)] public string Username { get; set; } = string.Empty;
    [Column("email"), MaxLength(255)] public string Email { get; set; } = string.Empty;
    [Column("first_name"), MaxLength(255)] public string FirstName { get; set; } = string.Empty;
    [Column("last_name"), MaxLength(255)] public string LastName { get; set; } = string.Empty;
    [NotMapped] public string FullName => $"{FirstName} {LastName}";
    [Column("hash_password")] public byte[] HashPassword { get; set; } = [];
    [Column("salt_password")] public byte[] SaltPassword { get; set; } = [];
    [Column("birth_date")] public required DateOnly BirthDate { get; set; }
    [Column("accepted_terms")] public bool AcceptedTerms { get; set; }
    [Column("accepted_terms_date")] public DateTime? AcceptedTermsDate { get; set; }
}