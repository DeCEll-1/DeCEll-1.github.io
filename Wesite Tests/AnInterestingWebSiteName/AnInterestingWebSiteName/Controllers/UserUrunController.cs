using AnInterestingWebSiteName.Classes;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Controllers
{
    public class UserUrunController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();

        // GET: UserUrun
        public ActionResult Index(int? id)
        {

            if (id == null || db.Urunlers.Find(id).Aktifmi == false || db.Urunlers.Find(id) == null)
            {
                TempData["message"] = "Ürün Bulunamadı";
                return RedirectToAction("Index", "Liste");
            }

            ProductViewUser pvu = new ProductViewUser();

            if (Session["userSession"] != null)
            {
                Kullanici k = (Kullanici)Session["userSession"];
                if (db.Kutuphanes.Count(s => s.Kullanici_ID == k.ID && s.Oyun_ID == id) > 0)
                {
                    pvu.Kutuphanedemi = true;
                }
                else
                {
                    pvu.Kutuphanedemi = false;
                }
            }


            pvu.Urun = db.Urunlers.Find(id);

            pvu.Urun.Yayinci = db.Firmas.Find(pvu.Urun.Yayinci_ID);

            pvu.Urun.Yapimci = db.Firmas.Find(pvu.Urun.Yapimci_ID);

            pvu.Tag = null;

            bool a = true;

            foreach (var item in db.TagsVeUrunAraClasses.ToList().Where(s => s.Urun_ID == pvu.Urun.ID))
            {

                if (a)
                {
                    List<Tag> gecicit = new List<Tag>();

                    gecicit.Add(db.Tags.FirstOrDefault(s => s.ID == item.Tag_ID));

                    pvu.Tag = gecicit;

                    a = false;
                }
                else
                {
                    List<Tag> gecicit = (List<Tag>)pvu.Tag;

                    gecicit.Add(db.Tags.FirstOrDefault(s => s.ID == item.Tag_ID));

                    pvu.Tag = gecicit;
                }
            }

            if (pvu.Urun.Yayinci_ID == pvu.Urun.Yapimci_ID)
            {
                foreach (var item in db.Urunlers.ToList().Where(s => s.Aktifmi == true && s.Yapimci_ID == pvu.Urun.Yapimci_ID))
                {

                    if (pvu.Urunler == null)
                    {
                        List<Urunler> geciciu = new List<Urunler>();

                        geciciu.Add(item);

                        pvu.Urunler = geciciu;
                    }
                    else
                    {
                        List<Urunler> geciciu = (List<Urunler>)pvu.Urunler;

                        geciciu.Add(item);

                        pvu.Urunler = geciciu;
                    }
                }
            }


            pvu.OyunResimleri = db.OyunResimleris.ToList().Where(s => s.Oyun_ID == pvu.Urun.ID);

            return View(pvu);
        }
    }
}