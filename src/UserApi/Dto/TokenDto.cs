namespace UserApi.Dto;

public record TokenDto(
    string AccessToken,
    DateTime ExpiresAt,
    string? LoginMethod,
    string TokenType = "Bearer"); // TODO FIXME: TokenType é necessário?