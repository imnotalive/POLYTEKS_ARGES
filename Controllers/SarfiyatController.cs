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
    public class SarfiyatController : Controller
    {
        PTKS_ARGE1 db = new PTKS_ARGE1();

        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_Sarfiyat.ToList().ToPagedList(page, recordsPerPage);
            return View(list);
          
        }

        [HttpPost]
        public ActionResult Index(DateTime? ProjeBaslangicTarihi, DateTime? ProjeBitisTarihi)
        {

            var model = db.ARGE_Sarfiyat.ToList();
         


            return View(model);
        }
        public ActionResult Create()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKisaAd");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_Sarfiyat aRGE_Sarfiyat)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_Sarfiyat.Add(aRGE_Sarfiyat);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Sarfiyat Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKisaAd", aRGE_Sarfiyat.ProjeId);
            return View(aRGE_Sarfiyat);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Sarfiyat aRGE_Sarfiyat = db.ARGE_Sarfiyat.Find(id);
            if (aRGE_Sarfiyat == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKisaAd", aRGE_Sarfiyat.ProjeId);
            return View(aRGE_Sarfiyat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_Sarfiyat aRGE_Sarfiyat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_Sarfiyat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Sarfiyat Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKisaAd", aRGE_Sarfiyat.ProjeId);
            return View(aRGE_Sarfiyat);
        }

  
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Sarfiyat aRGE_Sarfiyat = db.ARGE_Sarfiyat.Find(id);
            if (aRGE_Sarfiyat == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Sarfiyat);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_Sarfiyat aRGE_Sarfiyat = db.ARGE_Sarfiyat.Find(id);
            db.ARGE_Sarfiyat.Remove(aRGE_Sarfiyat);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Sarfiyat Silinmiştir.";
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

        public ActionResult SarfiyatRapor()
        {
            return View(new SarfiyatModel());
        }
        [HttpPost]
        public ActionResult SarfiyatRapor(SarfiyatModel model)
        {


            return ArgeSarfiyatModeliOlustur(model);
        }


        [HttpPost]
        public FileResult ArgeSarfiyatModeliOlustur(SarfiyatModel gelenModel)
        {

            var model = new SarfiyatModel()
            {

            };

            model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
            model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;

            if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
            {
                model.ArgeSarfiyatRapor = db.ARGE_Sarfiyat
                    .Where(a => a.SarfiyatTarih >= (model.BaslangicTarihiDateTime) && a.SarfiyatTarih <= (model.BitisTarihiDateTime)).OrderBy(a => a.SarfiyatTarih).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                 new DataColumn("İlgili Proje Kodu"),
                    new DataColumn("Proje Kisa Ad"),
                        new DataColumn("Sarfiyat Tarihi"),
                            new DataColumn("Sarfiyat Cinsi"),
                                 new DataColumn("Miktar"),
                                new DataColumn("Birim"),





            });
            var liste = from list in db.ARGE_Sarfiyat select list;



            if (model.ArgeSarfiyatRapor != null)
            {

                foreach (var list in model.ArgeSarfiyatRapor)
                {
                    dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.ARGE_Proje.ProjeKisaAd,
                        list.SarfiyatTarih.GetValueOrDefault().ToString("dd.MM.yyyy"), list.SarfiyatCinsi,list.Miktar,list.Birim);
                }

            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Sarfiyat");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                                                          //W_WorkSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;/*.Alignment.SetVertical(XLAlignmentVerticalValues.Center);*/
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE Sarfiyatlar" + ".xlsx");
                }


            }

        }

    }
}

