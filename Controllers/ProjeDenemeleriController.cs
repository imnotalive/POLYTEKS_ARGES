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
using ClosedXML;
using POLYTEKS_ARGE.Models;
using PagedList;

namespace POLYTEKS_ARGE.Controllers
{
    [AllowAnonymous]
    public class ProjeDenemeleriController : Controller
    {

        PTKS_ARGE1 db = new PTKS_ARGE1();
        //public ActionResult Index()
        //{
        //    var aRGE_ProjeDenemeleri = db.ARGE_ProjeDenemeleri.OrderByDescending(a=>a.DenemeTarih).Include(a => a.ARGE_Proje);
        //    return View(aRGE_ProjeDenemeleri.ToList());
        //}
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_ProjeDenemeleri.OrderByDescending(a => a.DenemeTarih).ToPagedList(page, recordsPerPage);
            ViewBag.list = list;
            return View(list);

        }

        public ActionResult List()
        {
            var orders = db.ARGE_ProjeDenemeleri.ToList();
            return View(orders.ToList());
        }
        [HttpPost]
        public ActionResult List(int listid)
        {
            List<SelectListItem> degerler2 = (from i in db.ARGE_Proje.ToList().OrderBy(a => a.ProjeKodu)
                                              select new SelectListItem
                                              {
                                                  Text = i.ProjeKodu,
                                                  Value = i.ProjeKodu
                                              }).ToList();
            ViewBag.dgr2 = degerler2;
            return View();
        }
        private static ProjeDenemeModel PopulateModel(string country)
        {
            using (PTKS_ARGE1 entities = new PTKS_ARGE1())
            {
                ProjeDenemeModel model = new ProjeDenemeModel()
                {
                    Customers = (from c in entities.ARGE_ProjeDenemeleri
                                 where c.ARGE_Proje.ProjeKodu == country || string.IsNullOrEmpty(country)
                                 select c).ToList(),
                    Countries = (from c in entities.ARGE_ProjeDenemeleri
                                 select new SelectListItem { Text = c.ARGE_Proje.ProjeKodu, Value = c.ARGE_Proje.ProjeKodu }).Distinct().ToList()
                };

                return model;
            }
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri = db.ARGE_ProjeDenemeleri.Find(id);
            if (aRGE_ProjeDenemeleri == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_ProjeDenemeleri);
        }


        public ActionResult _PoyDenemeOlustur()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu");
            ViewBag.KesitTipi = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi");
            ViewBag.Chips = new SelectList(db.ARGE_DenemeChips, "ID", "ChipsAdi");
            //List<SelectListItem> degerler = (from i in db.ARGE_DenemeChips.ToList()
            //                                 select new SelectListItem
            //                                 {
            //                                     Text = i.ChipsAdi,
            //                                     Value = i.ID.ToString()
            //                                 }).ToList();
            //ViewBag.dgr = degerler;

