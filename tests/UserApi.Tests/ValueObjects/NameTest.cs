using UserApi.ValueObjects;

namespace UserApi.Tests.ValueObjects;

public class NameTest
{
    [Theory(DisplayName = "create valid name should success")]
    [InlineData("João", "Da Silva")]
    [InlineData("João Matheus", "De Souza Oliveira")]
    [InlineData("abc", "xyz")]
    [InlineData("  Marcelo  ", "  Reis  ")]
    public void Create_ValidName_ShouldSuccess(string? firstName, string? lastName)
    {
        var result = Name.Create(firstName, lastName);
        Assert.True(result.IsSuccess);
    }

    [Theory(DisplayName = "create invalid name should fail")]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("Luís", " ")]
    [InlineData("  ", "César")]
    [InlineData("null", "null")]
    [InlineData("admin", "Da Silva")]
    [InlineData("user", "De Souza Oliveira")]
    [InlineData("J0oã0 $iLv4", "Tr3m b4L@ !")]
    [InlineData("J0oã0 $iLv4", "Trem Bala")]
    [InlineData("João Silva", "Tr3m b4L@ !")]
    [InlineData("ab", "cd")]
    [InlineData("firstNameWithMoreThanFiftyCharactersAAAAAAAAAAAAAAA", "lastNameWithMoreThanFiftyCharactersAAAAAAAAAAAAAAAA")]
    public void Create_InvalidName_ShouldFail(string? firstName, string? lastName)
    {
        var result = Name.Create(firstName, lastName);
        Assert.True(result.IsFailure);
    }
}