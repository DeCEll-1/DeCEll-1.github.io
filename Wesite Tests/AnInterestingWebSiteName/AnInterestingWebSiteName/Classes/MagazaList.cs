using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Classes
{
    public class MagazaList
    {
        public IEnumerable<Urunler> Urunler { get; set; }

        public IEnumerable<OyunResimleri> OyunResimleri { get; set; }

        public IEnumerable<TagsVeUrunAraClass> TagsVeUrunAraClass { get; set; }

        public IEnumerable<Tag> Tag { get; set; }
    }
}