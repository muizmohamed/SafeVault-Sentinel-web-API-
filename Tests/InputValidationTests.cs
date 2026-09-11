using NUnit.Framework;
using SafeVault.Services;

namespace SafeVault.Tests;

[TestFixture]
public sealed class InputValidationTests
{
    [TestCase("alice_123")]
    [TestCase("User01")]
    public void ValidUsername_IsAccepted(string username)
    {
        Assert.That(InputValidator.IsValidUsername(username), Is.True);
    }

    [TestCase("admin' OR '1'='1")]
    [TestCase("<script>alert(1)</script>")]
    [TestCase("user name")]
    public void MaliciousOrInvalidUsername_IsRejected(string username)
    {
        Assert.That(InputValidator.IsValidUsername(username), Is.False);
    }

    [TestCase("user@example.com")]
    [TestCase("student@university.edu")]
    public void ValidEmail_IsAccepted(string email)
    {
        Assert.That(InputValidator.IsValidEmail(email), Is.True);
    }

    [TestCase("not-an-email")]
    [TestCase("<script>alert(1)</script>@example.com")]
    public void InvalidEmail_IsRejected(string email)
    {
        Assert.That(InputValidator.IsValidEmail(email), Is.False);
    }
}
