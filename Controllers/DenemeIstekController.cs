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
    public class DenemeIstekController : Controller
    {
        PTKS_ARGE1 db = new PTKS_ARGE1();


        //public ActionResult Index()
        //{
        //    var aRGE_DenemeIstek = db.ARGE_DenemeIstek.Include(a => a.ARGE_Proje).OrderByDescending(a=>a.Tarih);
        //    return View(aRGE_DenemeIstek.ToList());
        //}
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_DenemeIstek.OrderByDescending(a => a.Tarih).ToList().ToPagedList(page, recordsPerPage);
            return View(list);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_DenemeIstek aRGE_DenemeIstek = db.ARGE_DenemeIstek.Find(id);
            if (aRGE_DenemeIstek == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_DenemeIstek);
        }

     
        public ActionResult Create()
        {
            ViewBag.ProjeDenemeIstekId = new SelectList(db.ARGE_Proje.OrderBy(a=>a.ProjeKodu), "ID", "ProjeKodu");
            ViewBag.KesitTipiId = new SelectList(db.ARGE_KesitTipi.OrderBy(a => a.KesitTipi), "ID", "KesitTipi");
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi.OrderBy(a => a.KesitTipi), "ID", "KesitTipi");
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi.OrderBy(a => a.KesitTipi), "ID", "KesitTipi");
            ViewBag.DenemeIstekId = new SelectList(db.ARGE_DenemeDurum, "ID", "DenemeDurum"); 
            ViewBag.ChipsSecme = new SelectList(db.ARGE_DenemeChips.OrderBy(a=>a.ChipsAdi), "ID", "ChipsAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_DenemeIstek aRGE_DenemeIstek)
        {
            if (ModelState.IsValid)
            {
                db.ARGE_DenemeIstek.Add(aRGE_DenemeIstek);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni Deneme İsteği Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeDenemeIstekId = new SelectList(db.ARGE_Proje.OrderBy(a=>a.ProjeKodu), "ID", "ProjeKodu", aRGE_DenemeIstek.ProjeDenemeIstekId);
            ViewBag.KesitTipiId = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId);
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId2);
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId3);
            ViewBag.DenemeIstekId = new SelectList(db.ARGE_DenemeDurum, "ID", "DenemeDurum", aRGE_DenemeIstek.DenemeIstekId);
            ViewBag.ChipsSecme = new SelectList(db.ARGE_DenemeChips, "ID", "ChipsAdi",aRGE_DenemeIstek.ChipsSecme);
            return View(aRGE_DenemeIstek);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_DenemeIstek aRGE_DenemeIstek = db.ARGE_DenemeIstek.Find(id);
            if (aRGE_DenemeIstek == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjeDenemeIstekId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_DenemeIstek.ProjeDenemeIstekId);
            ViewBag.KesitTipiId = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId);
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId2);
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId3);
            ViewBag.DenemeIstekId = new SelectList(db.ARGE_DenemeDurum, "ID", "DenemeDurum", aRGE_DenemeIstek.DenemeIstekId);
            return View(aRGE_DenemeIstek);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_DenemeIstek aRGE_DenemeIstek)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_DenemeIstek).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "Deneme İsteği Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            ViewBag.ProjeDenemeIstekId = new SelectList(db.ARGE_Proje, "ID", "ProjeKodu", aRGE_DenemeIstek.ProjeDenemeIstekId);
            ViewBag.KesitTipiId = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId);
            ViewBag.KesitTipiId2 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId2);
            ViewBag.KesitTipiId3 = new SelectList(db.ARGE_KesitTipi, "ID", "KesitTipi", aRGE_DenemeIstek.KesitTipiId3);
            ViewBag.DenemeIstekId = new SelectList(db.ARGE_DenemeDurum, "ID", "DenemeDurum", aRGE_DenemeIstek.DenemeIstekId);
            return View(aRGE_DenemeIstek);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_DenemeIstek aRGE_DenemeIstek = db.ARGE_DenemeIstek.Find(id);
            if (aRGE_DenemeIstek == null)
            {
                return HttpNotFound();
            }
            return View(aRGE_DenemeIstek);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARGE_DenemeIstek aRGE_DenemeIstek = db.ARGE_DenemeIstek.Find(id);
            db.ARGE_DenemeIstek.Remove(aRGE_DenemeIstek);
            db.SaveChanges();
            TempData["DeleteAlertMessage"] = "Deneme İsteği Silinmiştir.";
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
        public ActionResult DenemeIstekRapor()
        {
            return View(new DenemeIstekModel());
        }
        [HttpPost]
        public ActionResult DenemeIstekRapor(DenemeIstekModel model)
        {


            return ProjeDenemeModeliOlustur(model);
        }

        public FileResult ProjeDenemeModeliOlustur(DenemeIstekModel gelenModel)
        {

            var model = new DenemeIstekModel()
            {

            };

            model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
            model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;
            if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
            {
                model.ArgeDenemeIstekModel = db.ARGE_DenemeIstek
                    .Where(a => a.Tarih >= (model.BaslangicTarihiDateTime) && a.Tarih <= (model.BitisTarihiDateTime)).OrderByDescending(a => a.Tarih).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("PROJE ADI"),
                new DataColumn("CHIPS"),
                    new DataColumn("DOZAJ"),
                        new DataColumn("KATKI"),
                         new DataColumn("KESİT TİPİ"),
                          new DataColumn("POY DTEX"),
                           new DataColumn("POY MİKTARI"),
                            new DataColumn("POY REFERANS"),
                             new DataColumn("TARİH"),





            });
            var liste = from list in db.ARGE_DenemeIstek.OrderByDescending(a => a.Tarih) select list;

            var raporViews = db.ARGE_DenemeIstek.FirstOrDefault(a => a.Tarih >= (model.BaslangicTarihiDateTime) && a.Tarih <= (model.BitisTarihiDateTime));

            if (raporViews != null)
            {
                foreach (var list in liste)
                {
                    dt.Rows.Add(list.ARGE_Proje.ProjeAdi, list.Chips, list.Dozaj, list.Katki, list.KesitTipi,list.POYDtex,list.POYMiktari,list.POYReferans,
                        list.Tarih.Value.ToString("dd.MM.yyyy"));
                }
                dt.Rows.Add("F-32.09B/01");

            }
            else if (raporViews == null)
            {
                ViewBag.Error = "VERİ YOK";
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "DenemeIstek");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                W_WorkSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "DenemeIstek" + ".xlsx");
                }


            }

        }

   
    }
}
