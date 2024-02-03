//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POLYTEKS_ARGE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ARGE_FikirHavuzu
    {
        public int ID { get; set; }
        public string FikirSahibi { get; set; }
        public string FikirKonusu { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string SonDurum { get; set; }
        public string Aciklama { get; set; }
        public Nullable<int> ProjeId { get; set; }
        public Nullable<int> GuncelDurumId { get; set; }
        public Nullable<int> OnayDurumId { get; set; }
    
        public virtual ARGE_FikirHavuzuGuncelDurum ARGE_FikirHavuzuGuncelDurum { get; set; }
        public virtual ARGE_FikirHavuzuOnayDurum ARGE_FikirHavuzuOnayDurum { get; set; }
        public virtual ARGE_Proje ARGE_Proje { get; set; }
    }
}
