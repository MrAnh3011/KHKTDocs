﻿using KHKTDocs.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KHKTDocs.Controllers
{
    public class AccountController : Controller
    {
        //private ClaimsIdentity _claimsIdentity;
        //private ClaimsPrincipal _claimsPrincipal;
        //public AccountController(ClaimsIdentity claimsIdentity, ClaimsPrincipal claimsPrincipal)
        //{
        //    _claimsIdentity = claimsIdentity;
        //    _claimsPrincipal = claimsPrincipal;
        //}

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/postusersession")]
        public async Task<IActionResult> PostUserSession(LoginViewModel loginViewModel)
        {
            ClaimsIdentity _claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, loginViewModel.DisplayName),
                    new Claim(ClaimTypes.Role, "User")
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(_claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, _claimsPrincipal);

            return Json(new { status = "success", message = "Login success" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}