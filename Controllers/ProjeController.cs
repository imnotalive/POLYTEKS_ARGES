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
    public class ProjeController : BaseController
    {

   
        //public ActionResult Index()
        //{
        //    var aRGE_Proje = db.ARGE_Proje.OrderByDescending(a=>a.ProjeDurum).ThenByDescending(a=>a.ProjeBaslangicTarihi).ToList();
        //    return View(aRGE_Proje);

        //}
        public ActionResult Index(int page = 1)
        {
            //var aRGE_Sarfiyat = db.ARGE_Sarfiyat.OrderByDescending(a=>a.SarfiyatTarih).Include(a => a.ARGE_Proje);
            //return View(aRGE_Sarfiyat.ToList());
            int recordsPerPage = 50;

            var list = db.ARGE_Proje.OrderByDescending(a => a.ProjeDurum).ThenByDescending(a => a.ProjeBaslangicTarihi).ToPagedList(page, recordsPerPage);
            return View(list);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Proje aRGE_Proje = db.ARGE_Proje.Find(id);
            if (aRGE_Proje == null)
            {   TempData["InfoMessage"]="Böyle Bir Kayıt Bulunamadı."+id.ToString();
                return HttpNotFound();
            }
            return View(aRGE_Proje);
        }


        public ActionResult Create()
        {
            ViewBag.ProjeYurutucusuId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad");
            ViewBag.IsBirligiTuruId = new SelectList(db.ARGE_ProjeIsBirligiTuru, "ID", "IsBirligiTuru");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARGE_Proje aRGE_Proje)
        {
            if (ModelState.IsValid)
            {  
                db.ARGE_Proje.Add(aRGE_Proje);
                db.SaveChanges();
                TempData["AlertMessage"] = "Yeni AR-GE Projesi Eklenmiştir.";
                return RedirectToAction("Index");
            }

            ViewBag.ProjeYurutucusuId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_Proje.ProjeYurutucusuId);
            ViewBag.IsBirligiTuruId = new SelectList(db.ARGE_ProjeIsBirligiTuru, "ID", "IsBirligiTuru", aRGE_Proje.IsBirligiTuruId);
            return View(aRGE_Proje);
        }

     
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARGE_Proje aRGE_Proje = db.ARGE_Proje.Find(id);
            if (aRGE_Proje == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjeYurutucusuId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_Proje.ProjeYurutucusuId);
            ViewBag.IsBirligiTuruId = new SelectList(db.ARGE_ProjeIsBirligiTuru, "ID", "IsBirligiTuru", aRGE_Proje.IsBirligiTuruId);
            return View(aRGE_Proje);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARGE_Proje aRGE_Proje)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRGE_Proje).State = (System.Data.Entity.EntityState)EntityState.Modified;
                db.SaveChanges();
                TempData["EditAlertMessage"] = "AR-GE Projesi Düzenlenmiştir.";
                return RedirectToAction("Index");
            }
            ViewBag.ProjeYurutucusuId = new SelectList(db.ARGE_Personel, "PersonelId", "AdSoyad", aRGE_Proje.ProjeYurutucusuId);
            ViewBag.IsBirligiTuruId = new SelectList(db.ARGE_ProjeIsBirligiTuru, "ID", "IsBirligiTuru", aRGE_Proje.IsBirligiTuruId);
            return View(aRGE_Proje);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.ARGE_Proje.FirstOrDefault(x => x.ID == id);
            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ARGE_Proje aRGE_Proje = db.ARGE_Proje.Find(id);
        //    if (aRGE_Proje == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aRGE_Proje);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ARGE_Proje aRGE_Proje = db.ARGE_Proje.Find(id);
        //    db.ARGE_Proje.Remove(aRGE_Proje);
        //    db.SaveChanges();
        //    TempData["DeleteAlertMessage"] = "AR-GE Projesi Silinmiştir.";
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        #region RAPORLAMA
        public ActionResult ProjeRapor()
        {
            return View(new ProjeModel() );
        }
        [HttpPost]
        public ActionResult ProjeRapor2(ProjeModel model)
        {


            return ProjeModeliOlusturs(model);
        }
      

        [HttpPost]
        public FileResult ProjeModeliOlusturs(ProjeModel gelenModel)
        {
            var secilenId = gelenModel.id; // ID

            var model = new ProjeModel()
            {
              
            };
          

            model.BaslangicTarihiDateTime = gelenModel.BaslangicTarihiDateTime;
            model.BitisTarihiDateTime = gelenModel.BitisTarihiDateTime;

            if (model.BaslangicTarihiDateTime != null && model.BitisTarihiDateTime != null)
            {
                model.ArgeProjeRaporOzet = db.ARGE_Proje
                    .Where(a => a.ProjeBaslangicTarihi >= (model.BaslangicTarihiDateTime) && a.ProjeBitisTarihi <= (model.BitisTarihiDateTime)).OrderByDescending(a => a.ProjeAdi).ToList();
            }
            else
            {
                ViewBag.Error = "VERİ YOK";
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[11]{
                new DataColumn("PROJE KODU"),
                      new DataColumn("PROJE KISA ADI"),
                new DataColumn("PROJE ADI"),
                           new DataColumn("PROJE DURUMU"),
                    new DataColumn("PROJE BAŞLANGIÇ TARİHİ"),
                        new DataColumn("PROJE BİTİŞ TARİHİ"),
                             new DataColumn("PERSONEL SAYISI"),
                            new DataColumn("PROJE YÜRÜTÜCÜSÜ"),
                             new DataColumn("MALİYET ÖNGÖRÜSÜ"),
                                
                                       new DataColumn("TOPLAM ADAM AY"),
                              new DataColumn("DESTEK DURUMU"),
            });
            var liste = from list in db.ARGE_Proje.OrderByDescending(a => a.ProjeDurum).ThenByDescending(a => a.ProjeBaslangicTarihi) select list;

           

            if (model.ArgeProjeRaporOzet != null)
            {
               
                foreach (var list in model.ArgeProjeRaporOzet)
                {
                    dt.Rows.Add(list.ProjeKodu, list.ProjeKisaAd, list.ProjeAdi,list.ProjeDurum ? "AKTİF PROJE" : "PASİF PROJE", list.ProjeBaslangicTarihi.Value.ToString("dd.MM.yyyy"),
                        list.ProjeBitisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"), list.PersonelSayisi, list.ARGE_Personel.AdSoyad, list.MaliyetOngorusu.ToString("#,##0"),list.ToplamAdamAy,list.ProjeDestekDurumu);
                  
                }
             
               dt.Rows.Add("F-32.01/00"); 
               
                


            }
         
            else 
            {
                ViewBag.Error = "VERİ YOK";
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                //wb.Worksheets.Add(dt);
                //wb.
                var W_WorkSheet = wb.Worksheets.Add(dt, "Proje");
                W_WorkSheet.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "Proje" + ".xlsx");
                }


            }

        }
     
        public ActionResult ProjeRaporDetay(int id)
        {
            var model = db.ARGE_Proje.Find(id);

            return ProjeRaporDetays(model);
          
        }
        [HttpPost]

        public FileResult ProjeRaporDetays(ARGE_Proje gelenmodels)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                 new DataColumn(""),
                new DataColumn("PROJE KODU"),
                      new DataColumn("PROJE KISA ADI"),
                new DataColumn("PROJE ADI"),
                           new DataColumn("PROJE DURUMU"),
                    new DataColumn("PROJE BAŞLANGIÇ TARİHİ"),
                        new DataColumn("PROJE BİTİŞ TARİHİ"),
                     
                             new DataColumn("PERSONEL SAYISI"),
                            new DataColumn("PROJE YÜRÜTÜCÜSÜ"),
                             new DataColumn("MALİYET ÖNGÖRÜSÜ"),
                               
                                       new DataColumn("TOPLAM ADAM AY"),
                              new DataColumn("DESTEK DURUMU"),
                              new DataColumn("PROJE AR-GE YÖNÜ"),
                               new DataColumn("PROJEDE YAPILACAKLAR"),
                              new DataColumn("PROJE ÇIKTILARI"),
                              new DataColumn("PROJE KONUSU"),
                                    
                                        new DataColumn("F-32.03A1/00"),


            });
      
            var liste = from list in db.ARGE_Proje.Where(a => a.ID == gelenmodels.ID) select list;
        
            foreach (var list in liste)
            {

                dt.Rows.Add(list.ID,list.ProjeKodu, list.ProjeKisaAd, list.ProjeAdi, list.ProjeDurum ? "AKTİF PROJE" : "PASİF PROJE", list.ProjeBaslangicTarihi.Value.ToString("dd.MM.yyyy"),
                    list.ProjeBitisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"), list.PersonelSayisi, list.ARGE_Personel.AdSoyad, list.MaliyetOngorusu.ToString("#,##0"), list.ToplamAdamAy, list.ProjeDestekDurumu, list.ProjeArgeYonu, list.ProjedeYapilacaklar, list.ProjeCiktilari, list.ProjeKonusu, list.Aciklama2);
               
            }
            //dt.Rows.Add("F-32.03A1/00");
            //DataRow row = table.NewRow();
            //table.Rows.Add(row);
            //DataRow row = dt.NewRow();
            //row[17] = "Ravi";
            //dt.Rows.Add(row);
            //dt.Columns.Add("MyRow", typeof(System.Int32));
            ////dt.Columns["MyRow"].Expression = "'F-32.03A1/00'";
            //dt.Columns.Add("Ravi");
            //dt.Rows.Add("F-32.03A1/00");

            //Example table your yours (1st one:
            //DataTable tableA = new DataTable();
            //tableA.Columns.AddRange(new DataColumn[] {
            //    new DataColumn("Company", typeof(string)),
            //    new DataColumn("Name", typeof(string)),
            //    new DataColumn("Value", typeof(string)) });

            //tableA.Rows.Add("A123", "City", "Atlanta");
            //tableA.Rows.Add("A123", "Country", "USA");
            //tableA.Rows.Add("A123", "ZipCode", "30328");
            //tableA.Rows.Add("ABC", "City", "Chicago");
            //tableA.Rows.Add("ABC", "Country", "USA");
            //tableA.Rows.Add("ABC", "ZipCode", "60661");

            ////
            ////NEW TABLE:
            ////
            //DataTable table = new DataTable("MyData");
            //table.Columns.AddRange(new DataColumn[] {
            //    new DataColumn("Company", typeof(string)),
            //    new DataColumn("City", typeof(string)),
            //    new DataColumn("Country", typeof(string)),
            //    new DataColumn("ZipCode", typeof(string)) });

            //int rowNumber = 0;
            //for (int i = 0; i < tableA.Rows.Count; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        table.Rows.Add();
            //        table.Rows[rowNumber][0] = tableA.Rows[i][0]; //adding company name
            //        table.Rows[rowNumber][1] = tableA.Rows[i][2]; //adding City

            //    }
            //    else
            //    {
            //        table.Rows[rowNumber][2] = tableA.Rows[i][2]; //adding Country
            //        table.Rows[rowNumber++][3] = tableA.Rows[i + 1][2]; //adding Zipcode
            //        i++;
            //    }
            //}

            //DataTable dtNew = new DataTable();

            ////adding columns    
            //for (int i = 0; i <= dt.Rows.Count; i++)
            //{
            //    dtNew.Columns.Add(i.ToString());
            //}



            ////Changing Column Captions: 
            //dtNew.Columns[0].ColumnName = " ";

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    //For dateTime columns use like below
            //    dtNew.Columns[i + 1].ColumnName = Convert.ToDateTime(dt.Rows[i].ItemArray[0].ToString()).ToString("MM/dd/yyyy");
            //    //Else just assign the ItermArry[0] to the columnName prooperty
            //}

            ////Adding Row Data
            //for (int k = 1; k < dt.Columns.Count; k++)
            //{
            //    DataRow r = dtNew.NewRow();
            //    r[0] = dt.Columns[k].ToString();
            //    for (int j = 1; j <= dt.Rows.Count; j++)
            //        r[j] = dt.Rows[j - 1][k];
            //    dtNew.Rows.Add(r);
            //}

            //using (XLWorkbook wbs = new XLWorkbook())
            //{
            //    var workbook = new XLWorkbook("BasicTable.xlsx");
            //    var ws = workbook.Worksheet(1);

            //    var rngTable = ws.Range("A1:P1");

            //    rngTable.Transpose(XLTransposeOptions.MoveCells);

            //    ws.Columns().AdjustToContents();

            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        wbs.SaveAs(stream);


            //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "Tranpose" + ".xlsx");
            //    }
            //}
            //XLBorderStyleValues bs = XLBorderStyleValues.Thick;
            //dt..Border.SetOutsideBorder(bs);

            using (XLWorkbook wbs = new XLWorkbook())
            {
               
//                cellSheetHeading.SetValue("THIS IS A TEST HEADER");
//cellSheetHeading.Style.Font.FontSize = 18
//cellSheetHeading.Style.Font.SetBold()
//cellSheetHeading.Style.Font.FontColor = XLColor.Blue
//cellSheetHeading.WorksheetRow.Height = 50
//workSheet.Range("A1:D1").Row(1).Merge()
                //wbs.Worksheets.Add(dt);
                var W_WorkSheet111 = wbs.Worksheets.Add(dt,"ProjeDetay");
                W_WorkSheet111.Tables.FirstOrDefault().Style.Font.FontColor = XLColor.Black;
                //W_WorkSheet111.FirstRow().FirstCell().InsertData(dt.Rows);
                W_WorkSheet111.Style.Font.FontColor = XLColor.Black; 

                W_WorkSheet111.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                //W_WorkSheet111.Range("A1:P1").Row(1).Merge();
                //var W_WorkSheet = wbs.Worksheets.Add(tableA, "ProjeDetaysssss");
                //var W_WorkSheets = wbs.Worksheets.Add(dtNew, "Proje");
                W_WorkSheet111.Rows().AdjustToContents();
                W_WorkSheet111.Columns().AdjustToContents();
                W_WorkSheet111.Tables.FirstOrDefault().ShowAutoFilter = false;
                W_WorkSheet111.Style.Fill.BackgroundColor = XLColor.White;
                var rngTable = W_WorkSheet111.Range("A1:P20");

                rngTable.Transpose(XLTransposeOptions.MoveCells);
                rngTable.Style.Font.FontColor = XLColor.Black;
                //rngTable.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                //W_WorkSheet111.Columns().AdjustToContents();
                W_WorkSheet111.Rows().AdjustToContents();// Sütunların içerigine göre sütünları genişletir
     
                using (MemoryStream stream = new MemoryStream())
                {
                    wbs.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "ProjeDetay" + ".xlsx");
                }


            }

        }
        #endregion
        #region PROJE GENEL EXCEL
        [HttpPost]
        public FileResult ProjeExcelOlustur(ARGE_Proje proje)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[11]{
                new DataColumn("PROJE KODU"),
                      new DataColumn("PROJE KISA ADI"),
                new DataColumn("PROJE ADI"),
                           new DataColumn("PROJE DURUMU"),
                    new DataColumn("PROJE BAŞLANGIÇ TARİHİ"),
                        new DataColumn("PROJE BİTİŞ TARİHİ"),
                             new DataColumn("PERSONEL SAYISI"),
                            new DataColumn("PROJE YÜRÜTÜCÜSÜ"),
                             new DataColumn("MALİYET ÖNGÖRÜSÜ"),

                                       new DataColumn("TOPLAM ADAM AY"),
                              new DataColumn("DESTEK DURUMU"),
            });
            var liste = from list in db.ARGE_Proje.OrderByDescending(a => a.ProjeDurum).ThenByDescending(a => a.ProjeBaslangicTarihi) select list;



            foreach (var list in liste) { 
                dt.Rows.Add(list.ProjeKodu, list.ProjeKisaAd, list.ProjeAdi, list.ProjeDurum ? "AKTİF PROJE" : "PASİF PROJE", list.ProjeBaslangicTarihi.Value.ToString("dd.MM.yyyy"),
                        list.ProjeBitisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"), list.PersonelSayisi, list.ARGE_Personel.AdSoyad, list.MaliyetOngorusu.ToString("#,##0"), list.ToplamAdamAy, list.ProjeDestekDurumu);

                }

                dt.Rows.Add("F-32.01/00");


                using (XLWorkbook wb = new XLWorkbook())
            {
                var W_WorkSheet = wb.Worksheets.Add(dt, "Proje");
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
    }
}
