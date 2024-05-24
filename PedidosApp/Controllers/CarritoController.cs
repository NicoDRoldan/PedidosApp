using Microsoft.AspNetCore.Mvc;
using PedidosApp.Models;

namespace PedidosApp.Controllers
{
    public class CarritoController : Controller
    {
        private List<CarritoModel> ListaCarrito()
        {
            return new List<CarritoModel>
            {
                new CarritoModel { Id_Articulo = 1, NombreArticulo = "Producto 1", Cantidad = 2, PrecioUnitario = 10.0m },
                new CarritoModel { Id_Articulo = 2, NombreArticulo = "Producto 2", Cantidad = 1, PrecioUnitario = 20.0m }
            };
        }

        public IActionResult Index()
        {
            var carrito = ListaCarrito();
            return View(carrito);
        }

        public IActionResult _CarritoPartial()
        {
            var carrito = ListaCarrito();
            return View();
        }
    }
}
