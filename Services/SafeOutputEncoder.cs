using System.Text.Encodings.Web;

namespace SafeVault.Services;

public static class SafeOutputEncoder
{
    // Use only when an encoded string is explicitly required by an API or test.
    // Razor views should normally encode untrusted values automatically.
    public static string HtmlEncode(string value) => HtmlEncoder.Default.Encode(value);
}
