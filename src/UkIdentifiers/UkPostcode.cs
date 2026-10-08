using System.Text.RegularExpressions;

namespace UkIdentifiers;

/// <summary>
/// Validates and parses UK postcodes.
/// Format: outward code + inward code (e.g. "SW1A 1AA", "M1 1AE").
/// </summary>
public static partial class UkPostcode
{
    // Covers all valid outward-code patterns per BS 7666.
    [GeneratedRegex(
        @"^([A-Z]{1,2}\d[A-Z\d]?|ASCN|STHL|TDCU|BBND|[BFS]IQQ|PCRN|TKCA) ?\d[A-Z]{2}$|^GIR ?0A{2}$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FormatRegex();

    /// <summary>
    /// Returns true if <paramref name="value"/> is a structurally valid UK postcode.
    /// </summary>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return FormatRegex().IsMatch(value.Trim());
    }

    /// <summary>
    /// Normalizes a valid postcode to uppercase with a single space before
    /// the inward code (e.g. "sw1a1aa" → "SW1A 1AA").
    /// Throws <see cref="FormatException"/> if invalid.
    /// </summary>
    public static string Normalize(string value)
    {
        if (!IsValid(value))
            throw new FormatException($"'{value}' is not a valid UK postcode.");

        var compact = value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal).ToUpperInvariant();
        return compact[..^3] + " " + compact[^3..];
    }

    /// <summary>
    /// Splits a valid postcode into outward code ("SW1A") and inward code ("1AA").
    /// Throws <see cref="FormatException"/> if invalid.
    /// </summary>
    public static (string OutwardCode, string InwardCode) Split(string value)
    {
        var normalized = Normalize(value);
        var space = normalized.LastIndexOf(' ');
        return (normalized[..space], normalized[(space + 1)..]);
    }
}
