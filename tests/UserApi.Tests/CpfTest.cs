using UserApi.ValueObjects;

namespace UserApi.Tests;

public class CpfTest
{
    [Theory(DisplayName = "Create valid cpf should success.")]
    [InlineData("801.639.350-09")]
    [InlineData("80163935009")]
    [InlineData("376.787.950-68")]
    [InlineData("37678795068")]
    public void Create_ValidCpf_ShouldSuccess(string cpf)
    {
        var result = Cpf.Create(cpf);
        Assert.True(result.IsSuccess);
    }

    [Theory(DisplayName = "Create invalid cpf should fail.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("00000000000")]
    [InlineData("12345678910")]
    [InlineData("123456789101")]
    [InlineData("11@86!3~9*.")]
    public void Create_InvalidCpf_ShouldFail(string? cpf)
    {
        var result = Cpf.Create(cpf!);
        Assert.True(result.IsFailure);
    }

    [Theory(DisplayName = "Create cpf with invalid first verificator digit should fail.")]
    [InlineData("801.639.350-19")]
    [InlineData("80163935019")]
    [InlineData("376.787.950-78")]
    [InlineData("37678795078")]
    public void Cpf_With_InvalidFirstVerificator_ShouldFail(string cpf)
    {
        var result = Cpf.Create(cpf);
        Assert.True(result.IsFailure);
    }

    [Theory(DisplayName = "Create cpf with invalid second verificator digit should fail.")]
    [InlineData("801.639.350-08")]
    [InlineData("80163935008")]
    [InlineData("376.787.950-69")]
    [InlineData("37678795069")]
    public void Cpf_With_Invalid_SecondVerificator_ShouldFail(string cpf)
    {
        var result = Cpf.Create(cpf);
        Assert.True(result.IsFailure);
    }
}