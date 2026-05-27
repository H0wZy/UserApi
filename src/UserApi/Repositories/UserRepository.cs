using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Repositories;

public class UserRepository(UserDbContext ctx) : GenericRepository<User>(ctx), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email) =>
        await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username.Value == username);

    public async Task<User?> GetByCpfAsync(string cpf) =>
        await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Cpf.Value == cpf);

    public async Task<bool> GetEmailExistenceAsync(string email) =>
        await Context.Users.AnyAsync(u => u.Email.Value == email);

    public async Task<bool> GetUsernameExistenceAsync(string username) =>
        await Context.Users.AnyAsync(u => u.Username.Value == username);

    public async Task<bool> GetCpfExistenceAsync(string cpf)
        => await Context.Users.AnyAsync(u => u.Cpf.Value == cpf);

    public async Task<bool> GetPhoneNumberExistenceAsync(string phoneNumber) =>
        await Context.Users.AnyAsync(u => u.PhoneNumber != null && u.PhoneNumber.Value == phoneNumber);
}