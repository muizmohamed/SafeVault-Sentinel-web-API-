using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Controllers;

[Authorize(Roles = "Admin")]
public sealed class AdminController : Controller
{
    public IActionResult Index() => View();
}
