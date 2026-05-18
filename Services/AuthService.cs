using user_api.cs.Constants;
using user_api.cs.Dto;
using user_api.cs.Repositories;
using user_api.cs.Shared;

namespace user_api.cs.Services;

public class AuthService(IUserRepository userRepository, IUserService userService, ITokenService tokenService) : IAuthService
{
    public async Task<GenericResponse<TokenDto>> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
            return GenericResponse<TokenDto>.BadRequest("Dados inválidos.", ["Login e senha são obrigatórios."]);

        var login = dto.Login.Trim();
        var loginMethod = IsEmail(login) ? "email" : "username";

        var user = loginMethod == "email"
            ? await userRepository.GetByEmailAsync(login) : await userRepository.GetByUsernameAsync(login);

        if (user is null)
            return GenericResponse<TokenDto>.BadRequest(UserResponse.InvalidCredentials);

        if (user.IsDisabled)
            return GenericResponse<TokenDto>.BadRequest("Usuário desabilitado.");

        var passwordIsValid = user.Password.Verify(dto.Password);

        if (!passwordIsValid)
            return GenericResponse<TokenDto>.BadRequest(UserResponse.InvalidCredentials);

        // TODO FIXME: LoginMethod não está sendo transportado para o banco, devo mapear no UserProfile?
        user.LoginMethod = loginMethod;

        await userService.UpdateUserLastLoginAsync(user.Id);

        var token = tokenService.GenerateToken(user, loginMethod);

        return GenericResponse<TokenDto>.Ok(token, "Login realizado com sucesso.");

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
    }

    // TODO: Implementar lógica de logout verificando se User.IsOnline == true
    public Task<GenericResponse<bool>> LogoutAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    private static bool IsEmail(string login) => login.Contains('@', StringComparison.Ordinal);
}