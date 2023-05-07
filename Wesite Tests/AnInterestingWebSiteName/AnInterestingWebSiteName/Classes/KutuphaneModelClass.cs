using AnInterestingWebSiteName.Areas.Admin.Controllers;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Classes
{
    public class KutuphaneModelClass
    {
        public Urunler Urun { get; set; }

        public IEnumerable<Kutuphane> Kutuphane { get; set; }

        public IEnumerable<Urunler> Urunler { get; set; }

        public IEnumerable<Firma> Firma { get; set; }

        public IEnumerable<TagsVeUrunAraClass> tagsVeUrunAraClasses { get; set; }

        public IEnumerable<Tag> Tag { get; set; }
    }
}