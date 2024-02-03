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
    public class PersonelDisaridaGecirilenSureController : Controller
    {
        PTKS_ARGE1 db = new PTKS_ARGE1();

        //public ActionResult Index()
        //{
        //    var aRGE_PersonelDisaridaGecirilenSure = db.ARGE_PersonelDisaridaGecirilenSure.Include(a => a.ARGE_Personel).Include(a => a.ARGE_Proje).OrderByDescending(a=>a.CikisTarih);
        //    return View(aRGE_PersonelDisaridaGecirilenSure.ToList());
        //}

        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_PersonelDisaridaGecirilenSure.Include(a => a.ARGE_Personel).Include(a => a.ARGE_Proje).OrderByDescending(a => a.CikisTarih).ToPagedList(page, recordsPerPage);
            return View(list);

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_PersonelDisaridaGecirilenSure aRGE_PersonelDisaridaGecirilenSure = db.ARGE_PersonelDisaridaGecirilenSure.Find(id);
            if (aRGE_PersonelDisaridaGecirilenSure == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_PersonelDisaridaGecirilenSure);
        }

      
        public ActionResult Create()
        {
            ViewBag.PersonelId = new SelectList(db.ARGE_Personel.OrderBy(a => a.AdSoyad), "PersonelId", "AdSoyad");
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje.OrderBy(a=>a.ProjeAdi), "ID", "ProjeAdi");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_PersonelDisaridaGecirilenSure aRGE_PersonelDisaridaGecirilenSure)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_PersonelDisaridaGecirilenSure.Add(aRGE_PersonelDisaridaGecirilenSure);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Personel Dışarı Geçirilen Süre Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.PersonelId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_PersonelDisaridaGecirilenSure.PersonelId);
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_PersonelDisaridaGecirilenSure.ProjeId);
            return View(aRGE_PersonelDisaridaGecirilenSure);
        }

  
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_PersonelDisaridaGecirilenSure aRGE_PersonelDisaridaGecirilenSure = db.ARGE_PersonelDisaridaGecirilenSure.Find(id);
            if (aRGE_PersonelDisaridaGecirilenSure == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonelId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_PersonelDisaridaGecirilenSure.PersonelId);
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_PersonelDisaridaGecirilenSure.ProjeId);
            return View(aRGE_PersonelDisaridaGecirilenSure);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ARGE_PersonelDisaridaGecirilenSure aRGE_PersonelDisaridaGecirilenSure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_PersonelDisaridaGecirilenSure).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Dışarıda Geçirilen Süre Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            ViewBag.PersonelId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_PersonelDisaridaGecirilenSure.PersonelId);
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_PersonelDisaridaGecirilenSure.ProjeId);
            return View(aRGE_PersonelDisaridaGecirilenSure);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_PersonelDisaridaGecirilenSure aRGE_PersonelDisaridaGecirilenSure = db.ARGE_PersonelDisaridaGecirilenSure.Find(id);
            if (aRGE_PersonelDisaridaGecirilenSure == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_PersonelDisaridaGecirilenSure);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_PersonelDisaridaGecirilenSure aRGE_PersonelDisaridaGecirilenSure = db.ARGE_PersonelDisaridaGecirilenSure.Find(id);
            db.ARGE_PersonelDisaridaGecirilenSure.Remove(aRGE_PersonelDisaridaGecirilenSure);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Dışarıda Geçirilen Süre Silinmiştir.";
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
     
        public ActionResult PersonelRapor()
        {
            return View(new PersonelDisaridaGecirilenSureModel());
        }
        [HttpPost]
        public ActionResult PersonelRapor(PersonelDisaridaGecirilenSureModel model)
        {


            return PersonelRaporModeliOlustur(model);
        }


        [HttpPost]
        public FileResult PersonelRaporModeliOlustur(PersonelDisaridaGecirilenSureModel gelenModel)
        {

            var model = new PersonelDisaridaGecirilenSureModel()
            {

            };

            model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
            model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;

            if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
            {
                model.PersonelSureModel = db.ARGE_PersonelDisaridaGecirilenSure
                    .Where(a => a.CikisTarih >= (model.BaslangicTarihiDateTime) && a.CikisTarih <= (model.BitisTarihiDateTime)).OrderBy(a=>a.CikisTarih).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("İlgili Proje Kodu"),
                      new DataColumn("Personel"),
                new DataColumn("Tarih"),
                           new DataColumn("Cikis Saati"),
                    new DataColumn("Giris Saati"),
                        new DataColumn("Yer"),
                             new DataColumn("Dis Gorev Adi"),
                            new DataColumn("İcerik"),
                                                     


            });
            var liste = from list in db.ARGE_Proje select list;



            if (model.PersonelSureModel != null)
            {

                foreach (var list in model.PersonelSureModel)
                {
                    dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.ARGE_Personel.AdSoyad,
                        list.CikisTarih.GetValueOrDefault().ToString("dd.MM.yyyy"),list.CikisSaati,list.GirisSaati,list.Yer,list.DisGorevAdi,list.Aciklama);
                }

            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "PersonelDisaridaGecirilenSure");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "PersonelDisaridaGecirilenSure" + ".xlsx");
                }


            }

        }




    }
}