            List<SelectListItem> degerler2 = (from i in db.ARGE_Makineler.ToList().Where(a=>a.MakineBolum=="URETIM").OrderBy(a=>a.MakineNo)
                                             select new SelectListItem 
                                             {
                                                 Text = i.MakineNo,
                                                 Value = i.MakineNo
                                             }).ToList();
            ViewBag.dgr2 = degerler2;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PoyDenemeOlustur( ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_ProjeDenemeleri.Add(aRGE_ProjeDenemeleri);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni AR-GE Projesi POY Denemesi Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_ProjeDenemeleri.ProjeId);
            ViewBag.KesitTipiId = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.POYDenemeKesitId);
      
            ViewBag.ChipsId = new SelectList(db.ARGE_DenemeChips, "ID", "ChipsAdi", aRGE_ProjeDenemeleri.Chips);
            return PartialView(aRGE_ProjeDenemeleri);
        }
        public ActionResult _TekstureDenemeOlustur()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu");
            ViewBag.KesitTipiId1 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi");
            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "POYLotu");
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            //foreach ile db.Categories deki kategorileri listemize ekliyoruz
            foreach (var item in db.ARGE_ProjeDenemeleri.GroupBy(a => a.POYLotu).Select(g => g.FirstOrDefault()))
            {
                kategoriler.Add(new SelectListItem { Text = item.POYLotu, Value = item.POYLotu });
            }
            ViewBag.Kategoriler = kategoriler;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            List<SelectListItem> degerlerS = (from i in db.ARGE_Makineler.ToList().Where(a => a.MakineBolum =="TEKSTURE")
                                              select new SelectListItem
                                              {
                                                  Text = i.MakineNo,
                                                  Value = i.MakineNo
                                              }).ToList();
            ViewBag.dgrs = degerlerS;
            //List<SelectListItem> degerler10 = (from i in db.ARGE_Makineler.ToList().Where(a => a.MakineBolum == "TEKSTURE").OrderBy(a => a.MakineNo)
            //                                  select new SelectListItem
            //                                  {
            //                                      Text = i.MakineNo,
            //                                      Value = i.MakineNo
            //                                  }).ToList();
            //ViewBag.dgr10 = degerler10;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _TekstureDenemeOlustur(ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_ProjeDenemeleri.Add(aRGE_ProjeDenemeleri);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni AR-GE Projesi Tekstüre Denemesi Eklenmiştir.";
                return RedirectToAction("Index");
            }
           
                ViewBag.ProjeId = new SelectList(db.ARGE_Proje.OrderBy(a=>a.ProjeKodu), "ID", "ProjeKodu", aRGE_ProjeDenemeleri.ProjeId);
     
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.TekstureDenemeKesitId);
      

            return View(aRGE_ProjeDenemeleri);
        }

        public ActionResult _BukumDenemeOlustur()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu");
            ViewBag.DenemeId1 = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "POYLotu");
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi");
            List<SelectListItem> degerler4 = (from i in db.ARGE_Makineler.ToList().Where(a => a.MakineBolum == "BUKUM").OrderBy(a => a.MakineNo)
                                              select new SelectListItem
                                              {
                                                  Text = i.MakineNo,
                                                  Value = i.MakineNo
                                              }).ToList();
            ViewBag.dgr4 = degerler4;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            List<SelectListItem> kategoriler1 = new List<SelectListItem>();
            foreach (var item in db.ARGE_ProjeDenemeleri.GroupBy(a => a.POYLotu).Select(g => g.FirstOrDefault()))
            {
                kategoriler1.Add(new SelectListItem { Text = item.POYLotu, Value = item.POYLotu });
            }
            ViewBag.Kategoriler1 = kategoriler1;
            //////////////////////////////////////////////////////////////////////
            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "TekstureLotu");
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            //foreach ile db.Categories deki kategorileri listemize ekliyoruz
            foreach (var item in db.ARGE_ProjeDenemeleri.GroupBy(a => a.TekstureLotu).Select(g => g.FirstOrDefault()))
            {
                kategoriler.Add(new SelectListItem { Text = item.TekstureLotu, Value = item.TekstureLotu });
            }
            ViewBag.Kategoriler = kategoriler;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _BukumDenemeOlustur(ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_ProjeDenemeleri.Add(aRGE_ProjeDenemeleri);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni AR-GE Projesi Büküm Denemesi Eklenmiştir.";
                return RedirectToAction("Index");
            }
   
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.BukumDenemeKesitId);
           
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_ProjeDenemeleri.ProjeId);
            return View(aRGE_ProjeDenemeleri);
        }

        public ActionResult _HavaTekstureDenemeOlustur()
        {
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu");
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi");
            ViewBag.DenemeId1 = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "POYLotu");
            List<SelectListItem> degerler5 = (from i in db.ARGE_Makineler.ToList().Where(a => a.MakineBolum == "TEKNİK TEKSTİL").OrderBy(a => a.MakineNo)
                                              select new SelectListItem
                                              {
                                                  Text = i.MakineNo,
                                                  Value = i.MakineNo
                                              }).ToList();
            ViewBag.dgr5 = degerler5;
            List<SelectListItem> kategoriler1 = new List<SelectListItem>();
            foreach (var item in db.ARGE_ProjeDenemeleri.GroupBy(a => a.POYLotu).Select(g => g.FirstOrDefault()))
            {
                kategoriler1.Add(new SelectListItem { Text = item.POYLotu, Value = item.POYLotu });
            }
            ViewBag.Kategoriler1 = kategoriler1;
            //////////////////////////////////////////////////////////////////////
            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "TekstureLotu");
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            //foreach ile db.Categories deki kategorileri listemize ekliyoruz
            foreach (var item in db.ARGE_ProjeDenemeleri.GroupBy(a => a.TekstureLotu).Select(g => g.FirstOrDefault()))
            {
                kategoriler.Add(new SelectListItem { Text = item.TekstureLotu, Value = item.TekstureLotu });
            }
            ViewBag.Kategoriler = kategoriler;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _HavaTekstureDenemeOlustur(ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_ProjeDenemeleri.Add(aRGE_ProjeDenemeleri);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni AR-GE Projesi Hava Tekstüre Denemesi Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_ProjeDenemeleri.ProjeId);
        
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.HavaTekstureDenemeKesitId);
            ViewBag.DenemeId1 = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "POYLotu", aRGE_ProjeDenemeleri.Chips);
            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "TekstureLotu", aRGE_ProjeDenemeleri);
            return View(aRGE_ProjeDenemeleri);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri = db.ARGE_ProjeDenemeleri.Find(id);
            if (aRGE_ProjeDenemeleri == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> degerler2 = (from i in db.ARGE_Makineler.ToList().Where(a => a.MakineBolum == "URETIM").OrderBy(a => a.MakineNo)
                                              select new SelectListItem
                                              {
                                                  Text = i.MakineNo,
                                                  Value = i.MakineNo.ToString()
                                              }).ToList();
            if (degerler2.ToString() !=null || degerler2.ToString()==null)
            {

                ViewBag.dgr2 = degerler2; }
            List<SelectListItem> degerler4 = (from i in db.ARGE_Makineler.ToList().Where(a => a.MakineBolum == "BUKUM").OrderBy(a => a.MakineNo)
                                              select new SelectListItem
                                              {
                                                  Text = i.MakineNo,
                                                  Value = i.MakineNo.ToString()
                                              }).ToList();
            if (degerler4.ToString() != null || degerler2.ToString() == null)
            {
                ViewBag.dgr4 = degerler4;
            }
            List<SelectListItem> degerler5 = (from i in db.ARGE_Makineler.ToList().Where(a => a.MakineBolum == "TEKNİK TEKSTİL").OrderBy(a => a.MakineNo)
                                              select new SelectListItem
                                              {
                                                  Text = i.MakineNo,
                                                  Value = i.MakineNo.ToString()
                                              }).ToList();
            if (degerler5.ToString() != null || degerler2.ToString() == null)
            {
                ViewBag.dgr5 = degerler5;
            }
        
            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_ProjeDenemeleri.ProjeId);
            ViewBag.KesitTipiId = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi);
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi1);
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi2);
            ViewBag.KesitTipiId4 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi3);
            ViewBag.ChipsId = new SelectList(db.ARGE_DenemeChips, "ID", "ChipsAdi", aRGE_ProjeDenemeleri.Chips);

            return View(aRGE_ProjeDenemeleri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_ProjeDenemeleri).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "AR-GE Proje Denemesi Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
           
                ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_ProjeDenemeleri.ProjeId);
            ViewBag.KesitTipiId = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi);
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi1);
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi2);
            ViewBag.KesitTipiId4 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_ProjeDenemeleri.ARGE_KesitTipi3);
            ViewBag.ChipsId = new SelectList(db.ARGE_DenemeChips, "ID", "ChipsAdi", aRGE_ProjeDenemeleri.Chips);
            return View(aRGE_ProjeDenemeleri);
        }
   
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri = db.ARGE_ProjeDenemeleri.Find(id);
            if (aRGE_ProjeDenemeleri == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_ProjeDenemeleri);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_ProjeDenemeleri aRGE_ProjeDenemeleri = db.ARGE_ProjeDenemeleri.Find(id);
            db.ARGE_ProjeDenemeleri.Remove(aRGE_ProjeDenemeleri);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "AR-GE Proje Denemesi Silinmiştir.";
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
        public FileResult DenemeModeliOlustur(ARGE_ProjeDenemeleri personel)
        {


            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("Proje Kodu"),
                      new DataColumn("Proje Adı"),
          new DataColumn("Proje Başlama Tarihi"),
        new DataColumn("Proje Bitiş Tarihi"), 
                new DataColumn("Proje Durumu"),
                new DataColumn("Deneme Tarihi"), 
                new DataColumn("POY Lotu"), 
                new DataColumn("POY DTEX"), new DataColumn("Katkı"), new DataColumn("Chips"),new DataColumn("Chips2"), new DataColumn("POY Mak."), new DataColumn("POY Miktarı"), new DataColumn("Tekstüre Lotu"), new DataColumn("Tekstüre DTEX"), new DataColumn("Tekstüre Mak."), new DataColumn("Tekstüre Miktarı"), new DataColumn("Fantazi Büküm Lotu"),new DataColumn("Fantazi Büküm DTEX"), new DataColumn("Fantazi Büküm Mak."),
                                       new DataColumn("Fantezi Büküm Miktarı"),new DataColumn("Hava Tekstüre Lotu"),new DataColumn("Hava Tekstüre DTEX"),new DataColumn("Hava Tekstüre Mak."),new DataColumn("Hava Tekstüre Miktarı"),new DataColumn("NOT-AÇIKLAMA"),
                                                                               

            });
            var liste = from list in db.ARGE_ProjeDenemeleri.OrderByDescending(a => a.DenemeTarih)  select list;

            foreach (var list in liste)
            {
                dt.Rows.Add(list.ARGE_Proje.ProjeKodu, list.ARGE_Proje.ProjeAdi, list.ARGE_Proje.ProjeBaslangicTarihi.Value.ToString("dd.MM.yyyy"), list.ARGE_Proje.ProjeBitisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"), list.ARGE_Proje.ProjeDurum ? "AKTİF PROJE" : "PASİF PROJE", list.DenemeTarih.Value.ToString("dd.MM.yyyy"), list.POYLotu,list.POYDtex,list.Katki,list.ARGE_DenemeChips.ChipsAdi, list.Chips2,list.POYMakinesi,list.POYMiktari,list.TekstureLotu,list.TekstureDtex,list.TekstureMakinesi,list.TekstureMiktari,list.BukumLotu,list.BukumDtex,list.BukumMakinesi,list.BukumMiktari,list.HavaTekstureLotu,list.HavaTekstureDtex,list.HavaTekstureMakinesi,list.HavaTekstureMiktari,list.Aciklama);

            }
            dt.Rows.Add("F-32.09A/01");
          

            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Deneme");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy")+ "-" + "ARGE-Deneme" + ".xlsx");
                }


            }

        }

    }
}
