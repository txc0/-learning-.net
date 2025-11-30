using Food_Waste_management.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Food_Waste_management.Controllers
{
    public class CollectRequestController : Controller
    {
        private WasteFoodDbEntities db = new WasteFoodDbEntities(); 

        // GET: CollectRequest
        public ActionResult Index()
        {
            var requests = db.CollectRequests
                .Include( r => r.Restaurant)
                .Include( r => r.Restaurant)
                .OrderByDescending(r => r.Restaurant.Name) .ToList();
            return View(requests);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(CollectRequest request)
        {
            if (ModelState.IsValid)
            {
                request.RequestDate = System.DateTime.Now;
                request.Status = "Pending";
                request.AssignedEmployeeId = null;

                db.CollectRequests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Name", request.RestaurantId);
            return View(request);

        }

        [HttpGet]
        public ActionResult Assign (int? id)
        {
            if (id == null) return new HttpStatusCodeResult(400);

            var request = db.CollectRequests.Find(id);
            if(request == null) return HttpNotFound();

            ViewBag.AssignEmployeeId = new SelectList(db.Employees, "Id", "Name", request.AssignedEmployeeId);
            return View(request);
        }

        [HttpPost]
        public ActionResult Assign(int id, int AssignedEmployeeId)
        {
            var request = db.CollectRequests.Find(id); if(request == null) return HttpNotFound();

            request.AssignedEmployeeId = AssignedEmployeeId;
            request.Status = "Accepted";

            db.Entry(request).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Complete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(400);

            var request = db.CollectRequests.Find(id);
            if (request == null) return HttpNotFound();

            return View(request);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Complete(int id, string DistributionNotes)
        {
            var request = db.CollectRequests.Find(id);
            if (request == null) return HttpNotFound();

            request.Status = "Completed";
            request.DistributionNotes = DistributionNotes;

            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }


}
