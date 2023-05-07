using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Models
{
    [Serializable]
    public class Urunler
    {

        public int ID { get; set; }

        public int Yapimci_ID { get; set; }

        [ForeignKey("Yapimci_ID")]
        public Firma Yapimci { get; set; }

        public int? Yayinci_ID { get; set; }

        [ForeignKey("Yayinci_ID")]
        public Firma Yayinci { get; set; }


        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage = "Lütfen Bu Alanı Doldurunuz")]
        [StringLength(64, ErrorMessage = "Bu Alan En Fazla 64 Karakter Olabilir")]
        public string Ad { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Ürün Açıklaması")]
        [Required(ErrorMessage = "Lütfen Bu Alanı Doldurunuz")]
        [StringLength(1024)]
        public string Aciklama { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Ürün Altdaki İlk Açıklaması")]
        [Required(ErrorMessage = "Lütfen Bu Alanı Doldurunuz")]
        public string AciklamaAlt1 { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Ürün Altdaki İkinci Açıklaması")]
        public string AciklamaAlt2 { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Ürün Altdaki Üçüncü Açıklaması")]
        public string AciklamaAlt3 { get; set; }

        [Display(Name = "Ürünün Fiyatı")]
        [Required(ErrorMessage = "Lütfen Bu Alanı Doldurunuz")]
        public double Fiyat { get; set; }

        [Range(0, 100)]
        [Display(Name = "İndirim")]
        public byte? Indirim { get; set; }

        [Display(Name = "İndirimli Fiyat")]
        public double? IndirimliFiyat { get; set; }

        [Required(ErrorMessage = "Lütfen Fotoğraf Giriniz")]
        [Display(Name = "Logo")]
        [StringLength(155)]
        public string IkonYolu { get; set; }

        [Required(ErrorMessage = "Lütfen Fotoğraf Giriniz")]
        [Display(Name = "Tam Resim")]
        [StringLength(155)]
        public string TamResimYolu { get; set; }

        [Required(ErrorMessage = "Lütfen Fotoğraf Giriniz")]
        [Display(Name = "Arka Plan Resmi")]
        [StringLength(155)]
        public string ArkaPlanResmi { get; set; }

        [Required]
        public DateTime YayinTarihi { get; set; }

        public bool Aktifmi { get; set; }
    }
}