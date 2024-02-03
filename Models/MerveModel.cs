using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class MerveModel
    {
        public MerveModel()
        {
            ARGE_FikirHavuzus = new List<ARGE_FikirHavuzu>();
        }
        public List<ARGE_FikirHavuzu> ARGE_FikirHavuzus { get; set; }
    }
}