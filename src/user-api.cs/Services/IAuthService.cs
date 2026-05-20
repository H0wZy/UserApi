using user_api.cs.Dto;
using user_api.cs.Shared;

namespace user_api.cs.Services;

public interface IAuthService
{
    Task<GenericResponse<TokenDto>> LoginAsync(LoginDto dto);
    Task<GenericResponse<bool>> LogoutAsync(Guid userId);
}
