using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace POLYTEKS_ARGE.Models
{
    public class ProjeGuncelBilgiModel
    {
        public ProjeGuncelBilgiModel()
        {
            //BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            //BitisTarihiDateTimeg = DateTime.Now;
            //SecmeliProjeKodu = new List<ARGE_ProjeGuncelBilgi>();
            GuncelBilgiRapors = new List<ARGE_ProjeGuncelBilgi>();
            ArgeProjes = new List<ARGE_ProjeGuncelBilgi>();
        }

        //public string BaslangicTarihi { get; set; }

        //public string BitisTarihi { get; set; }

        //[DataType(DataType.Date)]

        //public DateTime BaslangicTarihiDateTime { get; set; }


        //public DateTime BitisTarihiDateTime { get; set; }
        public SelectList SecmeliProjeKodus { get; set; }
        public string ProjeKodus { get; set; }
        public SelectList GunlukTalimatTipiDrop { get; set; }

        //public SelectList Partiler { get; set; }
        public string TalimatBasliks { get; set; }
        public string id { get; set; }
        public List<ARGE_ProjeGuncelBilgi> SecmeliProjeKodu { get; set; }
        public List<ARGE_ProjeGuncelBilgi> GuncelBilgiRapors { get; set; }
        public List<ARGE_ProjeGuncelBilgi> ArgeProjes { get; set; }

    }
}