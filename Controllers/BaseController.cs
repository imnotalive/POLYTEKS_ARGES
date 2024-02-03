using POLYTEKS_ARGE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace POLYTEKS_ARGE.Controllers
{
    [AllowAnonymous]
    public class BaseController : Controller
    {
        //#region
        //public void TempDataOlustur(string Mesaj, bool OlumLuMu)
        //{
        //    TempData["Msg"] = Mesaj;
        //    TempData["class"] = "danger";
        //    if (OlumLuMu)
        //    {
        //        TempData["class"] = "success";
        //    }
        //}
     
        //// GET: Base
        ///// <summary>
        ///// 1-Create
        ///// 2-Update
        ///// 3-Delete
        ///// </summary>
        ///// <param name="Durum"></param>
        //public void TempDataCRUDOlustur(int Durum)
        //{
        //    string Mesaj = "Kayıt İşlemi Yapılmıştır";
        //    TempData["class"] = "success";
        //    bool OlumLuMu = true;

        //    if (Durum == 2)
        //    {
        //        // güncelleme
        //        Mesaj = "Güncelleme İşlemi Yapılmıştır";
        //    }
        //    if (Durum == 3)
        //    {
        //        // sİLME
        //        Mesaj = "Silme İşlemi Yapılmıştır";
        //        OlumLuMu = false;
        //    }
        //    TempData["Msg"] = Mesaj;

        //    if (OlumLuMu == false)
        //    {
        //        TempData["class"] = "danger";
        //    }
        //}
        // #endregion
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public PTKS_ARGE1 db = new PTKS_ARGE1();
        public ARGE_Personel  Kullanici { get; set; }

        private static PTKS_ARGE1 _appContext;

        public static PTKS_ARGE1 _dbPolyCreate(bool yenilensinMi)
        {
            if (_appContext == null)
            {

                _appContext = new PTKS_ARGE1() { Configuration = { LazyLoadingEnabled = false, ProxyCreationEnabled = false } };


            }
            return _appContext;
        }
      
        public class DropItem
        {
            public string Id { get; set; }
            public int IdInt { get; set; }
            public string Tanim { get; set; }
            public string DigerTanim { get; set; }
            public bool SeciliMi { get; set; }
            public int Sira { get; set; }
            public int OnId { get; set; }
            public int OnIdInt { get; set; }
        }
    }
}