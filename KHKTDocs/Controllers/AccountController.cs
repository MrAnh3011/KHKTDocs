using ApplicationCore.Interfaces.Services;
using KHKTDocs.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KHKTDocs.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRoleService _userRoleService;

        public AccountController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [Route("/postusersession")]
        public async Task<IActionResult> PostUserSession(LoginViewModel loginViewModel)
        {
            try
            {
                ClaimsIdentity _claimsIdentity;
                var listClaim = new List<Claim>();
                var result = await _userRoleService.GetUserRoleByUserName(loginViewModel.Username);

                listClaim.Add(new Claim(ClaimTypes.Name, loginViewModel.DisplayName));
                listClaim.Add(new Claim("UserName", loginViewModel.Username));

                if (result != null)
                {
                    if (result.isaccess == 1)
                        listClaim.Add(new Claim(ClaimTypes.Role, "Access"));
                    if (result.isapprove == 1)
                        listClaim.Add(new Claim(ClaimTypes.Role, "Approve"));
                    if (result.isdelete == 1)
                        listClaim.Add(new Claim(ClaimTypes.Role, "Delete"));
                    if (result.isadmin == 1)
                        listClaim.Add(new Claim(ClaimTypes.Role, "Admin"));
                    if (result.issuperadmin == 1)
                        listClaim.Add(new Claim(ClaimTypes.Role, "SuperAdmin"));
                }
                _claimsIdentity = new ClaimsIdentity(listClaim, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(_claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, _claimsPrincipal);

                return Json(new { status = "success", message = "Login success" });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
