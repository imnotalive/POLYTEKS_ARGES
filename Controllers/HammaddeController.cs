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
    public class HammaddeController : BaseController
    {
       
        //public ActionResult Index()
        //{

        //        //return View(db.ARGE_Hammadde.OrderBy(a => a.KatkiIsmi).GroupBy(s => s.HammaddeGirisTarihi).Select(g => g.Count()));
        //    return View(db.ARGE_Hammadde.OrderBy(a=>a.KatkiIsmi).ToList());
        //}
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_Hammadde.OrderBy(a => a.KatkiIsmi).ToPagedList(page, recordsPerPage);
            return View(list);

        }

        public ActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_Hammadde aRGE_Hammadde)
        {
            if (ModelState.IsValid)
            {
              
                db.ARGE_Hammadde.Add(aRGE_Hammadde);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Hammadde Eklenmiştir.";
                return RedirectToAction("Index");
            }

            return View(aRGE_Hammadde);
        }

   
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Hammadde aRGE_Hammadde = db.ARGE_Hammadde.Find(id);
            if (aRGE_Hammadde == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Hammadde);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_Hammadde aRGE_Hammadde)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_Hammadde).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Hammadde Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            return View(aRGE_Hammadde);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Hammadde aRGE_Hammadde = db.ARGE_Hammadde.Find(id);
            if (aRGE_Hammadde == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Hammadde);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Hammadde aRGE_Hammadde = db.ARGE_Hammadde.Find(id);
            if (aRGE_Hammadde == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_Hammadde);
        }


        [HttpPost, ActionName("Delete")]
     
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_Hammadde aRGE_Hammadde = db.ARGE_Hammadde.Find(id);
            db.ARGE_Hammadde.Remove(aRGE_Hammadde);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Hammadde Silinmiştir.";
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
        public ActionResult deleted(string roleName)
        {
            return View();
        }
        //public ActionResult deleted(string ID)
        //{
        //    return Json("deleted", JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult DeleteEmployee(int ID)
        //{


        //    bool result = false;
        //    ARGE_Hammadde emp = db.ARGE_Hammadde.SingleOrDefault(x=>x.ID == ID);
        //    if (emp != null)
        //    {

        //        db.SaveChanges();
        //        result = true;
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}




        //    public ActionResult HammaddeRapor()
        //    {
        //        return View(new Hammadde() );
        //    }

        //    [HttpPost]
        //    public ActionResult HammaddeRapor(Hammadde model)
        //    {


        //        return PaketlemeTefrikModeliOlustur(model);
        //    }
        //    public FileResult PaketlemeTefrikModeliOlustur(Hammadde gelenModel)
        //    {

        //        var model = new Hammadde()
        //        {

        //        };



        //        if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
        //        {
        //            model.TefrikRaporOzetItems = db.ARGE_Hammadde
        //                .Where(a => a.HammaddeGirisTarihi >= (model.BaslangicTarihiDateTime) && a.HammaddeGirisTarihi <= (model.BitisTarihiDateTime)).OrderByDescending(a => a.HammaddeGirisTarihi).ToList();
        //        }
        //        else
        //        {
        //            ViewBag.Error = "VERİ YOK";
        //        }
        //        DataTable dt = new DataTable("Grid");
        //        dt.Columns.AddRange(new DataColumn[10]{
        //            new DataColumn("HAMMADDE TARİHİ"),
        //            new DataColumn("TEDARİKÇİ"),
        //                new DataColumn("KATKI İSMİ"),
        //                    new DataColumn("ÜRÜN KODU"),
        //                         new DataColumn("FONKSİYON"),
        //                          new DataColumn("DOZAJ"),
        //                             new DataColumn("MİKTAR"),
        //                                new DataColumn("BİRİM"),
        //                                   new DataColumn("FİYAT"),

        //                                             new DataColumn("ÖZELLİK"),





        //        });
        //        var liste = from list in db.ARGE_Hammadde.ToList() select list;


        //        if (model.TefrikRaporOzetItems != null)
        //        {
        //            foreach (var list in liste)
        //            {
        //                dt.Rows.Add(list.HammaddeGirisTarihi.Value.ToString("dd.MM.yyyy"),
        //                            list.Tedarikci,
        //                            list.KatkiIsmi,
        //                            list.UrunKodu,
        //                            list.Fonksiyon,
        //                            list.Dozaj,
        //                            list.Miktar,
        //                            list.MiktarBirim,
        //                            list.Fiyat,

        //                            list.Ozellik);
        //            }

        //        }
        //        else if (model.TefrikRaporOzetItems == null)
        //        {
        //            ViewBag.Error = "VERİ YOK";
        //        }

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            wb.Worksheets.Add(dt);
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                wb.SaveAs(stream);


        //                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "Hammadde" + ".xlsx");
        //            }


        //        }

        //    }


        //}

        [HttpPost]
        public FileResult HammaddeModeliOlustur(ARGE_Hammadde gelenModel)
        {

   
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                     new DataColumn("HAMMADDE TARİHİ"),
                    new DataColumn("TEDARİKÇİ"),
                        new DataColumn("KATKI İSMİ"),
                            new DataColumn("ÜRÜN KODU"),
                                 new DataColumn("FONKSİYON"),
                                  new DataColumn("DOZAJ"),
                                     new DataColumn("MİKTAR"),
                                        new DataColumn("MİKTAR BİRİMİ"),
                                           new DataColumn("FİYAT"),
                                            new DataColumn("FİYAT BİRİMİ"),
                                                     new DataColumn("ÖZELLİK"),




            });
            var liste = from list in db.ARGE_Hammadde.OrderBy(a => a.KatkiIsmi) select list ;



                foreach (var list in liste)
                {
                //dt.Rows.Add(list.HammaddeGirisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"),list.Tedarikci, list.KatkiIsmi, list.UrunKodu, list.Fonksiyon
                //    , list.Dozaj, list.Miktar,list.MiktarBirim,list.Fiyat,list.FiyatBirim, list.Ozellik);
                dt.Rows.Add(list.HammaddeGirisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"), list.Tedarikci, list.KatkiIsmi, list.UrunKodu, list.Fonksiyon
                 , list.Dozaj, list.Miktar, list.MiktarBirim, list.Fiyat, list.FiyatBirim, list.Ozellik);
            }
            dt.Rows.Add("F-32.09D/02");


            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Hammadde");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ARGE-Hammadde" + ".xlsx");
                }


            }

        }

    }
}
