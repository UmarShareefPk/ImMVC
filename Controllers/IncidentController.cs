using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IM.Data;
using IM.Models;

namespace MVCWeb.Controllers
{
    public class IncidentController : Controller
    {

        private readonly IIncidentService incidentService;
        public IncidentController(IIncidentService _incidentService)
        {
            incidentService = _incidentService;
        }

        [HttpGet]
        public ActionResult Listing()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> ListingPage([FromQuery] string pageSize, [FromQuery]  string pageNumber, [FromQuery] string search)
        {
            string token = User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value;
            IncidentPages incidentPages = await incidentService.GetIncidentsWithPage(token, int.Parse(pageSize), int.Parse(pageNumber), search);
            return Json(incidentPages);
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
