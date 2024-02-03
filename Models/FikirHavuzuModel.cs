using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class FikirHavuzuModel
    {
        public FikirHavuzuModel()
        {
            BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            BitisTarihiDateTime = DateTime.Now;
            FikirHavuzuRapors = new List<ARGE_FikirHavuzu>();
        }

        //public string BaslangicTarihi { get; set; }

        //public string BitisTarihi { get; set; }

        [DataType(DataType.Date)]

        public DateTime BaslangicTarihiDateTime { get; set; }

     
        public DateTime BitisTarihiDateTime { get; set; }

        public List<ARGE_FikirHavuzu> FikirHavuzuRapors { get; set; }
    }
}