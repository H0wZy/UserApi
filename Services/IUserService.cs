using AutoMapper;
using user_api.cs.Dto;
using user_api.cs.Models;
using user_api.cs.Repositories;
using user_api.cs.Shared;

namespace user_api.cs.Services;

public interface IUserService : IGenericService<UserDto, CreateUserDto, UpdateUserDto>
{
    Task<GenericResponse<UserDto>> GetUserByEmailAsync(string email);
    Task<GenericResponse<UserDto>> GetUserByUsernameAsync(string username);
    Task<GenericResponse<bool>> UpdateUserPasswordAsync(Guid id, UpdatePasswordDto dto);
    Task<GenericResponse<bool>> UpdateUserLastLoginAsync(Guid id);
}
