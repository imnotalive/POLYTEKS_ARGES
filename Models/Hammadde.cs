using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class Hammadde
    {
        public Hammadde()
        {
            BaslangicTarihiDateTime = DateTime.Now.AddDays(-1);
            BitisTarihiDateTime = DateTime.Now;
            TefrikRaporOzetItems = new List<ARGE_Hammadde>();
        }

        [DataType(DataType.Date)]

        public DateTime BaslangicTarihiDateTime { get; set; }


        public DateTime BitisTarihiDateTime { get; set; }

        public List<ARGE_Hammadde> TefrikRaporOzetItems { get; set; }
    }
}