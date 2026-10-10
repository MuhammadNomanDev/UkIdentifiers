using System.Text.RegularExpressions;

namespace UkIdentifiers;

/// <summary>
/// Validates UK VAT numbers: GB (or XI for Northern Ireland) prefix plus
/// 9 digits, or 12 digits for some group/branch traders.
/// </summary>
public static partial class UkVatNumber
{
    [GeneratedRegex(@"^(GB|XI)?\d{9}(\d{3})?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FormatRegex();

    /// <summary>
    /// Returns true if <paramref name="value"/> matches the UK VAT format.
    /// The Mod-97 check only applies to old-style numbers, so this is
    /// format-only validation — it cannot confirm VAT registration.
    /// </summary>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return FormatRegex().IsMatch(value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal));
    }

    /// <summary>
    /// Normalizes a valid VAT number to uppercase without spaces, preserving
    /// any GB/XI prefix (e.g. "gb 123 4567 89" → "GB123456789").
    /// Throws <see cref="FormatException"/> if invalid.
    /// </summary>
    public static string Normalize(string value)
    {
        if (!IsValid(value))
            throw new FormatException($"'{value}' is not a valid UK VAT number.");

        return value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal).ToUpperInvariant();
    }
}
