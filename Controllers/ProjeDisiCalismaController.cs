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
    public class ProjeDisiCalismaController : Controller
    {

        PTKS_ARGE1 db = new PTKS_ARGE1();
        //public ActionResult Index()
        //{
        //    var aRGE_ProjeDisiCalisma = db.ARGE_ProjeDisiCalisma.Include(a => a.ARGE_Personel);
        //    return View(aRGE_ProjeDisiCalisma.ToList());
        //}
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_ProjeDisiCalisma.OrderByDescending(a => a.Tarih).ToPagedList(page, recordsPerPage);
            return View(list);

        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeDisiCalisma aRGE_ProjeDisiCalisma = db.ARGE_ProjeDisiCalisma.Find(id);
            if (aRGE_ProjeDisiCalisma == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_ProjeDisiCalisma);
        }

        
        public ActionResult Create()
        {
            ViewBag.ProjeSorumluId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_ProjeDisiCalisma aRGE_ProjeDisiCalisma)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_ProjeDisiCalisma.Add(aRGE_ProjeDisiCalisma);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni AR-GE Proje Dışı Çalışma Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeSorumluId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_ProjeDisiCalisma.ProjeSorumluId);
            return View(aRGE_ProjeDisiCalisma);
        }

  
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeDisiCalisma aRGE_ProjeDisiCalisma = db.ARGE_ProjeDisiCalisma.Find(id);
            if (aRGE_ProjeDisiCalisma == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjeSorumluId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_ProjeDisiCalisma.ProjeSorumluId);
            return View(aRGE_ProjeDisiCalisma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_ProjeDisiCalisma aRGE_ProjeDisiCalisma)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_ProjeDisiCalisma).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "AR-GE Proje Dışı Çalışma Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            ViewBag.ProjeSorumluId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_ProjeDisiCalisma.ProjeSorumluId);
            return View(aRGE_ProjeDisiCalisma);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeDisiCalisma aRGE_ProjeDisiCalisma = db.ARGE_ProjeDisiCalisma.Find(id);
            if (aRGE_ProjeDisiCalisma == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_ProjeDisiCalisma);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_ProjeDisiCalisma aRGE_ProjeDisiCalisma = db.ARGE_ProjeDisiCalisma.Find(id);
            db.ARGE_ProjeDisiCalisma.Remove(aRGE_ProjeDisiCalisma);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "AR-GE Proje Dışı Çalışma Silinmiştir.";
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
        public FileResult ProjeDisiCalismaRapors(ARGE_ProjeDisiCalisma personel)
        {


            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("PROJE KONUSU"),
                      new DataColumn("AÇIKLAMA"),
                                      new DataColumn("TARİH"),
                                 new DataColumn("GÜNCELLEMELER"),
                                        new DataColumn("NUMUNELER"),
                                               new DataColumn("PROJE SORUMLUSU"),
            });
            var liste = from list in db.ARGE_ProjeDisiCalisma.OrderByDescending(a => a.Tarih) select list;

            foreach (var list in liste)
            {
                dt.Rows.Add(list.ProjeKonusu, list.Aciklama, list.Tarih, list.Guncelleme,list.Numuneler,list.ARGE_Personel.AdSoyad);

            }
            dt.Rows.Add("F-32.09C/01");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "ProjeDisiCalisma");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-ProjeDisiCalisma" + ".xlsx");
                }


            }

        }
    }
}
