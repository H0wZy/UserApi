using AutoMapper;
using Moq;
using UserApi.Dto;
using UserApi.Enum;
using UserApi.Models;
using UserApi.Repositories;
using UserApi.Services;
using UserApi.ValueObjects;

namespace UserApi.Tests.Services;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UserService _service;

    public UserServiceTest()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new UserService(_repositoryMock.Object, _mapperMock.Object);
    }

    #region CreateAsync Tests

    [Fact(DisplayName = "CreateAsync without accepting terms should fail")]
    public async Task CreateAsync_WithoutAcceptingTerms_ShouldFail()
    {
        var result = await _service.CreateAsync(MakeCreateUserDto(acceptedTerms: false));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync without select account type should fail")]
    public async Task CreateAsync_WithoutSelectAccountType_ShouldFail()
    {
        var result = await _service.CreateAsync(MakeCreateUserDto(accType: 0));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync with invalid Cpf should fail")]
    public async Task CreateAsync_InvalidCpf_ShouldFail()
    {
        var result = await _service.CreateAsync(MakeCreateUserDto(cpf: "000.000.000-00"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync with invalid password should fail")]
    public async Task CreateAsync_InvalidPassword_ShouldFail()
    {
        var result = await _service.CreateAsync(MakeCreateUserDto(password: "invalid_password"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when Cpf already exists should fail")]
    public async Task CreateAsync_When_CpfAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetCpfExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var result = await _service.CreateAsync(MakeCreateUserDto(cpf: "529.982.247-25"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when phone number already exists should fail")]
    public async Task CreateAsync_When_PhoneNumberAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetPhoneNumberExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var result = await _service.CreateAsync(MakeCreateUserDto(phoneNumber: "43988888888"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when email already exists should fail")]
    public async Task CreateAsync_When_EmailAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetEmailExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var result = await _service.CreateAsync(MakeCreateUserDto(email: "this_email_already_exists@email.com"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when username already exists should fail")]
    public async Task CreateAsync_When_UsernameAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetUsernameExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var result = await _service.CreateAsync(MakeCreateUserDto(username: "thisAlreadyExists"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync should success")]
    public async Task CreateAsync_ShouldSuccess()
    {
        var result = await _service.CreateAsync(MakeCreateUserDto());
        Assert.True(result.IsSuccess);
    }

    #endregion

    #region UpdateByIdAsync Tests

    [Fact(DisplayName = "UpdateByIdAsync when user dont exists should fail")]
    public async Task UpdateByIdAsync_When_UserDontExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
        var result = await _service.UpdateByIdAsync(Guid.NewGuid(), MakeUpdateUserDto(phoneNumber: "43988888888"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdateByIdAsync when username already exists should fail")]
    public async Task UpdateByIdAsync_When_UsernameAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        _repositoryMock.Setup(r => r.GetUsernameExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var result =
            await _service.UpdateByIdAsync(Guid.NewGuid(), MakeUpdateUserDto(username: "changeToAnotherFakeUsername"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdateByIdAsync when email already exists should fail")]
    public async Task UpdateByIdAsync_When_EmailAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        _repositoryMock.Setup(r => r.GetEmailExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var result =
            await _service.UpdateByIdAsync(Guid.NewGuid(),
                MakeUpdateUserDto(email: "change_to_another_fake_email@email.com"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdateByIdAsync should success")]
    public async Task UpdateByIdAsync_ShouldSuccess()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        var result = await _service.UpdateByIdAsync(Guid.NewGuid(),
            MakeUpdateUserDto(username: "ValidUsername", email: "valid_email@email.com"));
        Assert.True(result.IsSuccess);
    }

    #endregion

    #region UpdateUserPasswordAsync Tests

    [Fact(DisplayName = "UpdatePassword when user dont exists should fail")]
    public async Task UpdatePassword_When_UserDontExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
        var result = await _service.UpdateUserPasswordAsync(Guid.NewGuid(), MakeUpdatePasswordDto());
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdatePassword with invalid new password should fail")]
    public async Task UpdatePassword_WithInvalid_NewPassword_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        var result =
            await _service.UpdateUserPasswordAsync(Guid.NewGuid(),
                MakeUpdatePasswordDto(newPassword: "invalid_password"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdatePassword when current password dont match should fail")]
    public async Task UpdatePassword_WhenCurrentPassword_DontMatch_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        var result = await _service.UpdateUserPasswordAsync(Guid.NewGuid(),
            MakeUpdatePasswordDto(currentPassword: "ThisCurrentFakeStrongPass123DontMatch."));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdatePassword when new password is the same as current password should fail")]
    public async Task UpdatePassword_When_CurrentPassword_IsTheSameAs_NewPassword_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        var result = await _service.UpdateUserPasswordAsync(Guid.NewGuid(),
            MakeUpdatePasswordDto(newPassword: "FakeStrongPass123."));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdatePassword should success")]
    public async Task UpdatePassword_ShouldSuccess()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        var result = await _service.UpdateUserPasswordAsync(Guid.NewGuid(), MakeUpdatePasswordDto());
        Assert.True(result.IsSuccess);
    }

    #endregion

    private static User MakeUser() => new()
    {
        Username = Username.Create("FakeUsername").Data!,
        Email = Email.Create("fake@email.com").Data!,
        Name = Name.Create("Faker", "Faker").Data!,
        PhoneNumber = null,
        Cpf = Cpf.Create("37678795068").Data!,
        Password = Password.Create("FakeStrongPass123.").Data!,
        BirthDate = new DateOnly(2000, 1, 1),
        AcceptedTerms = true,
        AcceptedTermsAt = DateTime.UtcNow,
        Type = AccType.Individual,
        Role = Role.CommonUser
    };

    private static UpdatePasswordDto MakeUpdatePasswordDto(
        string currentPassword = "FakeStrongPass123.",
        string newPassword = "NewFakeStrongPass123.")
        => new(currentPassword, newPassword);

    private static UpdateUserDto MakeUpdateUserDto(
        string? username = null,
        string? email = null,
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null)
        => new(username, email, firstName, lastName, phoneNumber);

    private static CreateUserDto MakeCreateUserDto(
        string username = "FakeUsername",
        string email = "fake_email@email.com",
        string cpf = "529.982.247-25",
        string password = "StrongPass123.",
        bool acceptedTerms = true,
        AccType accType = AccType.Individual,
        string? phoneNumber = null)
        => new(
            username,
            email,
            FirstName: "Mark",
            LastName: "Tester",
            phoneNumber,
            password,
            cpf,
            accType,
            BirthDate: new DateOnly(2000, 1, 1),
            acceptedTerms
        );
}