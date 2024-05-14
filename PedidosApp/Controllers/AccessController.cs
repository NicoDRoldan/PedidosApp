using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PedidosApp.Data;
using PedidosApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace PedidosApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly PedidosAppContext _context;

        public AccessController(PedidosAppContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            
            if (claimsUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login (LoginModel loginModel)
        {
            var user = _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Usuario == loginModel.User && u.Clave == loginModel.Password)
                .FirstOrDefaultAsync();

            //if (user != null)
            //{
            //    List<Claim> claims = new List<Claim>()
            //        {
            //            new Claim(ClaimTypes.NameIdentifier, loginModel.User),
            //            new Claim(ClaimTypes.Role, user.ro),
            //            new Claim("userid", user.Id_Usuario.ToString())
            //        };

            //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
            //        CookieAuthenticationDefaults.AuthenticationScheme);

            //    AuthenticationProperties properties = new AuthenticationProperties()
            //    {
            //        AllowRefresh = true,
            //        IsPersistent = modelLogin.KeepLoggedIn,
            //    };

            //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            //        new ClaimsPrincipal(claimsIdentity), properties);

            //    return RedirectToAction("Index", "Ventas");

            //}

            ModelState.AddModelError(string.Empty, "Por favor, corroborar los datos ingresados.");
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}