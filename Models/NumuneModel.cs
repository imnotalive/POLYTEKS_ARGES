using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POLYTEKS_ARGE.Models
{
    public class NumuneModel
    {
        public NumuneModel()
        {
            NumuneKontrol = new List<ARGE_Numune>();
            ARGE_Numune = new ARGE_Numune();
        }
        public ARGE_Numune ARGE_Numune { get; set; }
        public List<ARGE_Numune> NumuneKontrol { get; set; }
        public string PartiKodu { get; set; }
        public SelectList NumuneSelect { get; set; }
        public string numuneAdis { get; set; }
        public int SecilenId { get; set; }
    }
}