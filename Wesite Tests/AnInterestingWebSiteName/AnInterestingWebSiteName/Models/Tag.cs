using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Models
{
    [Serializable]
    public class Tag
    {

        public int ID { get; set; }

        [Display(Name ="Etiket Adı")]
        [Required(ErrorMessage = "Bu Alan Boş Bırakılamaz")]
        [StringLength(64, ErrorMessage = "64 Karakterden Fazla Olamaz")]
        public string Ad { get; set; }
    }
}