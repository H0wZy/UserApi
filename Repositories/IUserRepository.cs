using user_api.cs.Models;

namespace user_api.cs.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> GetEmailExistenceAsync(string email);
    Task<bool> GetUsernameExistenceAsync(string username);
    Task<bool> GetCpfExistenceAsync(string cpf);
}