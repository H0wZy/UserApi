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
        var dto = MakeCreateUserDto(acceptedTerms: false);
        var result = await _service.CreateAsync(dto);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync with invalid Cpf should fail")]
    public async Task CreateAsync_InvalidCpf_ShouldFail()
    {
        var dto = MakeCreateUserDto(cpf: "000.000.000-00");
        var result = await _service.CreateAsync(dto);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when Cpf already exists should fail")]
    public async Task CreateAsync_When_CpfAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetCpfExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);

        var dto = MakeCreateUserDto();
        var result = await _service.CreateAsync(dto);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when phone number already exists should fail")]
    public async Task CreateAsync_When_PhoneNumberAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetPhoneNumberExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var service = new UserService(_repositoryMock.Object, _mapperMock.Object);

        var dto = MakeCreateUserDto(phoneNumber: "43988888888");
        var result = await service.CreateAsync(dto);

        Assert.True(result.IsFailure);
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
        var result = await _service.UpdateByIdAsync(Guid.NewGuid(), MakeUpdateUserDto(username: "AnotherFakeUsername"));
        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "UpdateByIdAsync when email already exists should fail")]
    public async Task UpdateByIdAsync_When_EmailAlreadyExists_ShouldFail()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(MakeUser());
        _repositoryMock.Setup(r => r.GetEmailExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var result =
            await _service.UpdateByIdAsync(Guid.NewGuid(), MakeUpdateUserDto(email: "another_fake_email@email.com"));
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
        UserType = UserType.Individual
    };

    private static UpdateUserDto MakeUpdateUserDto(
        string? username = null,
        string? email = null,
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null)
        => new(username, email, firstName, lastName, phoneNumber);

    private static CreateUserDto MakeCreateUserDto(
        string cpf = "529.982.247-25",
        string password = "StrongPass123.",
        bool acceptedTerms = true,
        string? phoneNumber = null)
        => new(
            Username: "testUser",
            Email: "test@email.com",
            FirstName: "Mark",
            LastName: "Tester",
            phoneNumber,
            password,
            cpf,
            BirthDate: new DateOnly(2000, 1, 1),
            acceptedTerms,
            UserType: UserType.Individual
        );
}