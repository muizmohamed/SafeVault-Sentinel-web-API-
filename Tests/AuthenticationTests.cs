using NUnit.Framework;
using SafeVault.Data;
using SafeVault.Models;
using SafeVault.Services;

namespace SafeVault.Tests;

[TestFixture]
public sealed class AuthenticationTests
{
    [Test]
    public void BCrypt_HashCanBeVerified_WithoutStoringPlaintext()
    {
        const string password = "Correct-Horse-Battery-2026";
        var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        Assert.That(hash, Does.Not.Contain(password));
        Assert.That(BCrypt.Net.BCrypt.Verify(password, hash), Is.True);
        Assert.That(BCrypt.Net.BCrypt.Verify("WrongPassword", hash), Is.False);
    }

    [Test]
    public void Registration_RejectsUnsafeUsername()
    {
        // Validation is isolated from persistence here so the security rule is deterministic.
        var input = new RegisterInput
        {
            Username = "admin' OR '1'='1",
            Email = "user@example.com",
            Password = "Correct-Horse-Battery-2026"
        };

        Assert.That(InputValidator.IsValidUsername(input.Username), Is.False);
    }
}
