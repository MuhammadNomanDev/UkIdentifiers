using AwesomeAssertions;
using Xunit;

namespace UkIdentifiers.Tests;

public class UkPostcodeTests
{
    [Theory]
    [InlineData("SW1A 1AA")]
    [InlineData("M1 1AE")]
    [InlineData("B33 8TH")]
    [InlineData("CR2 6XH")]
    [InlineData("DN55 1PT")]
    [InlineData("sw1a1aa")]          // lowercase, no space
    [InlineData("GIR 0AA")]         // special case
    public void IsValid_accepts_wellformed_postcodes(string value)
    {
        UkPostcode.IsValid(value).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")]
    [InlineData("SW1A 1A")]         // inward too short
    [InlineData("SW1A 1AAA")]        // inward too long
    [InlineData("1A 1AA")]          // no outward letters
    public void IsValid_rejects_malformed_postcodes(string? value)
    {
        UkPostcode.IsValid(value).Should().BeFalse();
    }

    [Fact]
    public void Normalize_formats_with_single_space()
    {
        UkPostcode.Normalize("sw1a1aa").Should().Be("SW1A 1AA");
        UkPostcode.Normalize("M11AE").Should().Be("M1 1AE");
    }

    [Fact]
    public void Split_returns_outward_and_inward_codes()
    {
        var (outward, inward) = UkPostcode.Split("sw1a1aa");
        outward.Should().Be("SW1A");
        inward.Should().Be("1AA");
    }

    [Fact]
    public void Normalize_throws_for_invalid_input()
    {
        var act = () => UkPostcode.Normalize("not-a-postcode");
        act.Should().Throw<FormatException>();
    }
}
