using ITInventoryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITInventoryProject.Controllers
{
    public class HomeController : Controller
    {
        // Ana sayfayı döndüren metod
        public ActionResult Index()
        {
            // Kullanıcı oturumu açmamışsa giriş sayfasına yönlendirilir.
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            return View();
        }

        // Giriş sayfası döndürülür ve kullanıcı girişi kontrol edilir.
        public ActionResult Login()
        {
            if (Request["giris"] != null)
            {
                var username = Request["username"];
                var password = Request["password"];
                using (var db = new OGMDatabase1Entities1())
                {
                    var user = db.Kullanicilar.FirstOrDefault(x => x.Username == username && x.Sifre == password && x.AktifMi == true);
                    if (user != null)
                    {
                        // Oturum değişkenleri ayarlandı
                        Session["KullaniciID"] = user.KullaniciID;
                        Session["AdSoyad"] = user.AdSoyad;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Hata = "Kullanıcı adı veya şifre yanlış.";
                    }
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult GetSalesData()
        {
            using (var db = new OGMDatabase1Entities1())
            {
                var bilgisayarsayisi = db.Bilgisayar.Count(x => x.AktifMi == true);
                var monisayisi = db.Monitor.Count(x => x.AktifMi == true);
                var yazicisayisi = db.Yazici.Count(x => x.AktifMi == true);
                var ambar = db.Bilgisayar.Count(x => x.Durum == 2);
                var terkin = db.Bilgisayar.Count(x => x.Durum == 1);
                // Ambardaki bilgisayar, monitör ve yazıcı sayıları
                var ambarbilgisayarsayisi = db.Bilgisayar.Count(x => x.Durum == 2); // Durum 2 olanlar ambardaki bilgisayarlar
                var ambarmonisayisi = db.Monitor.Count(x => x.Durum == 2); // Durum 2 olanlar ambardaki monitörler
                var ambaryazicisayisi = db.Yazici.Count(x => x.Durum == 2); // Durum 2 olanlar ambardaki yazıcılar
                // Terkindeki bilgisayar, monitör ve yazıcı sayıları
                var terkinbilgisayar = db.Bilgisayar.Count(x => x.AktifMi == null);
                var terkinmonisayisi = db.Monitor.Count(x => x.AktifMi == null); 
                var terkinyazicisayisi = db.Yazici.Count(x => x.AktifMi == null); 
                // Verileri JSON formatına dönüştür
                var data = new
                {
                    labels = new[] { "Bilgisayar", "Monitör", "Yazıcı", "Ambar", "Ambardaki Bilgisayar", "Ambardaki Monitör", "Ambardaki Yazıcı", "Terkin", "Terkindeki Bilgisayar",
                    "Terkindeki Monitor Sayısı","Terkindeki Yazıcı Sayısı"},
                    values = new[] { bilgisayarsayisi, monisayisi, yazicisayisi, ambar, ambarbilgisayarsayisi, ambarmonisayisi, ambaryazicisayisi,terkin,terkinbilgisayar,terkinmonisayisi,terkinyazicisayisi }
                };

                return Json(data);
            }
        }

        public ActionResult CikisYap()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Profil()
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");

            if (Request["kaydet"] != null)
            {
                var password = Request["password"];
                using (var db = new OGMDatabase1Entities1())
                {
                    var userId = int.Parse(Session["KullaniciID"].ToString());
                    var user = db.Kullanicilar.FirstOrDefault(x => x.KullaniciID == userId);
                    user.Sifre = password;
                    db.SaveChanges();
                        ViewBag.Basarili = "İşlem başarılı.";
                }
            }
            return View();
        }
    }
}