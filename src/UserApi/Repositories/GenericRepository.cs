using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Repositories;

public class GenericRepository<T>(UserDbContext ctx) : IGenericRepository<T> where T : AccountEntity
{
    protected readonly UserDbContext Context = ctx;
    private readonly DbSet<T> _dbSet = ctx.Set<T>();

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return false;

        _dbSet.Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<T> UpdateAsync(T entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }
}
