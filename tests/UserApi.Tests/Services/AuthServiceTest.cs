using Moq;
using UserApi.Dto;
using UserApi.Enum;
using UserApi.Models;
using UserApi.Repositories;
using UserApi.Services;
using UserApi.ValueObjects;

namespace UserApi.Tests.Services;

public class AuthServiceTest
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<ITokenService> _tokenMock;
    private readonly AuthService _service;

    public AuthServiceTest()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _tokenMock = new Mock<ITokenService>();
        _service = new AuthService(_repositoryMock.Object, _tokenMock.Object);
    }

    #region LoginAsync Tests

    [Fact(DisplayName = "LoginAsync with null data should fail")]
    public async Task LoginAsync_WithNullData_ShouldFail()
    {
        var result = await _service.LoginAsync(MakeLoginDto(login: null, password: null));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "LoginAsync with empty data should fail")]
    public async Task LoginAsync_WithEmptyData_ShouldFail()
    {
        var result = await _service.LoginAsync(MakeLoginDto(login: "", password: ""));
        Assert.True(result.IsFailure);
    }

    [Theory(DisplayName = "LoginAsync when user dont exists should fail")]
    [InlineData("NonExistentUser")]
    [InlineData("non_existent@email.com")]
    public async Task LoginAsync_When_UserDontExists_ShouldFail(string login)
    {
        _repositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _repositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        var result = await _service.LoginAsync(MakeLoginDto(login: login, password: "AnyPassword123."));
        Assert.True(result.IsFailure);
    }

    [Theory(DisplayName = "LoginAsync when user is disabled should fail")]
    [InlineData("DisabledUser")]
    [InlineData("disabledUser@email.com")]
    public async Task LoginAsync_When_UserIsDisabled_ShouldFail(string login)
    {
        var user = MakeUser(isDisabled: true);
        _repositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        var result = await _service.LoginAsync(MakeLoginDto(login: login, password: "AnyPassword123."));
        Assert.True(result.IsFailure);
    }

    [Theory(DisplayName = "LoginAsync when password is wrong should fail")]
    [InlineData("randomUser")]
    [InlineData("randomUser@email.com")]
    public async Task LoginAsync_When_PasswordIsWrong_ShouldFail(string login)
    {
        var user = MakeUser(password: "CorrectPassword123.");
        _repositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        var result = await _service.LoginAsync(MakeLoginDto(login: login, password: "WrongPassword123."));
        Assert.True(result.IsFailure);
    }

    [Theory(DisplayName = "LoginAsync should success")]
    [InlineData("randomUser")]
    [InlineData("randomUser@email.com")]
    public async Task LoginAsync_ShouldSuccess(string login)
    {
        var user = MakeUser(password: "CorrectPassword123.");
        _repositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _tokenMock.Setup(t => t.GenerateToken(It.IsAny<User>(), It.IsAny<string>())).Returns(MakeTokenDto());
        var result = await _service.LoginAsync(MakeLoginDto(login: login, password: "CorrectPassword123."));
        Assert.NotNull(result.Data);
        Assert.True(result.IsSuccess);
    }

    #endregion

    #region LogoutAsync Tests

    // TODO: terminar testes de LogoutAsync
    [Fact(DisplayName = "LogoutAsync should success")]
    public async Task LogoutAsync_ShouldSuccess()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
    }

    #endregion

    private static User MakeUser(
        string password = "FakeStrongPass123.",
        bool isDisabled = false,
        bool isOnline = false) => new()
    {
        Username = Username.Create("FakeUsername").Data!,
        Email = Email.Create("fake@email.com").Data!,
        Name = Name.Create("Faker", "Faker").Data!,
        PhoneNumber = null,
        Cpf = Cpf.Create("37678795068").Data!,
        Password = Password.Create(password).Data!,
        BirthDate = new DateOnly(2000, 1, 1),
        AcceptedTerms = true,
        AcceptedTermsAt = DateTime.UtcNow,
        UserType = UserType.Individual,
        IsDisabled = isDisabled,
        IsOnline = isOnline
    };

    private static TokenDto MakeTokenDto(
        string token = "fake_token",
        string? loginMethod = "username",
        string tokenType = "Bearer")
        => new(token, DateTime.UtcNow.AddHours(1), loginMethod, tokenType);

    private static LoginDto MakeLoginDto(
        string? login = "FakeUsername",
        string? password = "FakeStrongPass123.")
        => new(login!, password!);
}