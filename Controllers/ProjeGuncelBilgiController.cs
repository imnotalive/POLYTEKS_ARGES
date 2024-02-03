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
    public class ProjeGuncelBilgiController : BaseController
    {

  
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_ProjeGuncelBilgi.OrderByDescending(a => a.ProjeGuncellemeTarih).ToPagedList(page, recordsPerPage);
            return View(list);

        }
        //public ActionResult Index()
        //{
        //    var aRGE_ProjeGuncelBilgi = db.ARGE_ProjeGuncelBilgi.OrderByDescending(a=>a.ProjeGuncellemeTarih).Include(a => a.ARGE_Proje);
        //    return View(aRGE_ProjeGuncelBilgi.ToList());
        //}

        //[HttpPost]
        //public ActionResult Index(DateTime? ProjeBaslangicTarihi, DateTime? ProjeBitisTarihi)
        //{

        //    var model = db.ARGE_ProjeGuncelBilgi.ToList();


        //    return View(model);
        //}
        public ActionResult Create()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_ProjeGuncelBilgi aRGE_ProjeGuncelBilgi)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_ProjeGuncelBilgi.Add(aRGE_ProjeGuncelBilgi);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni AR-GE Proje Güncel Bilgisi Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_ProjeGuncelBilgi.ProjeId);
            return View(aRGE_ProjeGuncelBilgi);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeGuncelBilgi aRGE_ProjeGuncelBilgi = db.ARGE_ProjeGuncelBilgi.Find(id);
            if (aRGE_ProjeGuncelBilgi == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_ProjeGuncelBilgi.ProjeId);
            return View(aRGE_ProjeGuncelBilgi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_ProjeGuncelBilgi aRGE_ProjeGuncelBilgi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_ProjeGuncelBilgi).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = aRGE_ProjeGuncelBilgi.ARGE_Proje.ProjeAdi +"Projesi Güncel Bilgisi Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_ProjeGuncelBilgi.ProjeId);
            return View(aRGE_ProjeGuncelBilgi);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeGuncelBilgi aRGE_ProjeGuncelBilgi = db.ARGE_ProjeGuncelBilgi.Find(id);
            if (aRGE_ProjeGuncelBilgi == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_ProjeGuncelBilgi);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_ProjeGuncelBilgi aRGE_ProjeGuncelBilgi = db.ARGE_ProjeGuncelBilgi.Find(id);
            db.ARGE_ProjeGuncelBilgi.Remove(aRGE_ProjeGuncelBilgi);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "AR-GE Projesi Güncel Bilgi Silinmiştir.";
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
    
        public PartialViewResult Listing()
        {


            var orders = db.ARGE_ProjeGuncelBilgi.ToList();
            return PartialView(orders.ToList());

        }
        [HttpPost]
        public PartialViewResult Listing(int userid)
        {
            var orders = db.ARGE_ProjeGuncelBilgi.Include(s => s.ARGE_Proje.ProjeKodu).Where(x => x.ProjeGuncellemeId == userid);
            return PartialView(orders.ToList());
        }
        public ActionResult Index2()
        {
            var users = db.ARGE_Proje.ToList();
            ViewBag.users = users;
            return View();
        }

        //public ActionResult ProjeGuncelBilgiRapor()
        //{
        //    return View(new ProjeGuncelBilgi());
        //}
        //[HttpPost]
        //public ActionResult ProjeGuncelBilgiRapor(ProjeGuncelBilgi model)
        //{


        //    return PersonelGuncelModeliOlustur(model);
        //}


        [HttpPost]
        public FileResult PersonelGuncelModeliOlustur(int id)
        {

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("Proje Kodu"),
                      new DataColumn("Proje Adı"),
          new DataColumn("Proje Guncelleme Tarihi"),
       new DataColumn("Proje Guncelleme Detayı"),




            });
            var liste = from list in db.ARGE_ProjeGuncelBilgi.OrderByDescending(a => a.ProjeGuncellemeTarih) select list;

            foreach (var list in liste)
            {
                dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.ARGE_Proje.ProjeAdi, list.ProjeGuncellemeTarih.Value.ToString("dd.MM.yyyy"), list.ProjeGuncellemeDetay);

            }
            dt.Rows.Add("F-32.03H/00");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "GuncelBilgi");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-ProjeGuncelBilgi" + ".xlsx");
                }


            }

        }
        public ActionResult _ExcelAktar()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu");

            List<SelectListItem> degerler2 = (from i in db.ARGE_ProjeGuncelBilgi.ToList().OrderByDescending(a => a.ProjeGuncellemeTarih)
                                              select new SelectListItem
                                              {
                                                  Text = i.ProjeGuncellemeDetay,
                                                  Value = i.ProjeGuncellemeDetay
                                              }).ToList();
            ViewBag.dgr2 = degerler2;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ExcelAktar(ARGE_ProjeGuncelBilgi aRGE_ProjeDenemeleri)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_ProjeGuncelBilgi.Add(aRGE_ProjeDenemeleri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_ProjeDenemeleri.ProjeId);
            return PartialView(aRGE_ProjeDenemeleri);
        }
 
        public ActionResult ProjeGuncel()
        {
            //var model = db.ARGE_ProjeGuncelBilgi.Find(id);
            //model.SecmeliProjeKodus = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu");
            //var kodDrop = db.ARGE_ProjeGuncelBilgi.Where(a => a.ProjeGuncellemeId > 0).OrderBy(a => a.ARGE_Proje.ProjeKodu).GroupBy(a => new { a.ARGE_Proje.ProjeKodu, a.ProjeGuncellemeId }).Select(a => new DropItem() { Tanim = a.Key.ProjeKodu, Id = a.Key.ProjeGuncellemeId.ToString() }).Distinct().OrderBy(a => a.Id).ToList();

            //var model = new ProjeGuncelBilgi
            //{
            //    SecmeliProjeKodus = new SelectList(kodDrop, "ID", "Tanim"),

            //};

            return View(new ProjeGuncelBilgiModel());

        }
        //[HttpPost]
        //public ActionResult ProjeGuncel(ProjeGuncelBilgi model)
        //{
        //    var PartilerDrop = db.ARGE_ProjeGuncelBilgi.Where(a => a.ProjeGuncellemeId > 0).OrderBy(a => a.ARGE_Proje.ProjeKodu).GroupBy(a => new { a.ARGE_Proje.ProjeKodu, a.ProjeGuncellemeId }).Select(a => new DropItem() { Tanim = a.Key.ProjeKodu, Id = a.Key.ProjeGuncellemeId.ToString() }).Distinct().OrderBy(a => a.Id).ToList();
        //    model.SecmeliProjeKodus = new SelectList(PartilerDrop, "Id", "Tanim");
        //    return ProjeRaporDetays(model);
        //}
        [HttpPost]
        public ActionResult ProjeGuncels(ProjeGuncelBilgiModel projeGuncel)
        {
            var model = db.ARGE_ProjeGuncelBilgi.Where(a => a.ARGE_Proje.ProjeKodu ==projeGuncel.ProjeKodus);
            return ProjeRaporDetays(projeGuncel);
        }
        [HttpPost]

        public FileResult ProjeRaporDetays(ProjeGuncelBilgiModel gelenmodels)
        {
            var model = new ProjeGuncelBilgiModel()
            {
               
            };


            model.ProjeKodus = gelenmodels.ProjeKodus;
           

            if (model.ProjeKodus != null)
            {
                model.GuncelBilgiRapors = db.ARGE_ProjeGuncelBilgi
                    .Where(a => a.ARGE_Proje.ProjeKodu == (model.ProjeKodus)).OrderByDescending(a=>a.ProjeGuncellemeTarih).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("PROJE KODU"),
                      new DataColumn("PROJE KISA ADI"),
                new DataColumn("PROJE ADI"),
                   new DataColumn("PROJE GÜNCELLEME TARİHİ"),
                           new DataColumn("PROJE GÜNCELLEME DETAYLARI"),





            });

            var liste = from list in db.ARGE_ProjeGuncelBilgi.Where(a => a.ARGE_Proje.ProjeKodu == gelenmodels.ProjeKodus) select list;

            foreach (var list in model.GuncelBilgiRapors)
            {

                dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.ARGE_Proje.ProjeKisaAd, list.ARGE_Proje.ProjeAdi, list.ProjeGuncellemeTarih.Value.ToString("dd.MM.yyyy"), list.ProjeGuncellemeDetay);

            }
            dt.Rows.Add("F-32.03H/00");

            using (XLWorkbook wbs = new XLWorkbook())
            {


                var W_WorkSheet111 = wbs.Worksheets.Add(dt, "ProjeGuncellemeDetaylari");
                W_WorkSheet111.Columns().AdjustToContents();
     

                using (MemoryStream stream = new MemoryStream())
                {
                    wbs.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy")+"-"+model.ProjeKodus + "-" + "ProjeDetay" + ".xlsx");
                }


            }
        }
        #region GENEL RAPORLAMA
        [HttpPost]
        public FileResult GuncelBilgiModeliOlustur(ARGE_ProjeGuncelBilgi personel)
        {


            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("Proje Kodu"),
                      new DataColumn("Proje Adı"),
          new DataColumn("Proje Guncelleme Tarihi"),
       new DataColumn("Proje Guncelleme Detayı"),




            });
            var liste = from list in db.ARGE_ProjeGuncelBilgi.OrderByDescending(a => a.ProjeGuncellemeTarih) select list;

            foreach (var list in liste)
            {
                dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.ARGE_Proje.ProjeAdi, list.ProjeGuncellemeTarih.Value.ToString("dd.MM.yyyy"), list.ProjeGuncellemeDetay);

            }
            dt.Rows.Add("F-32.03H/00");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "GuncelBilgi");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-ProjeGuncelBilgi" + ".xlsx");
                }


            }

        }
        #endregion

    }
}
