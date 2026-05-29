using UserApi.Dto;
using UserApi.Shared;

namespace UserApi.Services;

public interface IUserService : IGenericService<UserDto, CreateUserDto, UpdateUserDto>
{
    Task<GenericResponse<UserDto>> GetUserByEmailAsync(string email);
    Task<GenericResponse<UserDto>> GetUserByUsernameAsync(string username);
    Task<GenericResponse<UserDto>> GetUserByCpfAsync(string cpf);
    Task<GenericResponse<bool>> UpdateUserPasswordAsync(Guid id, UpdatePasswordDto dto);
}
