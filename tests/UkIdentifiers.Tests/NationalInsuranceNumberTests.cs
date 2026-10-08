using AwesomeAssertions;
using Xunit;

namespace UkIdentifiers.Tests;

public class NationalInsuranceNumberTests
{
    [Theory]
    [InlineData("AB123456C")]
    [InlineData("AB123456D")]
    [InlineData("ab123456c")]       // lowercase accepted
    [InlineData("AB 12 34 56 C")]   // spaces accepted
    public void IsValid_accepts_wellformed_numbers(string value)
    {
        NationalInsuranceNumber.IsValid(value).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("QQ12345C")]        // too short
    [InlineData("QQ1234567C")]      // too long
    [InlineData("Q1123456C")]       // digit in prefix
    [InlineData("QQ12345XC")]       // letter in digits
    [InlineData("QQ123456C")]       // Q not allowed as first char
    [InlineData("DQ123456C")]       // D not allowed as first char
    [InlineData("QO123456C")]       // O not allowed as second char
    [InlineData("QQ123456E")]       // E not allowed as suffix
    [InlineData("BG123456C")]       // invalid prefix
    [InlineData("GB123456C")]       // invalid prefix
    [InlineData("ZZ123456C")]       // invalid prefix (never issued)
    public void IsValid_rejects_malformed_numbers(string? value)
    {
        NationalInsuranceNumber.IsValid(value).Should().BeFalse();
    }

    [Fact]
    public void Normalize_uppercases_and_strips_spaces()
    {
        NationalInsuranceNumber.Normalize("ab 12 34 56 c").Should().Be("AB123456C");
    }

    [Fact]
    public void Normalize_throws_for_invalid_input()
    {
        var act = () => NationalInsuranceNumber.Normalize("not-a-nino");
        act.Should().Throw<FormatException>();
    }
}
