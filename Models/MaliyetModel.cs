using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class MaliyetModel
    {
        public MaliyetModel()
        {
            BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            BitisTarihiDateTime = DateTime.Now;
            MaliyetRaporOzel = new List<ARGE_Maliyet>();
        }

        //public string BaslangicTarihi { get; set; }

        //public string BitisTarihi { get; set; }

        [DataType(DataType.Date)]

        public DateTime BaslangicTarihiDateTime { get; set; }


        public DateTime BitisTarihiDateTime { get; set; }

        public List<ARGE_Maliyet> MaliyetRaporOzel { get; set; }
    }
}