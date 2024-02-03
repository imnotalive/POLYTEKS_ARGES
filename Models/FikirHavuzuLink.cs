using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class FikirHavuzuLink
    {
        public  FikirHavuzuLink()
        {
            FikirHavuzus = new ARGE_FikirHavuzu();
            FikirHavuzuss = new List<ARGE_FikirHavuzu>();
            Projes = new List<ARGE_Proje>();
        }
        public ARGE_FikirHavuzu FikirHavuzus { get; set; }
        public List<ARGE_FikirHavuzu> FikirHavuzuss { get; set; }
        public List<ARGE_Proje> Projes { get; set; }

    }
}