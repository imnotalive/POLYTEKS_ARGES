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
    public class NumuneController : BaseController
    {



        //public ActionResult Index()
        //{
        //    var aRGE_Numune = db.ARGE_Numune;
        //    return View(aRGE_Numune.ToList());
        //}
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_Numune.OrderByDescending(a=>a.HatirlatmaTarihi).ToList().ToPagedList(page, recordsPerPage);
            return View(list);

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Numune aRGE_Numune = db.ARGE_Numune.Find(id);
            if (aRGE_Numune == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Numune);
        }
   
 
        public ActionResult Create()
        {
            //ViewBag.PartiId = new SelectList(db.ARGE_ProjeDenemeIplikDetay, "ID", "PartiNo");
            //List<SelectListItem> kategoriler = new List<SelectListItem>();
            ////foreach ile db.Categories deki kategorileri listemize ekliyoruz
            //foreach (var item in db.ARGE_ProjeDenemeIplikDetay.GroupBy(a => a.PartiNo).Select(g => g.FirstOrDefault()))
            //{   //Text = Görünen kısımdır. Kategori ismini yazdıyoruz
            //    //Value = Değer kısmıdır.ID değerini atıyoruz
            //    kategoriler.Add(new SelectListItem { Text = item.PartiNo, Value = item.PartiNo });
            //}
            ////Dinamik bir yapı oluşturup kategoriler list mizi view mize göndereceğiz
            ////bunun için viewbag kullanıyorum
            //ViewBag.Kategoriler = kategoriler;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_Numune aRGE_Numune)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_Numune.Add(aRGE_Numune);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Numune Eklenmiştir.";
                return RedirectToAction("Index");
            }

         
            return View(aRGE_Numune);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Numune aRGE_Numune = db.ARGE_Numune.Find(id);
            if (aRGE_Numune == null)
            {
                return HttpNotFound();
            }
     
            return View(aRGE_Numune);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ARGE_Numune aRGE_Numune)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_Numune).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Numune Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
      
            return View(aRGE_Numune);
        }

 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Numune aRGE_Numune = db.ARGE_Numune.Find(id);
            if (aRGE_Numune == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Numune);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_Numune aRGE_Numune = db.ARGE_Numune.Find(id);
            db.ARGE_Numune.Remove(aRGE_Numune);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Numune Silinmiştir.";
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
        //RAPORLAMA//
        
        #region PARTİKODUFİLTRELİ
        public ActionResult NumuneRapor()
        {
     

            return View(new NumuneModel());

        }
        [HttpPost]
        public ActionResult NumuneRapors(NumuneModel projeGuncel)
        {
            var model = db.ARGE_Numune.Where(a => a.PartiKodu == projeGuncel.PartiKodu);
            return NumuneExcelOlustur(projeGuncel);



        }
        [HttpPost]
        public ActionResult NumuneExcelOlustur(NumuneModel gelenmodels)
        {
            var model = new NumuneModel()
            {

            };
          
            
            model.numuneAdis = gelenmodels.numuneAdis;


            if (model.numuneAdis != null)
            {
                model.NumuneKontrol = db.ARGE_Numune
                    .Where(a => a.PartiKodu == (model.PartiKodu)).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("MÜŞTERİ FİRMA"),
                      new DataColumn("İLGİLİ KİŞİ"),
                                      new DataColumn("İLETİŞİM"),
                                 new DataColumn("MİKTAR"),
                                      new DataColumn("PARTİ KODU"),
                                      new DataColumn("AÇIKLAMA"),
            });
            var liste = from list in db.ARGE_Numune.OrderByDescending(a => a.HatirlatmaTarihi) select list;

            foreach (var list in model.NumuneKontrol)
            {
                dt.Rows.Add(list.MusteriFirma, list.IlgiliKisi, list.Iletisim, list.Miktar,list.PartiKodu,list.Aciklama);

            }
            dt.Rows.Add("F-32.03G/00");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Numune");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-Numune" + ".xlsx");
                }


            }
            
        }
        #endregion
        #region GENEL RAPOR
        [HttpPost]
        public FileResult NumuneExcelOlusturs(ARGE_Numune gelenmodels)
        {


            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("MÜŞTERİ FİRMA"),
                      new DataColumn("İLGİLİ KİŞİ"),
                                      new DataColumn("İLETİŞİM"),
                                 new DataColumn("MİKTAR"),
                                      new DataColumn("PARTİ KODU"),
                                      new DataColumn("AÇIKLAMA"),
            });
            var liste = from list in db.ARGE_Numune.OrderByDescending(a => a.HatirlatmaTarihi) select list;

            foreach (var list in liste)
            {
                dt.Rows.Add(list.MusteriFirma, list.IlgiliKisi, list.Iletisim, list.Miktar, list.PartiKodu, list.Aciklama);

            }
            dt.Rows.Add("F-32.03G/00");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Numune");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-Numune" + ".xlsx");
                }


            }




        }
        #endregion


        #region NumuneAdıFİLTRELİCombobox
        public ActionResult NumuneAdiRapor()
        {
            var model = new NumuneModel
            {

            };
            model.NumuneSelect = new SelectList(db.ARGE_Numune, "ID", "NumuneAdi");
            /*
            var kodDrop = db.ARGE_Numune.Where(a => a.ID > 0).OrderBy(a => a.ARGE_Proje.ProjeKodu).GroupBy(a => new { a.ARGE_Proje.ProjeKodu, a.ProjeGuncellemeId }).Select(a => new DropItem() { Tanim = a.Key.ProjeKodu, Id = a.Key.ProjeGuncellemeId.ToString() }).Distinct().OrderBy(a => a.Id).ToList();

          */

            return View(model);

        }
        [HttpPost]
        public ActionResult NumuneAdiRapor(NumuneModel model)
        {

            var secilenId = model.SecilenId;

         
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("MÜŞTERİ FİRMA"),
                      new DataColumn("İLGİLİ KİŞİ"),
                                      new DataColumn("İLETİŞİM"),
                                 new DataColumn("MİKTAR"),
                                      new DataColumn("PARTİ KODU"),
                                      new DataColumn("AÇIKLAMA"),
            });
            var liste = from list in db.ARGE_Numune.Where(a=>a.ID == secilenId).OrderByDescending(a => a.HatirlatmaTarihi).AsNoTracking().Distinct() select list;

            foreach (var list in liste)
            {
                dt.Rows.Add(list.MusteriFirma, list.IlgiliKisi, list.Iletisim, list.Miktar, list.PartiKodu, list.Aciklama);

            }
            dt.Rows.Add("F-32.03G/00");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Numune");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-Numune" + ".xlsx");
                }


            }
            // var model = db.ARGE_Numune.Where(a => a.NumuneAdi == numuneadi.);
            //return View(secilenId);



        }
        //[HttpPost]
        //public FileResult NumuneAdiExcelOlustur(NumuneModel gelenmodels)
        //{
        //    var model = new NumuneModel()
        //    {

        //    };


        //    model.NumuneAdi = gelenmodels.NumuneAdi;


        //    if (model.NumuneAdi != null)
        //    {
        //        model.TefrikRaporOzetItems = db.ARGE_Numune
        //            .Where(a => a.PartiKodu == (model.PartiKodu)).ToList();
        //    }
        //    else
        //    {
        //        ViewBag.Error = "VERİ YOK";
        //    }

        //    DataTable dt = new DataTable("Grid");
        //    dt.Columns.AddRange(new DataColumn[]{
        //        new DataColumn("MÜŞTERİ FİRMA"),
        //              new DataColumn("İLGİLİ KİŞİ"),
        //                              new DataColumn("İLETİŞİM"),
        //                         new DataColumn("MİKTAR"),
        //                              new DataColumn("PARTİ KODU"),
        //                              new DataColumn("AÇIKLAMA"),
        //    });
        //    var liste = from list in db.ARGE_Numune.OrderByDescending(a => a.HatirlatmaTarihi) select list;

        //    foreach (var list in model.TefrikRaporOzetItems)
        //    {
        //        dt.Rows.Add(list.MusteriFirma, list.IlgiliKisi, list.Iletisim, list.Miktar, list.PartiKodu, list.Aciklama);

        //    }
        //    dt.Rows.Add("F-32.03G/00");


        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        var W_WorkSheet = wb.Worksheets.Add(dt, "Numune");
        //        W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
        //        W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);


        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-Numune" + ".xlsx");
        //        }


        //    }

        //}
        #endregion
    }

}

