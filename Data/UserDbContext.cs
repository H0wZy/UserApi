using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using user_api.cs.Enum;
using user_api.cs.Models;
using user_api.cs.Utils;

namespace user_api.cs.Data;

public class UserDbContext(DbContextOptions<UserDbContext> opt) : DbContext(opt)
{
    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State is EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        var converter = new ValueConverter<UserType, string>(
            v => v.ToDescription(),
            v => ParseUserType(v));

        mb.Entity<User>(entity =>
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
            entity.OwnsOne(u => u.Email, email =>
            {
               email.Property(e => e.Value)
                   .HasColumnName("email")
                   .HasMaxLength(255)
                   .IsRequired();
            });
            entity.Property(u => u.UserType)
                .HasConversion(converter)
                .HasMaxLength(2)
                .IsRequired();
        });
    }

    private static UserType ParseUserType(string value)
    {
        var map = new Dictionary<string, UserType>
        {
            ["PF"] = UserType.Individual,
            ["PJ"] = UserType.Company,
        };

        return map.TryGetValue(value, out var type) ? type : throw new InvalidOperationException($"Valor '{value}' não é válido para UserType.");
    }
}
