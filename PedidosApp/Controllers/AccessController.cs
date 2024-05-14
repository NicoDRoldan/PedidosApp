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
            if(loginModel.User is null || loginModel.Password is null)
            {
                ModelState.AddModelError(string.Empty, "Por favor, corroborar los datos ingresados.");
                return View();
            }

            var user = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Usuario == loginModel.User)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Por favor, corroborar los datos ingresados.");
                return View();
            }

            bool Hash = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Clave);

            if (user != null && Hash)
            {
                List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, loginModel.User),
                        new Claim(ClaimTypes.Role, user.Rol.Nombre),
                        new Claim("userid", user.Usuario.ToString())
                    };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = loginModel.KeepLoggedIn,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Por favor, corroborar los datos ingresados.");
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult RecoverUser()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}