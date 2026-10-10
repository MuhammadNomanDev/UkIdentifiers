using System.Text.RegularExpressions;

namespace UkIdentifiers;

/// <summary>
/// Validates UK NHS numbers: 10 digits with a Mod-11 check digit.
/// </summary>
public static partial class NhsNumber
{
    [GeneratedRegex(@"^\d{10}$", RegexOptions.CultureInvariant)]
    private static partial Regex FormatRegex();

    /// <summary>
    /// Returns true if <paramref name="value"/> is a 10-digit number with a
    /// valid Mod-11 check digit. Does not verify the number is issued.
    /// </summary>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var digits = value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal);
        if (!FormatRegex().IsMatch(digits))
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += (digits[i] - '0') * (10 - i);

        int check = 11 - (sum % 11);
        if (check == 11)
            check = 0;
        if (check == 10)
            return false; // remainder 10 can never yield a valid check digit

        return (digits[9] - '0') == check;
    }

    /// <summary>
    /// Normalizes a valid NHS number to 10 digits without spaces.
    /// Throws <see cref="FormatException"/> if invalid.
    /// </summary>
    public static string Normalize(string value)
    {
        if (!IsValid(value))
            throw new FormatException($"'{value}' is not a valid NHS number.");

        return value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal);
    }
}
