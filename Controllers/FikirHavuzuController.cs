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
    public class FikirHavuzuController : Controller
    {
        PTKS_ARGE1 db = new PTKS_ARGE1();


        public ActionResult Index()
        {
            var model = db.ARGE_FikirHavuzu.OrderByDescending(a => a.Tarih).ToList();
            return View(model);




        }
        //public ActionResult Index1()
        //{
        //    var model = db.ARGE_FikirHavuzu.OrderByDescending(a => a.Tarih).ToList();
        //    return View(model);




        //}
        public ActionResult Index1(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_FikirHavuzu.OrderByDescending(a => a.Tarih).ToList().ToPagedList(page, recordsPerPage);
            return View(list);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_FikirHavuzu aRGE_FikirHavuzu = db.ARGE_FikirHavuzu.Find(id);
            if (aRGE_FikirHavuzu == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_FikirHavuzu);
        }


        public ActionResult Create()
        {
            ViewBag.GuncelDurumId = new SelectList(db.ARGE_FikirHavuzuGuncelDurum, "ID", "FikirHavuzuGuncelDurum");
            ViewBag.OnayDurumId = new SelectList(db.ARGE_FikirHavuzuOnayDurum, "ID", "FikirHavuzuDurum");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_FikirHavuzu aRGE_FikirHavuzu)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_FikirHavuzu.Add(aRGE_FikirHavuzu);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Fikir Eklenmiştir.";
                return RedirectToAction("Index1");
            }
            ViewBag.GuncelDurumId = new SelectList(db.ARGE_FikirHavuzuGuncelDurum, "ID", "FikirHavuzuGuncelDurum", aRGE_FikirHavuzu.GuncelDurumId);
            ViewBag.OnayDurumId = new SelectList(db.ARGE_FikirHavuzuOnayDurum, "ID", "FikirHavuzuDurum", aRGE_FikirHavuzu.OnayDurumId);
            return View(aRGE_FikirHavuzu);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_FikirHavuzu aRGE_FikirHavuzu = db.ARGE_FikirHavuzu.Find(id);
            if (aRGE_FikirHavuzu == null)
            {
                return HttpNotFound();
            }
            ViewBag.GuncelDurumId = new SelectList(db.ARGE_FikirHavuzuGuncelDurum, "ID", "FikirHavuzuGuncelDurum", aRGE_FikirHavuzu.GuncelDurumId);
            ViewBag.OnayDurumId = new SelectList(db.ARGE_FikirHavuzuOnayDurum, "ID", "FikirHavuzuDurum", aRGE_FikirHavuzu.OnayDurumId);
            return View(aRGE_FikirHavuzu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_FikirHavuzu aRGE_FikirHavuzu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_FikirHavuzu).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Fikir Düzenlenmiştir.";
                return RedirectToAction("Index1");
            }
            ViewBag.GuncelDurumId = new SelectList(db.ARGE_FikirHavuzuGuncelDurum, "ID", "FikirHavuzuGuncelDurum", aRGE_FikirHavuzu.GuncelDurumId);
            ViewBag.OnayDurumId = new SelectList(db.ARGE_FikirHavuzuOnayDurum, "ID", "FikirHavuzuDurum", aRGE_FikirHavuzu.OnayDurumId);
            return View(aRGE_FikirHavuzu);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_FikirHavuzu aRGE_FikirHavuzu = db.ARGE_FikirHavuzu.Find(id);
            if (aRGE_FikirHavuzu == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_FikirHavuzu);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_FikirHavuzu aRGE_FikirHavuzu = db.ARGE_FikirHavuzu.Find(id);
            db.ARGE_FikirHavuzu.Remove(aRGE_FikirHavuzu);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Fikir Silinmiştir.";
            return RedirectToAction("Index1");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult FikirHavuzuRapor()
        {
            return View(new FikirHavuzuModel());
        }
        [HttpPost]
        public ActionResult FikirHavuzuRapor(FikirHavuzuModel model)
        {


            return FikirHavuzuModeliOlustur(model);
        }


        [HttpPost]
        public FileResult FikirHavuzuModeliOlustur(FikirHavuzuModel gelenModel)
        {

            var model = new FikirHavuzuModel()
            {

            };

            model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
            model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;

            if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
            {
                model.FikirHavuzuRapors = db.ARGE_FikirHavuzu
                    .Where(a => a.Tarih >= (model.BaslangicTarihiDateTime) && a.Tarih <= (model.BitisTarihiDateTime)).OrderBy(a => a.Tarih).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                     new DataColumn("FİKİR SAHİBİ"),
            new DataColumn("FİKİR KONUSU"),
                new DataColumn("GÜNCEL DURUM"),
                    new DataColumn("SON DURUM"),
                         new DataColumn("TARİH"),




            });
            var liste = from list in db.ARGE_FikirHavuzu.OrderByDescending(a => a.Tarih) select list;



            if (model.FikirHavuzuRapors != null)
            {

                foreach (var list in model.FikirHavuzuRapors)
                {
                    dt.Rows.Add(list.FikirSahibi, list.FikirKonusu, list.ARGE_FikirHavuzuGuncelDurum.FikirHavuzuGuncelDurum, list.SonDurum,
                        list.Tarih.GetValueOrDefault().ToString("dd.MM.yyyy"));
                }
                dt.Rows.Add("F-32.05/00");

            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "FikirHavuzu");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-FikirHavuzu" + ".xlsx");
                }


            }

        }
        [HttpPost]
        public FileResult FikirHavuzuRaporsFiltresiz(ARGE_FikirHavuzu models)
        {

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                     new DataColumn("FİKİR SAHİBİ"),
            new DataColumn("FİKİR KONUSU"),
                new DataColumn("GÜNCEL DURUM"),
                    new DataColumn("SON DURUM"),
                         new DataColumn("TARİH"),




            });
            var liste = from list in db.ARGE_FikirHavuzu.OrderByDescending(a=>a.Tarih) select list;



          

                foreach (var list in liste)
                {
                    dt.Rows.Add(list.FikirSahibi, list.FikirKonusu, list.ARGE_FikirHavuzuGuncelDurum.FikirHavuzuGuncelDurum, list.SonDurum,
                        list.Tarih.GetValueOrDefault().ToString("dd.MM.yyyy"));
                }
            dt.Rows.Add("F-32.05/00");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "FikirHavuzu");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-FikirHavuzu" + ".xlsx");
                }


            }
   


           


        }
        //public ActionResult FikirHavuzuRapor()
        //{
        //    return View(new FikirHavuzu());
        //}


        //public FileResult PaketlemeTefrikModeliOlustur(FikirHavuzu gelenModel)
        //{

        //    var model = new FikirHavuzu()
        //    {

        //    };

        //    model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
        //    model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;
        //    if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
        //    {
        //        model.TefrikRaporOzetItems = db.ARGE_FikirHavuzu
        //            .Where(a => a.Tarih >= (model.BaslangicTarihiDateTime) && a.Tarih <= (model.BitisTarihiDateTime)).OrderByDescending(a => a.Tarih).ToList();
        //    }
        //    else
        //    {
        //        ViewBag.Error = "VERİ YOK";
        //    }
        //    DataTable dt = new DataTable("Grid");
        //    dt.Columns.AddRange(new DataColumn[5]{
        //        new DataColumn("FİKİR SAHİBİ"),
        //        new DataColumn("FİKİR KONUSU"),
        //            new DataColumn("GÜNCEL DURUM"),
        //                new DataColumn("SON DURUM"),
        //                     new DataColumn("TARİH"),





        //    });
        //    var liste = from list in db.ARGE_FikirHavuzu.ToList() select list;

        //    var raporViews = db.ARGE_FikirHavuzu.FirstOrDefault(a => a.Tarih >= (model.BaslangicTarihiDateTime) && a.Tarih <= (model.BitisTarihiDateTime));

        //    if (raporViews != null)
        //    {
        //        foreach (var list in liste)
        //        {
        //            dt.Rows.Add(list.FikirSahibi, list.FikirKonusu, list.GuncelDurum, list.SonDurum,
        //                list.Tarih.Value.ToString("dd.MM.yyyy"));
        //        }

        //    }
        //    else if (raporViews == null)
        //    {
        //        ViewBag.Error = "VERİ YOK";
        //    }

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);


        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "FikirHavuzu" + ".xlsx");
        //        }


        //    }

        //}

        //[HttpPost]
        //public ActionResult FikirHavuzuRapor(FikirHavuzu model)
        //{


        //    return PaketlemeTefrikModeliOlustur(model);
        //}
    }
}
