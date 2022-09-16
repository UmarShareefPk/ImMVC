using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IM.Data;
using IM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using IM.Helper;

namespace MVCWeb.Controllers
{
    public class IncidentController : Controller
    {

        private readonly IIncidentService incidentService;
        public IncidentController(IIncidentService _incidentService)
        {
            incidentService = _incidentService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Listing()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ListingPage([FromQuery] string pageSize, [FromQuery]  string pageNumber, [FromQuery] string search)
        {
            string token = User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value;
            IncidentPages incidentPages = await incidentService.GetIncidentsWithPage(token, int.Parse(pageSize), int.Parse(pageNumber), search);
            return Json(incidentPages);
        }

        // GET: IncidentController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            string token = User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value;
            Incident incident = await incidentService.GetIncidentById(token, id);
            return View(incident);
        }

        [HttpPost]
        public ActionResult Update([FromBody] IncidentUpdateModel model)
        {
            return Json(model);
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
