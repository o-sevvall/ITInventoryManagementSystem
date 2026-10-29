using ITInventoryProject.Models;
using ITInventoryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITInventoryProject.Controllers
{
    public class AmbarController : Controller
    {
        // GET: Ambar   Ana sayfa için bilgisayar detaylarını gösterir.
        public ActionResult Index(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            // OGMDatabase1Entities veri tabanı bağlamı oluşturulur
            using (var db = new OGMDatabase1Entities1())
            {
                // Gelen ID'yi integer olarak parse eder
                int.TryParse(id, out var gelenid);

                var veri = new AnaClass
                {
                    pcClass = gelenid == 0 ? new BilgisayarClass() : db.Bilgisayar.AsNoTracking().Where(x => x.BilgisayarID == gelenid && x.Durum == 2).Select(x => new BilgisayarClass()
                    {
                        BilgisayarId = x.BilgisayarID,
                        BilgisayarTurId = x.BilgisayarTurID,
                        ButceId = x.ButceID,
                        DomainNo = x.DomainNo,
                        HDD = x.HDD,
                        IslemciModeli = x.IslemciModeli,
                        IslemciTeknolojisi = x.IslemciTeknolojisi,
                        IslemciHizi = x.IslemciHizi,
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        PersonelId = x.PersonelID,
                        Ram = x.RAM,
                        SeriNo = x.SeriNo,
                        SSD = x.SSD
                    }).First(),
                    // Aktif personel listesi alınır
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif bilgisayar türleri alınır
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçeler alınır
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),

                };
                return View(veri);

            }

        }

        // Yazıcı listesini gösterir.
        public ActionResult yaziciListe(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                var veri = new AnaClass
                {
                    // Durumu 1 olan yazıcıları alır
                    yaziciList = db.Yazici.AsNoTracking().Where(x => x.Durum == 1).Select(x => new yaziciClass
                    {
                        yaziciId = x.YaziciID,
                        yaziciturAd = x.YaziciTuru.YaziciTurAdi,
                        ButceAd = x.Butce.ButceAdi,
                        Ip = x.IP,
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        personelAd = x.Personel.Ad + " " + x.Personel.Soyad,
                        SeriNo = x.SeriNo
                    }).ToList(),
                    // Aktif personel listesi alınır
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif bilgisayar türleri alınır
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçeler alınır
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),
                    // Aktif yazıcı türleri alınır
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),
                };
                return View(veri);
            }
        }
        // Yazıcıyı geri döndürmek için kullanılır.
        public ActionResult GeriDon(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                // Belirtilen ID'ye sahip yazıcıyı bulur
                if (id > 0)
                {
                    var blogdb = db.Yazici.FirstOrDefault(x => x.YaziciID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound();
                    }

                    blogdb.Durum = 2;
                    db.SaveChanges();


                    return RedirectToAction("yaziciListe", "Ambar");
                }
                else
                {
                    return RedirectToAction("yaziciListe", "Ambar");
                }
            }
        }
        // Monitör listesini gösterir.
        public ActionResult monitorListe(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                var veri = new AnaClass
                {
                    monitorList = db.Monitor.AsNoTracking().Where(x => x.Durum == 1).Select(x => new MonitorClass
                    {
                        MonitorId = x.MonitorID,
                        BilgisayarAd = x.Bilgisayar.SeriNo,
                        ButceAd = x.Butce.ButceAdi,
                        PersonelAd = x.Personel.Ad + " " + x.Personel.Soyad,
                        Marka = x.Marka,
                        Model = x.Model,
                        SeriNo = x.SeriNo,
                        aciklama = x.Aciklama,
                        BagliSeriNo = x.BagliSeriNo,
                        Boyut = x.Boyut,
                    }).ToList(),
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),
                };
                return View(veri);
            }
        }

        // Monitörü geri döndürmek için kullanılır.
        public ActionResult moniGeriDon(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                if (id > 0)
                {
                    var blogdb = db.Monitor.FirstOrDefault(x => x.MonitorID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound();
                    }

                    blogdb.Durum = 2;
                    db.SaveChanges();


                    return RedirectToAction("monitorListe", "Ambar");
                }
                else
                {
                    return RedirectToAction("monitorListe", "Ambar");
                }
            }
        }
        // pcListe metodu, aktif durumdaki bilgisayarların listesini alır ve görüntülemek için bir model oluşturur.
        public ActionResult pcListe(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                var veri = new AnaClass
                {
                    pcList = db.Bilgisayar.AsNoTracking().Where(x => x.Durum == 1).Select(x => new BilgisayarClass
                    {
                        BilgisayarId = x.BilgisayarID,
                        BilgisayarTurAdd = x.BilgisayarTuru.BilgisayarTurAd,
                        ButceAd = x.Butce.ButceAdi,
                        DomainNo = x.DomainNo,
                        HDD = x.HDD,
                        IslemciModeli = x.IslemciModeli,
                        IslemciTeknolojisi = x.IslemciTeknolojisi,
                        IslemciHizi = x.IslemciHizi,
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        personelAd = x.Personel.Ad + " " + x.Personel.Soyad,
                        Ram = x.RAM,
                        SeriNo = x.SeriNo,
                        SSD = x.SSD
                    }).ToList(),
                    // Aktif personel listesini veritabanından alır.
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif bilgisayar türlerini veritabanından alır.
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçeleri veritabanından alır.
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),
                    // Aktif yazıcı türlerini veritabanından alır.
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),
                };
                return View(veri);
            }
        }
        // Bilgisayarı geri döndürmek için kullanılır.
        public ActionResult pcGeriDon(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                // Belirtilen ID'ye sahip bilgisayarı bulur.
                if (id > 0)
                {
                    var blogdb = db.Bilgisayar.FirstOrDefault(x => x.BilgisayarID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound();
                    }

                    blogdb.Durum = 2;
                    db.SaveChanges();


                    return RedirectToAction("Liste", "Pc");
                }
                else
                {
                    return RedirectToAction("Liste", "Pc");
                }
            }
        }

    }
}