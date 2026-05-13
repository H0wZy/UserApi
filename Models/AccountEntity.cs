using System.ComponentModel.DataAnnotations.Schema;

namespace user_api.cs.Models;

public abstract class AccountEntity : BaseEntity
{
    [Column("is_disabled")] public bool IsDisabled { get; set; }
    [Column("last_login_at")] public DateTime? LastLoginAt { get; set; }
    [Column("last_logout_at")] public DateTime? LastLogoutAt { get; set; }
}