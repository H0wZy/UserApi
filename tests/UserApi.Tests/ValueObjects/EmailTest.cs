using UserApi.ValueObjects;

namespace UserApi.Tests.ValueObjects;

public class EmailTest
{
    [Theory(DisplayName = "create valid email should success")]
    [InlineData("mr.dot@email.com")]
    [InlineData("mr-dash@email.com")]
    [InlineData("mr_underline@email.com")]
    [InlineData("lettersFirst123@email.com")]
    [InlineData("123numbersFirst@email.com")]
    public void Create_ValidEmail_ShouldSuccess(string email)
    {
        var result = Email.Create(email);
        Assert.True(result.IsSuccess);
    }

    [Theory(DisplayName = "create invalid email should fail")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("@")]
    [InlineData("@email.com")]
    [InlineData("user")]
    [InlineData("user_email.com")]
    [InlineData("user@")]
    [InlineData("user@email")]
    [InlineData(".start_with_points@email.com")]
    [InlineData(".start_and_end_with_points@email.com.")]
    [InlineData(".start_and_end_with_points_without_com@email.")]
    [InlineData("email_with_more_than_64_characters_AAAAAAAAAAAAAAAAAAAA@email.com")]
    public void Create_InvalidEmail_ShouldFail(string? email)
    {
        var result = Email.Create(email);
        Assert.True(result.IsFailure);
    }
}