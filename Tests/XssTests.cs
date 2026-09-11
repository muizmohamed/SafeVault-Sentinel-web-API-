using NUnit.Framework;
using SafeVault.Services;

namespace SafeVault.Tests;

[TestFixture]
public sealed class XssTests
{
    [Test]
    public void HtmlEncoder_NeutralizesScriptMarkup()
    {
        const string payload = "<script>alert('xss')</script>";

        var encoded = SafeOutputEncoder.HtmlEncode(payload);

        Assert.That(encoded, Does.Not.Contain("<script>"));
        Assert.That(encoded, Does.Contain("&lt;script&gt;"));
    }

    [Test]
    public void HtmlEncoder_NeutralizesEventHandlerPayload()
    {
        const string payload = "<img src=x onerror=alert(1)>";

        var encoded = SafeOutputEncoder.HtmlEncode(payload);

        Assert.That(encoded, Does.Not.Contain("onerror="));
        Assert.That(encoded, Does.Contain("&lt;img"));
    }
}
