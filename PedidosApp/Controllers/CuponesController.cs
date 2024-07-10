using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PedidosApp.Models;

namespace PedidosApp.Controllers
{
    public class CuponesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CuponesController (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var wsCuponesClient = _httpClientFactory.CreateClient("WSCuponesClient");
                wsCuponesClient.DefaultRequestHeaders.Accept.Clear();
                wsCuponesClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers
                    .MediaTypeWithQualityHeaderValue("application/json"));

                var response = await wsCuponesClient.GetAsync("api/Cupones");

                if (response.IsSuccessStatusCode)
                {
                    var tiposCupones = new List<string> { "PROMO", "PRECIOFIJO" };

                    var cuponJson = await response.Content.ReadAsStringAsync();
                    var cuponModel = JsonConvert.DeserializeObject<List<CuponModel>>(cuponJson)
                        .Where(c => tiposCupones.Contains(c.TipoCupon));

                    return View(cuponModel);
                }
            }
            catch (Exception ex)
            {
                TempData["OriginalUrl"] = Request.Path + Request.QueryString;
                TempData["Error"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

            TempData["OriginalUrl"] = Request.Path + Request.QueryString;
            TempData["Error"] = "Error al realizar la conexión.";
            return RedirectToAction("Error", "Home");
        }
    }
}
