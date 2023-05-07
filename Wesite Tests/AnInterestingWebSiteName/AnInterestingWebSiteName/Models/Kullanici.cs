using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace AnInterestingWebSiteName.Models
{
    [Serializable]
    public class Kullanici
    {
        public int ID { get; set; }

        [Display(Name ="Kullanıcı Adı")]
        [Required]
        [StringLength(64,MinimumLength =4)]
        public string Ad { get; set; }

        [Display(Name ="Şifre")]
        [Required]
        [StringLength(20,MinimumLength =8)]
        public string Sifre { get; set; }

        [Display(Name = "Mail")]
        [Required]
        [StringLength(128,MinimumLength =7)]
        public string Mail { get; set; }

        [Display(Name ="Açıklama")]
        [DataType(DataType.MultilineText)]
        [StringLength(4096)]
        public string Aciklama { get; set; }

        [Display(Name ="Profil Fotoğrafı")]
        public string ProfilResmi { get; set; }

        public bool Aktifmi { get; set; }
    }
}