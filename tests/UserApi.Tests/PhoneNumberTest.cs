using UserApi.ValueObjects;

namespace UserApi.Tests;

public class PhoneNumberTest
{
    [Theory(DisplayName = "create valid phone number should success")]
    [InlineData("43988888888")]
    [InlineData("4398888-8888")]
    [InlineData("5543988888888")]
    [InlineData("554398888-8888")]
    public void Create_Valid_PhoneNumber_ShouldSuccess(string phoneNumber)
    {
        var result = PhoneNumber.Create(phoneNumber);
        Assert.True(result.IsSuccess);
    }

    [Theory(DisplayName = "create invalid phone number should fail")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("55439888888889")]
    [InlineData("554398888")]
    [InlineData("abcdefghij")]
    [InlineData("4398abc8888")]
    public void Create_Invalid_PhoneNumber_ShouldFail(string? phoneNumber)
    {
        var result = PhoneNumber.Create(phoneNumber);
        Assert.True(result.IsFailure);
    }
}