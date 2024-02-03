//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using POLYTEKS_ARGE.Models;


//namespace POLYTEKS_ARGE.Controllers
//{
//    [AllowAnonymous]
//    public class ProjeDenemeIplikDetayController : Controller
//    {
//         PTKS_ARGE db = new PTKS_ARGE();
//        public JsonResult DenemeGetir(int p)
//        {
//            var projedenemeleri = (from x in db.ARGE_ProjeDenemeleri
//                                   join y in db.ARGE_Proje on x.ARGE_Proje.ID equals y.ID
//                                   where x.ARGE_Proje.ID == p
//                                   select new
//                                   {
//                                       Text = x.DenemeAdi,
//                                       Value = x.ID.ToString()
//                                   }).ToList();
//            return Json(projedenemeleri, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Index()
//        {
         
//                var aRGE_ProjeDenemeIplikDetay = db.ARGE_ProjeDenemeIplikDetay.Include(a => a.ARGE_Proje).Include(a => a.ARGE_ProjeDenemeleri);
//                return View(aRGE_ProjeDenemeIplikDetay.ToList());
            
   
//        }


//        public ActionResult Create()
//        {
//            //cs.Projeler = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi");
//            //cs.Denemeler = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "DenemeAdi");
//            ////return View(cs);
//            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi");
//            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "DenemeAdi");
//            List<SelectListItem> kategoriler = new List<SelectListItem>();
//            //foreach ile db.Categories deki kategorileri listemize ekliyoruz
//            foreach (var item in db.ARGE_ProjeDenemeIplikDetay.GroupBy(a => a.PartiNo).Select(g => g.FirstOrDefault())) 
//            {   
//                kategoriler.Add(new SelectListItem { Text = item.PartiNo, Value = item.PartiNo });
//            }
     
//            ViewBag.Kategoriler = kategoriler;
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create( ARGE_ProjeDenemeIplikDetay aRGE_ProjeDenemeIplikDetay)
//        {
//            if (ModelState.IsValid)
//            {
//                db.ARGE_ProjeDenemeIplikDetay.Add(aRGE_ProjeDenemeIplikDetay);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_ProjeDenemeIplikDetay.ProjeId);
//            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "DenemeAdi", aRGE_ProjeDenemeIplikDetay.DenemeId);
          
//            return View(aRGE_ProjeDenemeIplikDetay);
//        }
  
     
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ARGE_ProjeDenemeIplikDetay aRGE_ProjeDenemeIplikDetay = db.ARGE_ProjeDenemeIplikDetay.Find(id);
//            if (aRGE_ProjeDenemeIplikDetay == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_ProjeDenemeIplikDetay.ProjeId);
//            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "DenemeAdi", aRGE_ProjeDenemeIplikDetay.DenemeId);
//            List<SelectListItem> kategoriler = new List<SelectListItem>();
//            //foreach ile db.Categories deki kategorileri listemize ekliyoruz
//            foreach (var item in db.ARGE_ProjeDenemeIplikDetay.GroupBy(a => a.PartiNo).Select(g => g.FirstOrDefault()))
//            {   //Text = Görünen kısımdır. Kategori ismini yazdıyoruz
//                //Value = Değer kısmıdır.ID değerini atıyoruz
//                kategoriler.Add(new SelectListItem { Text = item.PartiNo, Value = item.PartiNo });
//            }
//            //Dinamik bir yapı oluşturup kategoriler list mizi view mize göndereceğiz
//            //bunun için viewbag kullanıyorum
//            ViewBag.Kategoriler = kategoriler;
//            return View(aRGE_ProjeDenemeIplikDetay);
//        }

      
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit( ARGE_ProjeDenemeIplikDetay aRGE_ProjeDenemeIplikDetay)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aRGE_ProjeDenemeIplikDetay).State = System.Data.Entity.EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ProjeId = new SelectList(db.ARGE_Proje, "ID", "ProjeAdi", aRGE_ProjeDenemeIplikDetay.ProjeId);
//            ViewBag.DenemeId = new SelectList(db.ARGE_ProjeDenemeleri, "ID", "DenemeAdi", aRGE_ProjeDenemeIplikDetay.DenemeId);
//            return View(aRGE_ProjeDenemeIplikDetay);
//        }

//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ARGE_ProjeDenemeIplikDetay aRGE_ProjeDenemeIplikDetay = db.ARGE_ProjeDenemeIplikDetay.Find(id);
//            if (aRGE_ProjeDenemeIplikDetay == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aRGE_ProjeDenemeIplikDetay);
//        }

//        // POST: ProjeDenemeIplikDetay/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            ARGE_ProjeDenemeIplikDetay aRGE_ProjeDenemeIplikDetay = db.ARGE_ProjeDenemeIplikDetay.Find(id);
//            db.ARGE_ProjeDenemeIplikDetay.Remove(aRGE_ProjeDenemeIplikDetay);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ARGE_ProjeDenemeIplikDetay aRGE_Numune = db.ARGE_ProjeDenemeIplikDetay.Find(id);
//            if (aRGE_Numune == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aRGE_Numune);
//        }

//    }
//}
