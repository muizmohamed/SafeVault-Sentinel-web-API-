using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Models;

namespace SafeVault.Controllers;

public sealed class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Index() => View(new HomeViewModel(User.Identity?.Name, null));

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult Feedback(FeedbackInput input)
    {
        if (!ModelState.IsValid)
            return View("Index", new HomeViewModel(User.Identity?.Name, null));

        // Do not Html.Raw this value in the view. Razor output encoding is the XSS defense.
        return View("Index", new HomeViewModel(User.Identity?.Name, input.Message));
    }
}
