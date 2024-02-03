using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using POLYTEKS_ARGE.Models;

namespace POLYTEKS_ARGE.Controllers
{
    [AllowAnonymous]
    public class HomeController :Controller
    {
        PTKS_ARGE1 db = new PTKS_ARGE1();
        SearchModel viewModel = null;

        public ActionResult Index()
        {
            return View(db.ARGE_Proje.Where(a => a.ProjeDurum == true).OrderByDescending(a => a.ProjeBaslangicTarihi).ToList());
        }
  [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
     
        public ActionResult Giris(ARGE_Personel Kullanici)
        {
            var teyit = db.ARGE_Personel.FirstOrDefault(a => a.KullaniciAdi == Kullanici.KullaniciAdi && a.Sifre == Kullanici.Sifre);
            if (teyit != null)
            {
                FormsAuthentication.SetAuthCookie(teyit.AdSoyad, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.mesaj = "GEÇERSİZ KULLANICI ADI VEYA ŞİFRE !";
                return View();
            }

        }

        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Giris", "Home");
        }
        public ActionResult Profil()
        {

            //HttpContext.Current.User.Identity.Name.ToString();
            return View();
        }
        public async Task<PartialViewResult> Search(string searchKey)
        {
            var tasks = new Task[3];
            int i = 0;
            viewModel = new SearchModel();
            viewModel.SearchKey = searchKey;
            List<Task> TaskList = GetSeachResult(searchKey, viewModel);
            foreach (Task tsk in TaskList)
            {
                tasks[i] = tsk;
                i++;
            }
            await Task.WhenAll(tasks);

            return PartialView("ResultView", viewModel);
        }
        public ActionResult Home()
        {
            return View();
        }
        private List<Task> GetSeachResult(string search, SearchModel model)
        {
            //NorthContext dbContext = new NorthContext();
            List<Task> Tasks = new List<Task>();
            var taskCustomer = Task.Factory.StartNew(() =>
            {
                using (PTKS_ARGE1 db = new PTKS_ARGE1())
                {
                    model.DenemeIsteks = db.ARGE_DenemeIstek.Where(cus => cus.Katki.Contains(search)).ToList();
                }
            });
            Tasks.Add(taskCustomer);
            var taskSupplier = Task.Factory.StartNew(() =>
            {
                using (PTKS_ARGE1 db = new PTKS_ARGE1())
                {
                    model.Hammaddes = db.ARGE_Hammadde.Where(sup => sup.KatkiIsmi.Contains(search)).ToList();
                }
            });
            Tasks.Add(taskSupplier);
            var taskEmployee = Task.Factory.StartNew(() =>
            {
                using (PTKS_ARGE1 db = new PTKS_ARGE1())
                {
                    model.FikirHavuzus = db.ARGE_FikirHavuzu.Where(emp => emp.FikirSahibi.Contains(search)).ToList();
                }
            });
            Tasks.Add(taskEmployee);
            return Tasks;
        }
        //public PartialViewResult HizliArama(string kel)
        //{
        //    var model = new LayoutModel() { DropHizliArama = new List<DropItem>() };



        //    foreach (var linkModul in KullaniciLinkleri)
        //    {
        //        foreach (var item in linkModul.UserLinkList.Where(a => a.DetayLinkDurumu == 0).ToList())
        //        {



        //            if (item.LinkTanimAdi.ToLower().Contains(kel.ToLower()))
        //            {
        //                model.DropHizliArama.Add(new DropItem()
        //                {
        //                    Tanim = item.LinkTanimAdi,
        //                    Id = string.IsNullOrWhiteSpace(item.AreaName) ? string.Format("/{0}/{1}", item.ControllerName, item.ActionName) : string.Format("/{0}/{1}/{2}", item.AreaName, item.ControllerName, item.ActionName)
        //                });
        //            }

        //        }
        //    }

        //    model.DropHizliArama = model.DropHizliArama.OrderBy(a => a.Tanim).ToList();


        //    return PartialView("_HizliArama", model);
        //}
    }
}