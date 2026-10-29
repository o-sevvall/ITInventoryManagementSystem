using ITInventoryProject.Models;
using ITInventoryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITInventoryProject.Controllers
{
    public class YaziciController : Controller
    {
        // GET: Yazici Yazıcıların ana sayfasını gösterir
        public ActionResult Index(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);

                var veri = new AnaClass
                {
                    // Gelen ID'ye göre yazıcı bilgilerini alır
                    yaziciClass = gelenid == 0 ? new yaziciClass() : db.Yazici.AsNoTracking().Where(x => x.YaziciID == gelenid && x.Durum == 2).Select(x => new yaziciClass()
                    {
                        yaziciId = x.YaziciID,
                        yaziciTurId = x.YaziciTurID,
                        ButceId = x.ButceID,
                        Tip=x.Tip,                   
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        PersonelId = x.PersonelID,
                        SeriNo = x.SeriNo
                        
                    }).First(),
                    // Aktif personel listesini alır
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif bilgisayar türleri listesini alır
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçeler listesini alır
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),
                    // Aktif yazıcı türleri listesini alır
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),

                };
                return View(veri);

            }

        }
        // Yazıcı tanımlama sayfasını gösterir
        public ActionResult tanimla(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);

                var veri = new AnaClass
                {
                    // Gelen ID'ye göre yazıcı bilgilerini alır
                    yaziciClass = gelenid == 0 ? new yaziciClass() : db.Yazici.AsNoTracking().Where(x => x.YaziciID == gelenid && x.Durum == 2).Select(x => new yaziciClass()
                    {
                        yaziciId = x.YaziciID,
                        yaziciTurId = x.YaziciTurID,
                        ButceId = x.ButceID,
                        Ip =x.IP,
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        PersonelId = x.PersonelID,
                        SeriNo = x.SeriNo

                    }).First(),
                    // Aktif personel listesini alır
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif bilgisayar türleri listesini alır
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçeler listesini alır
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),
                    // Aktif yazıcı türleri listesini alır
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),

                };
                return View(veri);

            }

        }
        // Yazıcı ekleme veya düzenleme işlemi
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]

        public ActionResult EkleDuzenle(AnaClass yazici)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                try
                {
                    if (yazici.yaziciClass.yaziciId == 0)
                    {
                        // Yeni yazıcı ekleme
                        var veri = new Yazici()
                        {
                            YaziciTurID = yazici.yaziciClass.yaziciTurId,
                            Aciklama = yazici.yaziciClass.aciklama,
                            ButceID = yazici.yaziciClass.ButceId,
                            PersonelID = yazici.yaziciClass.PersonelId,
                            Marka = yazici.yaziciClass.Marka,
                            Model = yazici.yaziciClass.Model,
                            IP = yazici.yaziciClass.Ip,
                            SeriNo = yazici.yaziciClass.SeriNo,
                            Tip = yazici.yaziciClass.Tip,
                          
                            AktifMi = true,
                            Durum=2,
                        };

                        db.Yazici.Add(veri);
                        db.SaveChanges();
                        // TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Basarili, "Ekleme işlemi başarılı.");
                        return RedirectToAction("Liste", "Yazici");
                    }
                    else
                    {
                        // Yazıcıyı güncelleme
                        var yazici1 = db.Yazici.FirstOrDefault(x => x.YaziciID == yazici.yaziciClass.yaziciId);
                        if (yazici1 != null)
                        {
                            yazici1.YaziciTurID = yazici.yaziciClass.yaziciTurId;
                            yazici1.Aciklama = yazici.yaziciClass.aciklama;
                            yazici1.IP = yazici.yaziciClass.Ip;

                            yazici1.ButceID = yazici.yaziciClass.ButceId;
                            yazici1.PersonelID = yazici.yaziciClass.PersonelId;
                            yazici1.Tip = yazici.yaziciClass.Tip;
                            yazici1.SeriNo = yazici.yaziciClass.SeriNo;
                            yazici1.Marka = yazici.yaziciClass.Marka;
                            yazici1.Model = yazici.yaziciClass.Model;
                      
                            db.SaveChanges();
                            return RedirectToAction("Liste", "Yazici");
                        }
                        else
                        {

                            return RedirectToAction("Liste", "Yazici");
                        }
                    }
                }
                catch (Exception exc)
                {
                    // Hata yönetimi: Hata meydana geldi
                    // TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Hata, "Bir hata oluştu: " + exc.Message);
                    return View("Tanimla");
                }
            }
        }
        // Yazıcı listesini gösterir
        public ActionResult Liste(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                var veri = new AnaClass
                {
                    yaziciList = db.Yazici.AsNoTracking().Where(x => x.Durum == 2).Select(x => new yaziciClass
                    {
                        yaziciId = x.YaziciID,
                        yaziciturAd = x.YaziciTuru.YaziciTurAdi,
                        ButceAd = x.Butce.ButceAdi,
                        Ip = x.IP,
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        personelAd = x.Personel.Ad + " " + x.Personel.Soyad,
                        SeriNo = x.SeriNo,
                        Tip =x.Tip,

                    }).ToList(),
                    // Aktif personel listesini alır
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif bilgisayar türleri listesini alır
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçeler listesini alır
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),
                    // Aktif yazıcı türleri listesini alır
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),
                };
                return View(veri);
            }
        }

        public ActionResult Listeterkin(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                var veri = new AnaClass
                {
                    // Aktif yazıcıların listesini alır
                    yaziciList = db.Yazici.AsNoTracking().Where(x => x.Durum == 2).Select(x => new yaziciClass
                    {
                        yaziciId = x.YaziciID,
                        yaziciTurId = x.YaziciTurID,
                        ButceId = x.ButceID,
                        Ip = x.IP,
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        PersonelId = x.PersonelID,
                        SeriNo = x.SeriNo
                    }).ToList(),
                    // Aktif personel listesini alır
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif bilgisayar türleri listesini alır
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçeler listesini alır
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),
                    // Aktif yazıcı türleri listesini alır
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),
                };
                return View(veri);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Ambarat(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                // Eğer gelen ID pozitif ise işlemi gerçekleştir
                if (id > 0)
                {
                    // Veritabanından belirtilen ID'ye sahip yazıcı kaydı alınır
                    var blogdb = db.Yazici.FirstOrDefault(x => x.YaziciID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound();
                    }

                    // Silme işlemi
                    blogdb.Durum = 1;
                    db.SaveChanges();


                    return RedirectToAction("yaziciListe", "Ambar");
                }
                else
                {
                    return RedirectToAction("yaziciListe", "Ambar");
                }
            }
        }
        public ActionResult terkinAt(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            // OGMDatabase1Entities veri tabanı bağlamı oluşturulur
            using (var db = new OGMDatabase1Entities1())
            {
                if (id > 0)
                {
                    // Veritabanından belirtilen ID'ye sahip yazıcı kaydı alınır
                    var blogdb = db.Yazici.FirstOrDefault(x => x.YaziciID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound();
                    }

                    // Silme işlemi
                    blogdb.Durum = 0;
                    db.SaveChanges();


                    return RedirectToAction("yaziciListe", "Terkin");
                }
                else
                {
                    return RedirectToAction("yaziciListe", "Terkin");
                }
            }
        }

    }
}