using AwesomeAssertions;
using Xunit;

namespace UkIdentifiers.Tests;

public class CompaniesHouseNumberTests
{
    [Theory]
    [InlineData("01234567")]       // 8 digits
    [InlineData("AB123456")]       // 2 letters + 6 digits
    [InlineData("SC123456")]       // Scottish prefix
    [InlineData("NI123456")]       // Northern Ireland prefix
    [InlineData("ab123456")]       // lowercase accepted
    public void IsValid_accepts_wellformed_numbers(string value)
    {
        CompaniesHouseNumber.IsValid(value).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234567")]        // too short
    [InlineData("123456789")]      // too long
    [InlineData("A1234567")]       // 1 letter + 7 digits
    [InlineData("ABC12345")]       // 3 letters + 5 digits
    [InlineData("AB12345A")]       // trailing letter
    public void IsValid_rejects_malformed_numbers(string? value)
    {
        CompaniesHouseNumber.IsValid(value).Should().BeFalse();
    }

    [Fact]
    public void Normalize_uppercases()
    {
        CompaniesHouseNumber.Normalize("ab123456").Should().Be("AB123456");
    }

    [Fact]
    public void Normalize_throws_for_invalid_input()
    {
        var act = () => CompaniesHouseNumber.Normalize("nope");
        act.Should().Throw<FormatException>();
    }
}
