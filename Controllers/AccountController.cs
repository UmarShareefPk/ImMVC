using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IM.Models;
using IM.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MVCWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        public AccountController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
               return RedirectToAction("Index", "Dashboard");
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");

            //return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel user)
        {
           UserLogin userLogin =  await userService.Authenticate(user.Username, user.Password);
            if(userLogin is not null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("userName", userLogin.Username));
                claims.Add(new Claim("firstName", userLogin.user.FirstName));
                claims.Add(new Claim("lastName", userLogin.user.LastName));
                claims.Add(new Claim("Token", userLogin.Token));
                claims.Add(new Claim("userId", userLogin.user.Id));
                claims.Add(new Claim("email", userLogin.user.Email));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var items = new Dictionary<string, string>();
                items.Add(".AuthScheme", CookieAuthenticationDefaults.AuthenticationScheme);

                var properties = new AuthenticationProperties(items);
                await HttpContext.SignInAsync(claimsPrincipal, properties);

                return RedirectToAction("Index", "Dashboard");
            }

            TempData["loginError"] = "Invalid username or Password";
            return View(user);
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
