using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Models
{
    [Serializable]
    public class TagsVeUrunAraClass
    {
        public int ID { get; set; }

        [Required]
        public int Tag_ID { get; set; }

        [ForeignKey("Tag_ID")]
        public Tag Tag { get; set; }

        [Required]
        public int Urun_ID { get; set; }

        [ForeignKey("Urun_ID")]
        public Urunler Urunler { get; set; }
    }
}