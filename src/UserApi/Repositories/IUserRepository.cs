using UserApi.Models;

namespace UserApi.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByCpfAsync(string cpf);
    Task<bool> GetEmailExistenceAsync(string email);
    Task<bool> GetUsernameExistenceAsync(string username);
    Task<bool> GetCpfExistenceAsync(string cpf);
    Task<bool> GetPhoneNumberExistenceAsync(string phoneNumber);
}