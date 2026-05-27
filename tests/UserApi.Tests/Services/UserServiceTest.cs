using AutoMapper;
using Moq;
using UserApi.Dto;
using UserApi.Enum;
using UserApi.Repositories;
using UserApi.Services;

namespace UserApi.Tests.Services;

public class UserServiceTest
{
    [Fact(DisplayName = "CreateAsync without accepting terms should fail")]
    public async Task CreateAsync_WithoutAcceptingTerms_ShouldFail()
    {
        var repositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();
        var service = new UserService(repositoryMock.Object, mapperMock.Object);

        var dto = MakeDto(acceptedTerms: false);

        var result = await service.CreateAsync(dto);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync with invalid Cpf should fail")]
    public async Task CreateAsync_InvalidCpf_ShouldFail()
    {
        var repositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();
        var service = new UserService(repositoryMock.Object, mapperMock.Object);

        var dto = MakeDto(cpf: "000.000.000-00");

        var result = await service.CreateAsync(dto);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when Cpf already exists should fail")]
    public async Task CreateAsync_When_CpfAlreadyExists_ShouldFail()
    {
        var repositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock.Setup(r => r.GetCpfExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var service = new UserService(repositoryMock.Object, mapperMock.Object);

        var dto = MakeDto();
        var result = await service.CreateAsync(dto);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "CreateAsync when phone number already exists should fail")]
    public async Task CreateAsync_When_PhoneNumberAlreadyExists_ShouldFail()
    {
        var repositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock.Setup(r => r.GetPhoneNumberExistenceAsync(It.IsAny<string>())).ReturnsAsync(true);
        var service = new UserService(repositoryMock.Object, mapperMock.Object);

        var dto = MakeDto(phoneNumber: "43988888888");
        var result = await service.CreateAsync(dto);

        Assert.True(result.IsFailure);
    }

    private static CreateUserDto MakeDto(
        string cpf = "529.982.247-25",
        string password = "StrongPass123.",
        bool acceptedTerms = true,
        string? phoneNumber = null)
        => new(
            Username: "testUser",
            Email: "test@email.com",
            FirstName: "Mark",
            LastName: "Tester",
            PhoneNumber: phoneNumber,
            Password: password,
            Cpf: cpf,
            BirthDate: new DateOnly(2000, 1, 1),
            AcceptedTerms: acceptedTerms,
            UserType: UserType.Individual
        );
}