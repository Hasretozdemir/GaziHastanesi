using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() { return View(); }
}