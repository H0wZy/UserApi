using Microsoft.EntityFrameworkCore;

namespace user_api.cs;

public class GenericRepository<T>(UserDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly UserDbContext _ctx = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _ctx.SaveChangesAsync();
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
        await _ctx.SaveChangesAsync();
        return entity;
    }
}
