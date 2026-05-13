using Microsoft.EntityFrameworkCore;
using user_api.cs.Models;

namespace user_api.cs.Data;

public class UserDbContext(DbContextOptions<UserDbContext> opt) : DbContext(opt)
{
    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = entry.State switch
            {
                EntityState.Added or EntityState.Modified => DateTime.UtcNow,
                _ => entry.Entity.UpdatedAt
            };

            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>(entity =>
        {
            entity.OwnsOne(u => u.Cpf, cpf =>
            {
                cpf.Property(c => c.Value)
                .HasColumnName("cpf")
                .HasMaxLength(11)
                .IsRequired();
            });
        });

        base.OnModelCreating(mb);
    }
}
