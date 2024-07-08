using Microsoft.AspNetCore.Mvc;
using TodoAPP.Context;
using TodoApp.ViewModels;
using TodoApp.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace TodoApp.Controllers
{
    public class AccountController : Controller
    {
        TodoContext context;
        public AccountController()
        {
            context = new TodoContext();
        }

        public IActionResult Register()
        {
            return View();
        }

        public ActionResult SubmitRegister(CreateUser user)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }
            var newUser = new User()
            {
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
                Role = user.Role,
            };
            context.Users.Add(newUser);
            context.SaveChanges();

            return RedirectToAction("Index", "Todo");
        }
        public ActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> SubmitLogin(LoginCred login)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", login);
            }

            var user = context.Users.FirstOrDefault(u => (u.Email == login.Email && u.Password == login.Password));
            if (user == null)
            {
                ModelState.AddModelError("", "Wrong Email Or Password");
                return View("Login", login);
            }

            var claimsIdentity = new ClaimsIdentity("Cookie");
            claimsIdentity.AddClaim(new Claim("Id", user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim("Role", user.Role.ToString()));

            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync("Cookie", principal, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(2),
            });

            return RedirectToAction("Index", "Todo");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("index", "Todo");
        }
    }
}