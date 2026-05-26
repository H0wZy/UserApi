using UserApi.ValueObjects;

namespace UserApi.Tests.ValueObjects;

public class PasswordTest
{
    [Theory(DisplayName = "create valid password should success")]
    [InlineData("StrongPass123.")]
    [InlineData("TheMost_StrongestPassword_OfThe_W0rld_12732137!@#.")]
    public void Create_ValidPassword_ShouldSuccess(string password)
    {
        var result = Password.Create(password);
        Assert.True(result.IsSuccess);
    }

    [Theory(DisplayName = "create invalid password should fail")]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("StrongPass")]
    [InlineData("StrongPass123")]
    [InlineData("strongpass123.")]
    [InlineData("7Dig!ts")]
    [InlineData("Password_With_More_Than_128_Characters_ibue$a3KWRhB3j727T9Cdd2WDkSSoqCY8TMze$auwbxtmvZ2oomENQnjPpfL$ZAkPZN7i4fQh4#SET#&n!RhHjL!mN")]
    public void Create_InvalidPassword_ShouldFail(string? password)
    {
        var result = Password.Create(password!);
        Assert.True(result.IsFailure);
    }
}