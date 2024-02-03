using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POLYTEKS_ARGE.Models;

namespace POLYTEKS_ARGE.Controllers
{
    [AllowAnonymous]

    public class ChipsController : Controller
    {

        PTKS_ARGE1 db = new PTKS_ARGE1();
    
        public ActionResult Index()
        {
            return View(db.ARGE_DenemeChips.ToList());
        }
   
        public ActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_DenemeChips aRGE_DenemeChips)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_DenemeChips.Add(aRGE_DenemeChips);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Chips Eklenmiştir.";
                return RedirectToAction("Index");
            }

            return View(aRGE_DenemeChips);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_DenemeChips aRGE_DenemeChips = db.ARGE_DenemeChips.Find(id);
            if (aRGE_DenemeChips == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_DenemeChips);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ARGE_DenemeChips aRGE_DenemeChips)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_DenemeChips).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Chips Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            return View(aRGE_DenemeChips);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_DenemeChips aRGE_DenemeChips = db.ARGE_DenemeChips.Find(id);
            if (aRGE_DenemeChips == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_DenemeChips);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_DenemeChips aRGE_DenemeChips = db.ARGE_DenemeChips.Find(id);
            db.ARGE_DenemeChips.Remove(aRGE_DenemeChips);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Chips Silinmiştir.";
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
