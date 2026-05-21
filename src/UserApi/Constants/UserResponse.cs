namespace UserApi.Constants;

public static class UserResponse
{
        public const string CreationSuccess = "Usuário criado com sucesso!";
        public const string CreationFailed = "Não foi possível criar o usuário.";

        public const string SearchFailed = "Não foi possível buscar o usuário.";
        public const string NotFound = "Usuário não encontrado.";

        public const string UpdateSuccess = "Usuário atualizado com sucesso!";
        public const string UpdateFailed = "Não foi possível atualizar o usuário.";

        public const string DeleteSuccess = "Usuário deletado com sucesso!";
        public const string DeleteFailed = "Não foi possível deletar o usuário.";
        public const string AuthFailed = "Não foi possível autenticar o usuário.";
        public const string InvalidCredentials = "Credenciais inválidas.";
}