using System.ComponentModel.DataAnnotations.Schema;
using UserApi.Enum;

namespace UserApi.Models;

public abstract class AccountEntity : BaseEntity
{
    public required AccType Type { get; init; }
    public required Role Role { get; init; }
    [Column("accepted_terms")] public required bool AcceptedTerms { get; init; }
    [Column("accepted_terms_at")] public required DateTime AcceptedTermsAt { get; init; }
    [Column("is_disabled")] public bool IsDisabled { get; set; }
    [Column("disabled_at")] public DateTime? DisabledAt { get; set; }
    [Column("is_online")] public bool IsOnline { get; set; }
    [Column("last_login_method")] public string? LastLoginMethod { get; set; }
    [Column("last_login_at")] public DateTime? LastLoginAt { get; set; }
    [Column("last_logout_at")] public DateTime? LastLogoutAt { get; set; }
}