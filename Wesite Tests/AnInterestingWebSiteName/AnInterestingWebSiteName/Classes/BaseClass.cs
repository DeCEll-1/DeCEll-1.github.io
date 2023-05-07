using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Classes
{
    [Serializable]
    public class BaseClass
    {
        public string WebAdress { get; set; }

        public List<Firma> Firma { get; set; }

        public List<Urunler> Urunler { get; set; }

        public List<OyunResimleri> OyunResimleri { get; set; }

        public List<Kullanici> Kullanici { get; set; }

        public List<Kutuphane> Kutuphane{ get; set; }
            
        public List<Tag> Tag { get; set; }

        public List<TagsVeUrunAraClass> TagsVeUrunAraClass { get; set; }
            
        public List<YoneticiTur> YoneticiTur { get; set; }

        public List<Yonetici> Yonetici { get; set; }
    }
}