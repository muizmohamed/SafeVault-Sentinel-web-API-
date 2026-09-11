using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Models;
using SafeVault.Services;

namespace SafeVault.Controllers;

public sealed class AccountController : Controller
{
    private readonly AuthenticationService _authentication;

    public AccountController(AuthenticationService authentication) => _authentication = authentication;

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View(new RegisterInput());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult Register(RegisterInput input)
    {
        if (!ModelState.IsValid)
            return View(input);

        if (!_authentication.Register(input, out var error))
        {
            ModelState.AddModelError(string.Empty, error ?? "Unable to create account.");
            return View(input);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null) => View(new LoginInput());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginInput input, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(input);

        var user = _authentication.Authenticate(input.Username, input.Password);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(input);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : "/");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => View();
}
