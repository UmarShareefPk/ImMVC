using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IM.Data;
using IM.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVCWeb.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Listing()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> AllUsers()
        {
            string token = User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value;
            var users = await userService.GetAllUsers(token);
            return Json(users);            
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ListingPage([FromQuery] string pageSize, [FromQuery]  string pageNumber, [FromQuery] string search)
        {
            string token = User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value;
            UserPages userPages = await userService.GetUsersWithPage(token, int.Parse(pageSize), int.Parse(pageNumber), search);
            return Json(userPages);
        }

        // GET: IncidentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IncidentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncidentController/Create
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

        // GET: IncidentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IncidentController/Edit/5
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

        // GET: IncidentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IncidentController/Delete/5
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
