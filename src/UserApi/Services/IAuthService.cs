using UserApi.Dto;
using UserApi.Shared;

namespace UserApi.Services;

public interface IAuthService
{
    Task<GenericResponse<TokenDto>> LoginAsync(LoginDto dto);
    Task<GenericResponse<bool>> LogoutAsync(Guid userId);
}
