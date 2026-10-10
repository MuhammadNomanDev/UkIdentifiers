using AwesomeAssertions;
using Xunit;

namespace UkIdentifiers.Tests;

public class UniqueTaxpayerReferenceTests
{
    [Theory]
    [InlineData("1234567890")]
    [InlineData("0000000000")]
    [InlineData(" 1234567890 ")]   // surrounding whitespace accepted
    [InlineData("12345 67890")]    // inner spaces accepted
    public void IsValid_accepts_ten_digits(string value)
    {
        UniqueTaxpayerReference.IsValid(value).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456789")]      // too short
    [InlineData("12345678901")]    // too long
    [InlineData("123456789A")]     // non-digit
    public void IsValid_rejects_non_ten_digit_input(string? value)
    {
        UniqueTaxpayerReference.IsValid(value).Should().BeFalse();
    }

    [Fact]
    public void Normalize_strips_spaces()
    {
        UniqueTaxpayerReference.Normalize("12345 67890").Should().Be("1234567890");
    }

    [Fact]
    public void Normalize_throws_for_invalid_input()
    {
        var act = () => UniqueTaxpayerReference.Normalize("12345");
        act.Should().Throw<FormatException>();
    }
}
