using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserApi.Enum;
using UserApi.Models;
using UserApi.Utils;

namespace UserApi.Data;

public class UserDbContext(DbContextOptions<UserDbContext> opt) : DbContext(opt)
{
    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (ShouldUpdateAudit(entry))
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var accTypeConverter = new ValueConverter<AccType, string>(
            v => v.ToDescription(),
            v => ParseAccType(v));

        var roleConverter = new ValueConverter<Role, string>(
            v => v.ToDescription(),
            v => ParseRole(v));

        modelBuilder.Entity<User>(entity =>
        {
            entity.OwnsOne(u => u.Cpf, cpf =>
            {
                cpf.Property(c => c.Value)
                    .HasColumnName("cpf")
                    .HasMaxLength(11)
                    .IsRequired();
            });
            entity.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.Hash)
                    .HasColumnName("hash_password")
                    .HasColumnType("bytea")
                    .IsRequired();
                password.Property(p => p.Salt)
                    .HasColumnName("salt_password")
                    .HasColumnType("bytea")
                    .IsRequired();
            });
            entity.OwnsOne(u => u.Username, username =>
            {
                username.Property(u => u.Value)
                    .HasColumnName("username")
                    .HasMaxLength(20)
                    .IsRequired();
            });
            entity.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("email")
                    .HasMaxLength(64)
                    .IsRequired();
            });
            entity.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsRequired();
                name.Property(n => n.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsRequired();
            });
            entity.OwnsOne(u => u.PhoneNumber, phoneNumber =>
            {
                phoneNumber.Property(p => p.Value)
                    .HasColumnName("phone_number")
                    .HasMaxLength(13);
            });
            entity.Property(u => u.Type)
                .HasConversion(accTypeConverter)
                .HasColumnName("account_type")
                .HasMaxLength(2)
                .IsRequired();
            
            entity.Property(u => u.Role)
                .HasConversion(roleConverter)
                .HasColumnName("role")
                .HasMaxLength(30)
                .IsRequired();
        });
    }

    private static bool ShouldUpdateAudit(EntityEntry<BaseEntity> entry)
    {
        var hasChangedOwnedEntities = HasChangedOwnedEntities(entry);
        var hasRelevantChanges = HasRelevantChanges(entry);
        return hasRelevantChanges || (entry.State is EntityState.Unchanged && hasChangedOwnedEntities);
    }

    private static bool HasRelevantChanges(EntityEntry entry)
    {
        return entry.Properties.Any(p => p.IsModified && !IgnoredAuditProperties.Contains(p.Metadata.Name));
    }

    private static readonly HashSet<string> IgnoredAuditProperties =
    [
        nameof(AccountEntity.LastLoginAt),
        nameof(AccountEntity.LastLogoutAt),
        nameof(BaseEntity.UpdatedAt)
    ];

    private static bool HasChangedOwnedEntities(EntityEntry entry)
    {
        return entry.References.Any(r => r.TargetEntry is
        {
            State:
            EntityState.Modified
            or EntityState.Added
            or EntityState.Deleted
        });
    }

    private static AccType ParseAccType(string value)
    {
        var map = new Dictionary<string, AccType>
        {
            ["PF"] = AccType.Individual,
            ["PJ"] = AccType.Company
        };

        return map.TryGetValue(value, out var type)
            ? type
            : throw new InvalidOperationException($"Valor '{value}' não é válido para AccType.");
    }

    private static Role ParseRole(string value)
    {
        var map = new Dictionary<string, Role>
        {
            ["Administrador"] = Role.Admin,
            ["Usuário comum"] = Role.CommonUser
        };

        return map.TryGetValue(value, out var type)
            ? type
            : throw new InvalidOperationException($"Valor '{value}' não é válido para Role.");
    }
}