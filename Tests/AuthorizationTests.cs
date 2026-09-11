using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace SafeVault.Tests;

[TestFixture]
public sealed class AuthorizationTests
{
    [Test]
    public async Task AdminPolicy_AllowsAdminRole()
    {
        var services = new ServiceCollection();
        services.AddAuthorization(options => options.AddPolicy("AdminOnly", p => p.RequireRole("Admin")));
        var provider = services.BuildServiceProvider();
        var authorization = provider.GetRequiredService<IAuthorizationService>();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "Admin") }, "Test"));

        var result = await authorization.AuthorizeAsync(principal, null, "AdminOnly");

        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task AdminPolicy_DeniesNormalUser()
    {
        var services = new ServiceCollection();
        services.AddAuthorization(options => options.AddPolicy("AdminOnly", p => p.RequireRole("Admin")));
        var provider = services.BuildServiceProvider();
        var authorization = provider.GetRequiredService<IAuthorizationService>();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "User") }, "Test"));

        var result = await authorization.AuthorizeAsync(principal, null, "AdminOnly");

        Assert.That(result.Succeeded, Is.False);
    }
}
