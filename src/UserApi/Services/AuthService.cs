using UserApi.Constants;
using UserApi.Dto;
using UserApi.Repositories;
using UserApi.Shared;

namespace UserApi.Services;

public class AuthService(IUserRepository repository, ITokenService service)
    : IAuthService
{
    // TODO FIXME: Melhorias de robustez que você pode adicionar depois
    // Para deixar o login realmente mais seguro, considere implementar:
    // Rate limiting por IP e por usuário.
    // Bloqueio temporário após muitas tentativas inválidas.
    // Refresh token.
    // Revogação de tokens no logout.
    // Auditoria de login.
    // Registro de IP/User-Agent.
    // 2FA.
    // Validação mais forte de e-mail.
    // Uso de TimeProvider em vez de DateTime.UtcNow para facilitar testes.
    // Comparação de hash em tempo constante no Password.Verify.
    public async Task<GenericResponse<TokenDto>> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
            return GenericResponse<TokenDto>.BadRequest("Dados inválidos.", ["Login e senha são obrigatórios."]);

        var login = dto.Login.Trim();
        var lastLoginMethod = IsEmail(login) ? "email" : "username";

        var user = lastLoginMethod == "email"
            ? await repository.GetByEmailAsync(login)
            : await repository.GetByUsernameAsync(login);

        if (user is null)
            return GenericResponse<TokenDto>.BadRequest(UserResponse.AuthFailed, [UserResponse.InvalidCredentials]);

        if (user.IsOnline)
            return GenericResponse<TokenDto>.BadRequest(UserResponse.AuthFailed, [UserResponse.AlreadyLoggedIn]);

        if (user.IsDisabled)
            return GenericResponse<TokenDto>.BadRequest(UserResponse.AuthFailed, [UserResponse.Disabled]);

        var passwordIsValid = user.Password.Verify(dto.Password);

        if (!passwordIsValid)
            return GenericResponse<TokenDto>.BadRequest(UserResponse.AuthFailed, [UserResponse.InvalidCredentials]);

        user.LastLoginAt = DateTime.UtcNow;
        user.LastLoginMethod = lastLoginMethod;
        user.IsOnline = true;

        await repository.UpdateAsync(user);

        var token = service.GenerateToken(user, lastLoginMethod);

        return GenericResponse<TokenDto>.Ok(token, "Login realizado com sucesso.");
    }

    public async Task<GenericResponse<bool>> LogoutAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);

        if (user is null)
            return GenericResponse<bool>.NotFound(UserResponse.NotFound);

        if (!user.IsOnline)
            return GenericResponse<bool>.BadRequest(UserResponse.AlreadyLoggedOut);

        user.IsOnline = false;
        user.LastLogoutAt = DateTime.UtcNow;

        await repository.UpdateAsync(user);
        return GenericResponse<bool>.Ok(true, "Logout realizado com sucesso.");
    }

    private static bool IsEmail(string login) => login.Contains('@', StringComparison.Ordinal);
}