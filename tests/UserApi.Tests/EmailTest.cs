using UserApi.ValueObjects;

namespace UserApi.Tests;

public class EmailTest
{
    [Fact(DisplayName= "create valid email should success")]
    public void Create_ValidEmail_ShouldSuccess()
    {
        const string email = "mr.resenha!@gmail.com";
        var result = Email.Create(email);
        Assert.True(result.IsSuccess);
    }
}