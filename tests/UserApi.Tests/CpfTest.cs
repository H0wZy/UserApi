using UserApi.ValueObjects;
using Xunit;

namespace UserApi.Tests;

public class CpfTest
{

    [Fact(DisplayName = "Create valid cpf should return success.")]
    public void Create_ValidCpf_ShouldReturnSuccess()
    {
        const string cpf = "37678795068";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "Create valid cpf with formatting should return success.")]
    public void Create_ValidCpf_WithFormatting_ShouldReturnSuccess()
    {
        const string cpf = "801.639.350-09";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "Create with all same digits should return failure.")]
    public void Create_WithAllSameDigits_ShouldReturnFailure()
    {
        const string cpf = "00000000000";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "Create cpf with more than 11 digits should return failure.")]
    public void Create_WithMoreThan11Digits_ShouldReturnFailure()
    {
        const string cpf = "123456789101";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "Create cpf with less than 11 digits should return failure.")]
    public void Create_WithLessThan11Digits_ShouldReturnFailure()
    {
        const string cpf = "8605057408";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "Create cpf with invalid first verificator digit should fail.")]
    public void Cpf_WithInvalidFirstVerificator_ShouldReturnFailure()
    {
        const string cpf = "37678795078";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "Create cpf with invalid second verificator digit should fail.")]
    public void Cpf_WithInvalidSecondVerificator_ShouldReturnFailure()
    {
        const string cpf = "54941209027";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }

    [Fact(DisplayName = "Create cpf with invalid characters should fail.")]
    public void Create_WithInvalidCharacters_ShouldFail()
    {
        const string cpf = "11@86!3~9*.";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }
}