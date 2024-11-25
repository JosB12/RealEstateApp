using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //public async Task<IActionResult> LogOut()
        //{
        //    await _userService.SignOutAsync();
        //    HttpContext.Session.Remove("user");
        //    return RedirectToRoute(new { controller = "Home", action = "Index" });
        //}
    }
}
