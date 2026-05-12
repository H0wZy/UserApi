using System.ComponentModel.DataAnnotations.Schema;

namespace user_api.cs.Models;

[Table("user")]
public class User : BaseEntity
{
    [Column("username")] public string Username { get; set; } = string.Empty;
    [Column("email")] public string Email { get; set; } = string.Empty;
    [Column("first_name")] public string FirstName { get; set; } = string.Empty;
    [Column("last_name")] public string LastName { get; set; } = string.Empty;
    [Column("full_name")] public string FullName => $"{FirstName} {LastName}";
    [Column("hash_password")] public byte[] HashPassword { get; set; } = [];
    [Column("salt_password")] public byte[] SaltPassword { get; set; } = [];
    [Column("last_login_at")] public DateTime? LastLoginAt { get; set; } = null;
    [Column("user_disabled")] public bool UserDisabled { get; set; } = false;
    [Column("accepted_terms")] public bool AcceptedTerms { get; set; }
    [Column("accepted_terms_date")] public DateTime? AcceptedTermsDate { get; set; } = null;
}
