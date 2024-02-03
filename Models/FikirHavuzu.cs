using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class FikirHavuzu
    {
        public FikirHavuzu()
        {
            BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            BitisTarihiDateTime = DateTime.Now;
            TefrikRaporOzetItems = new List<ARGE_FikirHavuzu>();
        }

        //public string BaslangicTarihi { get; set; }

        //public string BitisTarihi { get; set; }

        [DataType(DataType.Date)]

        public DateTime BaslangicTarihiDateTime { get; set; }

     
        public DateTime BitisTarihiDateTime { get; set; }

        public List<ARGE_FikirHavuzu> TefrikRaporOzetItems { get; set; }
    }
}