using Data.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginDTO model)
        {
            return View(model);

        }


        public IActionResult Register()
        {
            return View();
        }
    }
}
