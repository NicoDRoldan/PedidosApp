using Microsoft.AspNetCore.Mvc;

namespace PedidosApp.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
