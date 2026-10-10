using AwesomeAssertions;
using Xunit;

namespace UkIdentifiers.Tests;

public class UkVatNumberTests
{
    [Theory]
    [InlineData("GB123456789")]
    [InlineData("gb123456789")]        // lowercase prefix accepted
    [InlineData("GB 123 4567 89")]    // spaces accepted
    [InlineData("XI123456789")]       // Northern Ireland prefix
    [InlineData("123456789")]         // prefix optional
    [InlineData("GB123456789012")]    // 12-digit branch trader format
    public void IsValid_accepts_wellformed_numbers(string value)
    {
        UkVatNumber.IsValid(value).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("GB12345678")]        // too short
    [InlineData("GB1234567890")]      // 10 digits — neither 9 nor 12
    [InlineData("FR123456789")]       // wrong country prefix
    [InlineData("GB12345678A")]       // non-digit
    public void IsValid_rejects_malformed_numbers(string? value)
    {
        UkVatNumber.IsValid(value).Should().BeFalse();
    }

    [Fact]
    public void Normalize_uppercases_and_strips_spaces_preserving_prefix()
    {
        UkVatNumber.Normalize("gb 123 4567 89").Should().Be("GB123456789");
    }

    [Fact]
    public void Normalize_throws_for_invalid_input()
    {
        var act = () => UkVatNumber.Normalize("GB123");
        act.Should().Throw<FormatException>();
    }
}
