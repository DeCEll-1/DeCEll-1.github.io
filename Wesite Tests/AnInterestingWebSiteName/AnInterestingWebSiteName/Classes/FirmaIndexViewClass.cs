using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Classes
{
    public class FirmaIndexViewClass
    {

        public Firma Firma { get; set; }

        public IEnumerable<Urunler> Urunler { get; set; }
    }
}