using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class ProjeDeneme
    {
        public ProjeDeneme()
        {
            //BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            //BitisTarihiDateTime = DateTime.Now;
         
            TefrikRaporOzetItems = new List<ARGE_ProjeDenemeleri>();
        }

        //public string BaslangicTarihi { get; set; }

        //public string BitisTarihi { get; set; }

        //[DataType(DataType.Date)]

        //public DateTime BaslangicTarihiDateTime { get; set; }


        //public DateTime BitisTarihiDateTime { get; set; }
        public string ProjeKods { get; set; }

        public List<ARGE_ProjeDenemeleri> TefrikRaporOzetItems { get; set; }
    }
}