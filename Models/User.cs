using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_api.cs.Models;

[Table("user")]
public class User
{
    [Key][Column("id")] private Guid Id { get; set; }
    [Column("username")] private string Username { get; set; }
    [Column("email")] private string Email { get; set; }
    [Column("first_name")] private string FirstName { get; set; }
    [Column("last_name")] private string LastName { get; set; }
    [Column("full_name")] private string FullName => $"{FirstName} {LastName}";
}
