using Microsoft.AspNetCore.Mvc;

namespace PedidosApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}