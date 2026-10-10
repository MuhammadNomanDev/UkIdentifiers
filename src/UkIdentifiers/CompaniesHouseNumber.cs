using System.Text.RegularExpressions;

namespace UkIdentifiers;

/// <summary>
/// Validates Companies House company numbers: 8 digits, or 2 letters
/// followed by 6 digits (e.g. "01234567", "AB123456", "SC123456").
/// </summary>
public static partial class CompaniesHouseNumber
{
    [GeneratedRegex(@"^([A-Z]{2}\d{6}|\d{8})$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FormatRegex();

    /// <summary>
    /// Returns true if <paramref name="value"/> matches the Companies House
    /// number format. There is no check digit — format only.
    /// </summary>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return FormatRegex().IsMatch(value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal));
    }

    /// <summary>
    /// Normalizes a valid company number to uppercase without spaces.
    /// Throws <see cref="FormatException"/> if invalid.
    /// </summary>
    public static string Normalize(string value)
    {
        if (!IsValid(value))
            throw new FormatException($"'{value}' is not a valid Companies House number.");

        return value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal).ToUpperInvariant();
    }
}
