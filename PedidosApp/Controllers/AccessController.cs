using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PedidosApp.Data;
using PedidosApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using PedidosApp.Interfaces;

namespace PedidosApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly PedidosAppContext _context;
        private readonly IAccessService _accessService;

        public AccessController(PedidosAppContext context, IAccessService accessService)
        {
            _context = context;
            _accessService = accessService;
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

        [HttpPost]
        public async Task<IActionResult> SendEmailRecover(string usuario)
        {
            var usuarioEntity = await _context.Usuarios
                .Where(u => u.Usuario == usuario)
                .FirstOrDefaultAsync();

            if(usuarioEntity == null)
            {
                return Ok(new { success = false, message = "Error, no existe el usuario." });
            }

            string result = await _accessService.SendEmailRecover(usuarioEntity.Email, usuarioEntity.Usuario);

            if(result.Contains("Código enviado correctamente"))
            {
                return Ok(new { success = true, message = result });
            }
            else
            {
                return BadRequest(new { success = false, message = result });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateUser(string usuario, string codigoRecuperacion)
        {
            var validationResult = await _accessService.ValidateUser(usuario, codigoRecuperacion);

            if(validationResult.ContainsKey("success") && (bool)validationResult["success"])
            {
                return Ok(validationResult);
            }
            else
            {
                return Ok(validationResult);
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult SendRecoverCode()
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