using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnInterestingWebSiteName.Classes
{
    public class UserViewClass
    {

        public Kullanici Kullanici{ get; set; }

        public IEnumerable<Kutuphane> Kutuphane { get; set; }

        public IEnumerable<Urunler> Urunler{ get; set; }

        //https://youtu.be/1MHWKpSUwGk
        public bool Benim { get; set; }
    }
}