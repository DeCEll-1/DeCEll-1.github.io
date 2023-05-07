using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AnInterestingWebSiteName.Models
{
    [Serializable]
    public class Firma
    {

        public int ID { get; set; }

        [Display(Name = "Firma Adı")]
        [Required(ErrorMessage = "Bu Alan Boş Bırakılamaz")]
        [MaxLength(256, ErrorMessage = "Bu Alan En Fazla 256 Karakter Olabilir")]
        public string Ad { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name ="Firma Hakkında")]
        [Required(ErrorMessage ="Bu Alan Boş Bırakılamaz")]
        public string FirmaHakkinda { get; set; }

        [Required]
        [Display(Name ="Firmanın Profil Resmi")]
        public string FirmaResmi { get; set; }

        [Required]
        [Display(Name = "Firmanın Arka Plan Resmi")]
        public string FirmaArkaPlanResmi { get; set; }

        [Display(Name = "Firmanın Maili")]
        [Required(ErrorMessage = "Bu Alan Boş Bırakılamaz")]
        [StringLength(128)]
        public string Mail { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool Aktifmi { get; set; }

    }
}