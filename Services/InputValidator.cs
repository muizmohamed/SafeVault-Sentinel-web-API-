using System.Net.Mail;

namespace SafeVault.Services;

public static class InputValidator
{
    public static string NormalizeUsername(string input) => input.Trim();

    public static string NormalizeEmail(string input) => input.Trim().ToLowerInvariant();

    public static bool IsValidUsername(string input) =>
        !string.IsNullOrWhiteSpace(input) &&
        input.Length is >= 3 and <= 30 &&
        input.All(ch => char.IsLetterOrDigit(ch) || ch == '_') &&
        input.All(ch => ch <= 127);

    public static bool IsValidEmail(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > 100)
            return false;

        try
        {
            var address = new MailAddress(input.Trim());
            return string.Equals(address.Address, input.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
