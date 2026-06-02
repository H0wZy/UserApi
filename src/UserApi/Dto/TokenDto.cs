namespace UserApi.Dto;

public record TokenDto(
    string AccessToken,
    DateTime ExpiresAt,
    string? LastLoginMethod,
    string TokenType = "Bearer"); // TODO FIXME: TokenType é necessário?