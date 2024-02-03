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
using DatatableJS.Data;

namespace POLYTEKS_ARGE.Controllers
{
    [AllowAnonymous]
    public class MaliyetController : Controller
    {
        PTKS_ARGE1 db = new PTKS_ARGE1();

        //public ActionResult Index()
        //{
        //    var aRGE_Maliyet = db.ARGE_Maliyet.OrderByDescending(a=>a.MaliyetTarih).Include(a => a.ARGE_Proje);
        //    return View(aRGE_Maliyet.ToList());
        //}
        //[HttpPost]
        //public ActionResult Index(DateTime? ProjeBaslangicTarihi, DateTime? ProjeBitisTarihi)
        //{

        //    var model = db.ARGE_Maliyet.ToList();



        //    return View(model);
        //}


        public ActionResult GetDataResult()
        {
            var result = db.ARGE_Maliyet.ToList();
            return View(result);
        }
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_Maliyet.OrderByDescending(a => a.MaliyetTarih).ToPagedList(page, recordsPerPage);
            return View(list);

        }

        public ActionResult Create()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_Maliyet aRGE_Maliyet)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_Maliyet.Add(aRGE_Maliyet);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Maliyet Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_Maliyet.ProjeId);
            return View(aRGE_Maliyet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Maliyet aRGE_Maliyet = db.ARGE_Maliyet.Find(id);
            if (aRGE_Maliyet == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_Maliyet.ProjeId);
            return View(aRGE_Maliyet);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_Maliyet aRGE_Maliyet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_Maliyet).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Maliyet Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_Maliyet.ProjeId);
            return View(aRGE_Maliyet);
        }

  
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Maliyet aRGE_Maliyet = db.ARGE_Maliyet.Find(id);
            if (aRGE_Maliyet == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Maliyet);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_Maliyet aRGE_Maliyet = db.ARGE_Maliyet.Find(id);
            db.ARGE_Maliyet.Remove(aRGE_Maliyet);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Maliyet Silinmiştir.";
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
        //public ActionResult PaketlemeTefrikRapor()
        //{
        //    return View(new Maliyet() );
        //}

        //[HttpPost]
        //public ActionResult PaketlemeTefrikRapor(Maliyet model)
        //{


        //    return PaketlemeTefrikModeliOlustur(model);
        //}
        //[HttpPost]
        //public FileResult PaketlemeTefrikModeliOlustur(Maliyet gelenModel)
        //{

        //    var model = new Maliyet()
        //    {

        //    };

        //    model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
        //    model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;

        //    if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
        //    {
        //        model.TefrikRaporOzetItems = db.ARGE_Maliyet.Where(a => a.MaliyetTarih >= (model.BaslangicTarihiDateTime) && a.MaliyetTarih <= (model.BitisTarihiDateTime)).OrderByDescending(a => a.MaliyetTarih).ToList();
        //    }
        //    else
        //    {
        //        ViewBag.Error = "VERİ YOK";
        //    }
        //    DataTable dt = new DataTable("Grid");
        //    dt.Columns.AddRange(new DataColumn[7]{
        //        new DataColumn("İlgili Proje Kodu"),
        //        new DataColumn("Personel"),
        //          new DataColumn("Firma Adı"),
        //            new DataColumn("Harcama Adı"),
        //                new DataColumn("Tarih"),
        //                     new DataColumn("KDVsiz Tutar"),
        //                    new DataColumn("KDVli Tutar"),




        //    });
        //    var liste = from list in db.ARGE_Maliyet.ToList() select list;

        //    //var raporViews = db.ARGE_Maliyet.FirstOrDefault(a => a.MaliyetTarih >= (model.BaslangicTarihiDateTime) && a.MaliyetTarih <= (model.BitisTarihiDateTime));

        //    if (model.TefrikRaporOzetItems != null)
        //    {
        //        foreach (var list in liste)
        //        {
        //            dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.Personel, list.Firma, list.MaliyetAdi,
        //                list.MaliyetTarih.GetValueOrDefault().ToString("dd.MM.yyyy"),list.KDVsizMiktar,list.KDVliMiktar);
        //        }

        //    }
        //    else
        //    {
        //        ViewBag.Error = "VERİ YOK";
        //    }

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);


        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "DisHizmetAlimiveDisHarcamalar" + ".xlsx");
        //        }


        //    }

        //}
        public ActionResult PaketlemeTefrikRapor()
        {
            return View(new MaliyetModel());
        }
        [HttpPost]
        public ActionResult PaketlemeTefrikRapor(MaliyetModel model)
        {


            return PaketlemeTefrikModeliOlustur(model);
        }


        [HttpPost]
        public FileResult PaketlemeTefrikModeliOlustur(MaliyetModel gelenModel)
        {

            var model = new MaliyetModel()
            {

            };

            model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
            model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;

            if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
            {
                model.MaliyetRaporOzel = db.ARGE_Maliyet
                    .Where(a => a.MaliyetTarih >= (model.BaslangicTarihiDateTime) && a.MaliyetTarih <= (model.BitisTarihiDateTime)).OrderBy(a => a.MaliyetTarih).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("İlgili Proje Kodu"),
                      new DataColumn("Personel"),
                        new DataColumn("Firma Adı"),
                new DataColumn("Harcama Adı"),
                           new DataColumn("Tarih"),
                    new DataColumn("KDVsiz Tutar"),
                        new DataColumn("KDVli Tutar"),
                      



            });
            var liste = from list in db.ARGE_Maliyet.OrderByDescending(a => a.MaliyetTarih) select list;



            if (model.MaliyetRaporOzel != null)
            {

                foreach (var list in model.MaliyetRaporOzel)
                {
                    dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.Personel,list.Firma,list.MaliyetAdi,
                        list.MaliyetTarih.GetValueOrDefault().ToString("dd.MM.yyyy"), list.KDVsizMiktar.GetValueOrDefault().ToString(), list.KDVliMiktar.GetValueOrDefault().ToString());
                }

            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "DisHizmetAlimiveDisHarcamalar");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "DisHizmetAlimiveDisHarcamalar" + ".xlsx");
                }


            }

        }

    }
}
