using user_api.cs.Dto;
using user_api.cs.Models;

namespace user_api.cs.Services;

public interface ITokenService
{
    TokenDto GenerateToken(User user, string loginMethod);
}