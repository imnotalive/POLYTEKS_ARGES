using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using DatatableJS.Data;
using System.Web;
using System.Web.Mvc;
using POLYTEKS_ARGE.Models;

namespace POLYTEKS_ARGE.Controllers
{
    [AllowAnonymous]
    public class KesitTipiController : BaseController
    {
        //PTKS_ARGE db = new PTKS_ARGE();

    
        public ActionResult Index()
        {
            return View(db.ARGE_KesitTipi.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_KesitTipi aRGE_KesitTipi)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_KesitTipi.Add(aRGE_KesitTipi);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Kesit Tipi Eklenmiştir.";
                return RedirectToAction("Index");
            }

            return View(aRGE_KesitTipi);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_KesitTipi aRGE_KesitTipi = db.ARGE_KesitTipi.Find(id);
            if (aRGE_KesitTipi == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_KesitTipi);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_KesitTipi aRGE_KesitTipi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_KesitTipi).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Kesit Tipi Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            return View(aRGE_KesitTipi);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_KesitTipi aRGE_KesitTipi = db.ARGE_KesitTipi.Find(id);
            if (aRGE_KesitTipi == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_KesitTipi);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_KesitTipi aRGE_KesitTipi = db.ARGE_KesitTipi.Find(id);
            db.ARGE_KesitTipi.Remove(aRGE_KesitTipi);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Kesit Tipi Silinmiştir.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       

    }

    
}
