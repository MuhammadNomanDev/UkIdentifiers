using System.Text.RegularExpressions;

namespace UkIdentifiers;

/// <summary>
/// Validates HMRC Unique Taxpayer References (UTR): 10 digits.
/// </summary>
public static partial class UniqueTaxpayerReference
{
    [GeneratedRegex(@"^\d{10}$", RegexOptions.CultureInvariant)]
    private static partial Regex FormatRegex();

    /// <summary>
    /// Returns true if <paramref name="value"/> is 10 digits.
    /// HMRC's check-digit algorithm is not public, so this is format-only
    /// validation — it cannot confirm a UTR was actually issued.
    /// </summary>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return FormatRegex().IsMatch(value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal));
    }

    /// <summary>
    /// Normalizes a valid UTR to 10 digits without spaces.
    /// Throws <see cref="FormatException"/> if invalid.
    /// </summary>
    public static string Normalize(string value)
    {
        if (!IsValid(value))
            throw new FormatException($"'{value}' is not a valid Unique Taxpayer Reference.");

        return value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal);
    }
}
