using UserApi.ValueObjects;

namespace UserApi.Tests.ValueObjects;

public class UsernameTest
{
    [Theory(DisplayName = "create valid username should success")]
    [InlineData("123resenha123")]
    [InlineData("abc")]
    [InlineData("usernameWith20charsX")]
    [InlineData("User123")]
    [InlineData("  User123  ")]
    public void Create_ValidUsername_ShouldSuccess(string username)
    {
        var result = Username.Create(username);
        Assert.True(result.IsSuccess);
    }

    [Theory(DisplayName = "create invalid username should fail")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("ab")]
    [InlineData("usernameWithMoreThan30charsShouldFail")]
    [InlineData("randomUser_!#@552")]
    [InlineData("admin")]
    [InlineData("root")]
    [InlineData("user")]
    public void Create_InvalidUsername_ShouldFail(string? username)
    {
        var result = Username.Create(username);
        Assert.True(result.IsFailure);
    }

    [Theory(DisplayName = "create username should normalize value")]
    [InlineData("User123", "user123")]
    [InlineData("  User123  ", "user123")]
    public void Create_ValidUsername_ShouldNormalizeValue(string username, string expected)
    {
        var result = Username.Create(username);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Data!.Value);
    }
}