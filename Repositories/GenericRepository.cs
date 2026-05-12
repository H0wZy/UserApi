using Microsoft.EntityFrameworkCore;
using user_api.cs.Models;

namespace user_api.cs.Repositories;

public class GenericRepository<T>(UserDbContext ctx) : IGenericRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet = ctx.Set<T>();

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await ctx.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var rows = await _dbSet.Where(u => u.Id == id).ExecuteDeleteAsync();
        return rows > 0;
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await ctx.SaveChangesAsync();
        return entity;
    }
}
