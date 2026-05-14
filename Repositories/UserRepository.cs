using Microsoft.EntityFrameworkCore;
using user_api.cs.Data;
using user_api.cs.Models;

namespace user_api.cs.Repositories;

public class UserRepository(UserDbContext ctx) : GenericRepository<User>(ctx), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email) => await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    public async Task<User?> GetByUsernameAsync(string username) => await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    public async Task<bool> GetEmailExistenceAsync(string email) => await Context.Users.AnyAsync(u => u.Email == email);
    public async Task<bool> GetUsernameExistenceAsync(string username) => await Context.Users.AnyAsync(u => u.Username == username);
    public async Task<bool> GetCpfExistenceAsync(string cpf) => await Context.Users.AnyAsync(u => u.Cpf.Value == cpf);
}
