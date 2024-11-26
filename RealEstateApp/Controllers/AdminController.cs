using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Repositories;

namespace RealEstateApp.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    
    }
}
