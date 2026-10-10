using AwesomeAssertions;
using Xunit;

namespace UkIdentifiers.Tests;

public class NhsNumberTests
{
    [Theory]
    [InlineData("4505577104")]      // check digit 4 verified by hand
    [InlineData("1111111111")]      // check digit 1 verified by hand
    [InlineData("450 557 7104")]    // spaces accepted
    public void IsValid_accepts_numbers_with_correct_check_digit(string value)
    {
        NhsNumber.IsValid(value).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("450557710")]       // too short
    [InlineData("45055771044")]     // too long
    [InlineData("4505577105")]      // wrong check digit
    [InlineData("1234567890")]      // remainder 10 → can never be valid
    [InlineData("450557710A")]      // non-digit
    public void IsValid_rejects_malformed_numbers(string? value)
    {
        NhsNumber.IsValid(value).Should().BeFalse();
    }

    [Fact]
    public void Normalize_strips_spaces()
    {
        NhsNumber.Normalize("450 557 7104").Should().Be("4505577104");
    }

    [Fact]
    public void Normalize_throws_for_invalid_input()
    {
        var act = () => NhsNumber.Normalize("4505577105");
        act.Should().Throw<FormatException>();
    }
}
