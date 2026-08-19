using F1.Extensions;
using F1.Models;
using F1.Models.DTOs;
using F1.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace F1.Controllers
{
    public class ManagedController : Controller
    {
        private readonly IRepositoryF1 _repo;
        private readonly IConfiguration _configuration;

        public ManagedController(IRepositoryF1 repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public IActionResult LogIn(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> LogIn([FromForm] LoginRequest model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            UserPlayer? user = await _repo.LogIn(model.Email, model.Password);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                return View(model);
            }

            bool isAdmin = string.Equals(user.Email, _configuration["Admin:Email"], StringComparison.OrdinalIgnoreCase);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new(ClaimTypes.Name, user.Nickname),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, isAdmin ? "Admin" : "Player")
            };

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.Session.SetObject(isAdmin ? "Admin" : "Usuario", user);

            return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? LocalRedirect(returnUrl)
                : RedirectToAction("Index", "F1");
        }

        public IActionResult ErrorAcceso() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "F1");
        }
    }
}
