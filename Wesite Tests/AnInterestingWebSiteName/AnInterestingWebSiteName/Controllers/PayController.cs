using AnInterestingWebSiteName.Classes;
using AnInterestingWebSiteName.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AnInterestingWebSiteName.Controllers
{
    public class PayModel
    {
        public string SaticiKodu { get; set; }
        public string SaticiSifre { get; set; }
        public string KartNo { get; set; }
        public string SonKullanmaAy { get; set; }
        public string SonKullanmaYil { get; set; }
        public string CVCKodu { get; set; }
        public decimal Bakiye { get; set; }
    }
    public class PayController : Controller
    {
        AnInterestingWebSiteName_Model db = new AnInterestingWebSiteName_Model();
        // GET: Pay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int? id)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            if (id == null || db.Urunlers.Find(id) == null)
            {
                TempData["message"] = "Ürün Bulunamadı";
                return RedirectToAction("Index", "Liste");
            }

            if (db.Urunlers.Find(id).IndirimliFiyat != 0)
            {
                TempData["message"] = "Ürün Bedava Değil";
                return RedirectToAction("Index", "Liste");
            }

            Kutuphane kui = new Kutuphane();

            Kullanici k = (Kullanici)Session["userSession"];

            kui.Kullanici_ID = k.ID;

            kui.Oyun_ID = (int)id;

            db.Kutuphanes.Add(kui);

            db.SaveChanges();

            return RedirectToAction("Index", "Kutuphane");
        }

        public ActionResult DoPay(int? id)
        {
            List<Urunler> ul = null;
            if (id != null)
            {
                if (db.Urunlers.Count(s => s.ID == id && s.Aktifmi == true) > 0)
                {
                    Urunler u = db.Urunlers.Find(id);
                    if (u.IndirimliFiyat != 0)
                    {
                        if (u.Fiyat != 0)
                        {
                            if (Session["sepetdekiUrunler"] == null)
                            {
                                List<Urunler> gecicitvuac = new List<Urunler>();

                                gecicitvuac.Add(db.Urunlers.FirstOrDefault(s => s.ID == id && s.Aktifmi == true));

                                Session["sepetdekiUrunler"] = gecicitvuac;
                            }
                            else
                            {

                                bool e = false;

                                foreach (var item in (List<Urunler>)Session["sepetdekiUrunler"])
                                {
                                    if (item.ID == id)
                                    {
                                        e = true;
                                    }
                                }

                                if (!e)
                                {
                                    List<Urunler> gecicitvuac = (List<Urunler>)Session["sepetdekiUrunler"];

                                    gecicitvuac.Add(db.Urunlers.FirstOrDefault(s => s.ID == id && s.Aktifmi == true));

                                    Session["sepetdekiUrunler"] = gecicitvuac;
                                }
                            }
                            Session.Timeout = 120;
                        }
                    }
                }
            }
            ul = (List<Urunler>)Session["sepetdekiUrunler"];

            if (ul != null)
            {
                double a = 0;
                foreach (var item in ul)
                {
                    a += (double)item.IndirimliFiyat;
                }
                ViewBag.totalFiyat = a;
            }

            if (ul != null)
            {
                return View((List<Urunler>)ul);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Pay()
        {

            if (Session["userSession"] == null)
            {
                TempData["message"] = "Giriş Süresi Dolmuştur";
                return RedirectToAction("Index", "UserLogin");
            }

            //TempData["errorMessage"] = "Test";

            List<SelectListItem> ay = new List<SelectListItem>();
            for (int i = 0; i <= 12; i++)
            {
                ay.Add(new SelectListItem { Text = Convert.ToString(i), Value = Convert.ToString(i), Selected = false });
            }
            ViewBag.Ay = ay;

            List<SelectListItem> yil = new List<SelectListItem>();
            for (int i = 0; i <= 30; i++)
            {
                yil.Add(new SelectListItem { Text = Convert.ToString(Convert.ToInt32(DateTime.Now.Year) + i), Value = Convert.ToString(Convert.ToInt32(DateTime.Now.Year) + i), Selected = false });
            }
            ViewBag.Yil = yil;

            return View();
        }

        [HttpPost]
        public ActionResult Pay(string Name, string Surname, string reqYear, string reqMonth, string CVC, string CardNo)
        {
            //ne yaptığımı bilmiyorum



            if (Session["userSession"] == null)
            {
                TempData["message"] = "Giriş Süresi Dolmuştur";
                return RedirectToAction("Index", "UserLogin");
            }

            #region Ay Yıl Liste Yenile
            List<SelectListItem> ay = new List<SelectListItem>();
            for (int i = 0; i <= 12; i++)
            {
                ay.Add(new SelectListItem { Text = Convert.ToString(i), Value = Convert.ToString(i), Selected = false });
            }
            ViewBag.Ay = ay;

            List<SelectListItem> yil = new List<SelectListItem>();
            for (int i = 0; i <= 30; i++)
            {
                yil.Add(new SelectListItem { Text = Convert.ToString(Convert.ToInt32(DateTime.Now.Year) + i), Value = Convert.ToString(Convert.ToInt32(DateTime.Now.Year) + i), Selected = false });
            }
            ViewBag.Yil = yil;
            #endregion

            string etcstr = "";

            byte b = 0;

            foreach (char item in reqYear)
            {
                if (b > 1)
                {
                    etcstr += item;
                }
                b++;
            }

            reqYear = etcstr;

            List<Urunler> ul = (List<Urunler>)Session["sepetdekiUrunler"];

            Kullanici kullanici = (Kullanici)Session["userSession"];

            decimal d = 0;

            if (ul == null)
            {
                return RedirectToAction("DoPay", "Pay");
            }

            foreach (var item in ul)
            {
                d += Convert.ToDecimal(item.IndirimliFiyat);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44351/api/Pay");

                PayModel pm = new PayModel();
                pm.SonKullanmaYil = reqYear;
                pm.SonKullanmaAy = reqMonth;
                pm.SaticiSifre = "1234567890";
                pm.SaticiKodu = "123456789012";
                pm.CVCKodu = CVC;
                pm.Bakiye = d;
                pm.KartNo = CardNo;

                var postTask = client.PostAsJsonAsync("Pay", pm);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var stringResp = result.Content.ReadAsStringAsync();
                    string resultCode = stringResp.Result;
                    if (resultCode == "\"11111\"")
                    {
                        TempData["message"] = "Ödeme Başarılı! <br/> Toplam Çekilen Para: " + d;
                        foreach (var item in ul)
                        {
                            Kutuphane kutuphane = new Kutuphane();
                            kutuphane.Kullanici_ID = kullanici.ID;
                            kutuphane.Oyun_ID = item.ID;
                            db.Kutuphanes.Add(kutuphane);
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index", "Kutuphane");
                    }
                    else if (resultCode == "\"67953\"")
                    {
                        TempData["errorMessage"] = "Sanal Post Bulunamadı";
                    }
                    else if (resultCode == "\"97624\"")
                    {
                        TempData["errorMessage"] = "Sanal Post Aktif Değil";
                    }
                    else if (resultCode == "\"97618\"")
                    {
                        TempData["errorMessage"] = "Kart Bulunamadı";
                    }
                    else if (resultCode == "\"34976\"")
                    {
                        TempData["errorMessage"] = "Yanlış Bilgi Girildi";//son kullanma yıl hatası
                    }
                    else if (resultCode == "\"61675\"")
                    {
                        TempData["errorMessage"] = "Yanlış Bilgi Girildi";//son kullanma ay hatası
                    }
                    else if (resultCode == "\"31864\"")
                    {
                        TempData["errorMessage"] = "Yanlış Bilgi Girildi";//cvc yanlış
                    }
                    else if (resultCode == "\"94628\"")
                    {
                        TempData["errorMessage"] = "Kart Aktif Değil";
                    }
                    else if (resultCode == "\"96483\"")
                    {
                        TempData["errorMessage"] = "Kartın Yeterli Bakiyesi Yok";
                    }
                }
            }

            TempData["sepetdekiUrunler"] = (List<Urunler>)TempData["ul"];

            return View();
        }

        public ActionResult Empty()
        {
            Session["sepetdekiUrunler"] = null;

            return RedirectToAction("DoPay", "Pay");
        }

        public ActionResult RemoveItem(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("DoPay", "Pay");
            }
            else if (db.Urunlers.Count(s => s.Aktifmi == true && s.ID == id) < 1)
            {
                return RedirectToAction("DoPay", "Pay");
            }

            List<Urunler> ul = null;

            foreach (var item in (List<Urunler>)Session["sepetdekiUrunler"])
            {
                if (item.ID != id)
                {
                    if (ul == null)
                    {
                        List<Urunler> gecicitvuac = new List<Urunler>();

                        gecicitvuac.Add(item);

                        ul = gecicitvuac;
                    }
                    else
                    {

                        bool e = false;

                        foreach (var item2 in (List<Urunler>)ul)
                        {
                            if (item2.ID == id)
                            {
                                e = true;
                            }
                        }

                        if (!e)
                        {
                            List<Urunler> gecicitvuac = ul;

                            gecicitvuac.Add(item);

                            ul = gecicitvuac;
                        }
                    }
                }
            }

            Session["sepetdekiUrunler"] = ul;

            return RedirectToAction("DoPay", "Pay");
        }

    }
}