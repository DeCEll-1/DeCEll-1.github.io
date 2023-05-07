using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Models
{
    [Serializable]
    public class OyunResimleri
    {
        public int ID { get; set; }

        [Display(Name ="Oyun ID")]
        public int Oyun_ID { get; set; }

        [ForeignKey("Oyun_ID")]
        public virtual Urunler Urunler { get; set; }

        [Display(Name ="Resim")]
        public string Resim { get; set; }

        public bool Aktifmi { get; set; }
    }
}