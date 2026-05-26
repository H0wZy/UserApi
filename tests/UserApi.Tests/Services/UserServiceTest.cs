using AutoMapper;
using Moq;
using UserApi.Repositories;
using UserApi.Services;

namespace UserApi.Tests.Services;

public class UserServiceTest
{
    [Fact(DisplayName = "CreateAsync without accepting the terms should fail")]
    public void CreateAsync_WithoutAcceptingTerms_ShouldFail()
    {
        var repositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();
        var service = new UserService(repositoryMock.Object, mapperMock.Object);

        
    }
}