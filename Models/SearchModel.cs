using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POLYTEKS_ARGE.Models
{
    public class SearchModel
    {
        public List<ARGE_DenemeIstek> DenemeIsteks { get; set; }
        public List<ARGE_FikirHavuzu> FikirHavuzus { get; set; }
        public List<ARGE_Hammadde> Hammaddes { get; set; }
        public List<ARGE_Maliyet> Maliyets { get; set; }
        public List<ARGE_Numune> Numunes { get; set; }
        public List<ARGE_Personel> Personels { get; set; }
        public List<ARGE_PersonelDisaridaGecirilenSure> PersonelDisaridaGecirilenSures { get; set; }
        public List<ARGE_Proje> Projes { get; set; }
        public List<ARGE_ProjeDenemeleri> ProjeDenemeleris { get; set; }
        public List<ARGE_ProjeGuncelBilgi> ProjeGuncelBilgis { get; set; }
        public List<ARGE_Sarfiyat> Sarfiyats { get; set; }
        public string SearchKey { get; set; }
        public int Search { get; set; }
        public class HtmlHelperExtensions
        {
            public static string UppercaseFirst(string s)
            {
                if (string.IsNullOrEmpty(s))
                {
                    return string.Empty;
                }
                char[] a = s.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                return new string(a);
            }
        }
    }
}
    
