using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IM.Data;
using IM.Models;

namespace MVCWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;
        private readonly IUserService userService;
        public DashboardController(IDashboardService _dashboardService, IUserService _userService)
        {
            dashboardService = _dashboardService;
            userService = _userService;
        }
        // GET: DashboardController
        [Authorize]
        public async Task<ActionResult> Index()
        {
            string token = User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value;
            string userId = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;
            var kpiData = await dashboardService.GetKpi(token, userId);
            var overallData = await dashboardService.GetOverallWidget(token);
            var mostAssignedData = await dashboardService.GetMostAssignedToUsers(token);
            var lastFive = await dashboardService.GetLast5Incidents(token);
            var oldest5 = await dashboardService.GetOldest5UnresolvedIncidents(token);

            var allUsers = await userService.GetAllUsers(token);

            ViewBag.allUsers = allUsers;

            return View(new DashboardData { 
                KpiData = kpiData,
                OverallData = overallData,
                MostAssignedToUsersData = mostAssignedData,
                LastFive = lastFive, 
                Oldest5 = oldest5 
            });
        }

        [Authorize]
        //public ActionResult KpiBar()
        //{
        //    return PartialView( "_KpiBar" ,"Umar");
        //}

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
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

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
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

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
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
