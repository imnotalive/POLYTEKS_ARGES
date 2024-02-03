using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class ProjeModel
    {
        public ProjeModel()
        {
            BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            BitisTarihiDateTime = DateTime.Now;
            ArgeProjeRaporOzet = new List<ARGE_Proje>();
            ArgeProjes = new List<ARGE_Proje>();
        }

        //public string BaslangicTarihi { get; set; }

        //public string BitisTarihi { get; set; }

        [DataType(DataType.Date)]

        public DateTime BaslangicTarihiDateTime { get; set; }
        public DateTime BitisTarihiDateTime { get; set; }

        public int id { get; set; }

        public List<ARGE_Proje> ArgeProjeRaporOzet { get; set; }

        public List<ARGE_Proje> ArgeProjes { get; set; }
    }
}