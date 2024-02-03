using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POLYTEKS_ARGE.Models
{
    public class ProjeDenemeModel
    {
        public ProjeDenemeModel()
        {
            //BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            //BitisTarihiDateTime = DateTime.Now;

            ProjeDenemeRapors = new List<ARGE_ProjeDenemeleri>();
      
        }

        //public string BaslangicTarihi { get; set; }

        //public string BitisTarihi { get; set; }

        //[DataType(DataType.Date)]

        //public DateTime BaslangicTarihiDateTime { get; set; }


        //public DateTime BitisTarihiDateTime { get; set; }
        public string Prop1 { get; set; }

        public string Prop2 { get; set; }

        public string Prop3 { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<ARGE_ProjeDenemeleri> Customers { get; set; }

        public IEnumerable<SelectListItem> EntityType1List { get; set; }

        public IEnumerable<SelectListItem> EntityType2List { get; set; }

        public IEnumerable<SelectListItem> EntityType3List { get; set; }

        public List<ARGE_ProjeDenemeleri> ProjeDenemeRapors { get; set; }
    }
}