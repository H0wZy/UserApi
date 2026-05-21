using UserApi.ValueObjects;

namespace UserApi.Tests;

public class CpfTest
{
    [Fact(DisplayName = "Create with all same digits should return failure.")]
    public void Create_WithAllSameDigits_ShouldReturnFailure()
    {
        var cpf = "00000000000";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }

    // TODO FIX THIS:
    [Fact(DisplayName = "Create with points should return failure.")]
    public void Create_WithPoints_ShouldReturnSuccess()
    {
        var cpf = "801.639.350-09";

        var result = Cpf.Create(cpf);

        Assert.True(result.IsFailure);
    }
}