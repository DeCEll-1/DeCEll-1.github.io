using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Models
{
    [Serializable]
    public class Kutuphane
    {
        public int ID { get; set; }

        public int Kullanici_ID { get; set; }

        [ForeignKey("Kullanici_ID")]
        public Kullanici Kullanici { get; set; }

        public int Oyun_ID { get; set; }

        [ForeignKey("Oyun_ID")]
        public Urunler Urunler { get; set; }


    }
}