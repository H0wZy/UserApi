using UserApi.Dto;
using UserApi.Models;

namespace UserApi.Services;

public interface ITokenService
{
    TokenDto GenerateToken(User user, string lastLoginMethod);
}