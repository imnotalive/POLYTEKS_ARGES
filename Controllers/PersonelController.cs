using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using PagedList;
using POLYTEKS_ARGE.Models;

namespace POLYTEKS_ARGE.Controllers
{
    [AllowAnonymous]
    public class PersonelController : Controller
    {
        PTKS_ARGE1 db = new PTKS_ARGE1();


        public ActionResult Index()
        {
            var aRGE_Personel = db.ARGE_Personel;
            return View(aRGE_Personel.OrderBy(a=>a.AdSoyad).ToList().ToPagedList(1, 30));
        }

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_Personel aRGE_Personel)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_Personel.Add(aRGE_Personel);
                db.SaveChanges();
                TempData["AlertMessage"] = "AR-GE Personeli Eklenmiştir.";
                return RedirectToAction("Index");
            }

           
            return View(aRGE_Personel);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Personel aRGE_Personel = db.ARGE_Personel.Find(id);
            if (aRGE_Personel == null)
            {
                return HttpNotFound();
            }
         
            return View(aRGE_Personel);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_Personel aRGE_Personel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_Personel).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "AR-GE Personeli Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
          
            return View(aRGE_Personel);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Personel aRGE_Personel = db.ARGE_Personel.Find(id);
            if (aRGE_Personel == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Personel);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_Personel aRGE_Personel = db.ARGE_Personel.Find(id);
            db.ARGE_Personel.Remove(aRGE_Personel);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "AR-GE Personeli Silinmiştir.";
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
  
        [HttpPost]
        public FileResult PersonelModeliOlustur(ARGE_Personel personel)
        {

       
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("KULLANICI ADI"),
                      new DataColumn("AD-SOYAD"),
                                      new DataColumn("DURUM"),
                                 new DataColumn("E-MAİL"),
            });
            var liste = from list in db.ARGE_Personel.OrderBy(a=>a.AdSoyad) select list;

                foreach (var list in liste)
                {
                dt.Rows.Add(list.KullaniciAdi, list.AdSoyad, list.Durum, list.Email);
                      
                }

     

            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Personel");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE Personelleri" + ".xlsx");
                }


            }

        }

    }
}
