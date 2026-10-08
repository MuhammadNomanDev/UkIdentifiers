using System.Text.RegularExpressions;

namespace UkIdentifiers;

/// <summary>
/// Validates UK National Insurance numbers.
/// Format: two letters, six digits, one letter (e.g. "QQ123456C").
/// </summary>
public static partial class NationalInsuranceNumber
{
    // First char excludes D, F, I, Q, U, V; second excludes D, F, I, O, Q, U, V.
    // Final char is A-D (administrative suffix).
    [GeneratedRegex(@"^[A-CEGHJ-PRSTW-Z][A-CEGHJ-NPRSTW-Z]\d{6}[A-D]$",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FormatRegex();

    // Prefixes never issued by HMRC.
    private static readonly HashSet<string> InvalidPrefixes = new(StringComparer.OrdinalIgnoreCase)
    {
        "BG", "GB", "KN", "NK", "NT", "TN", "ZZ"
    };

    /// <summary>
    /// Returns true if <paramref name="value"/> is a structurally valid
    /// National Insurance number. Does not verify the number is issued.
    /// </summary>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var normalized = value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal);
        if (!FormatRegex().IsMatch(normalized))
            return false;

        return !InvalidPrefixes.Contains(normalized[..2]);
    }

    /// <summary>
    /// Normalizes a valid NI number to uppercase without spaces
    /// (e.g. "qq 12 34 56 c" → "QQ123456C").
    /// Throws <see cref="FormatException"/> if invalid.
    /// </summary>
    public static string Normalize(string value)
    {
        if (!IsValid(value))
            throw new FormatException($"'{value}' is not a valid National Insurance number.");

        return value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal).ToUpperInvariant();
    }
}
