using ITInventoryProject.Models;
using ITInventoryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITInventoryProject.Controllers
{
    public class TerkinController : Controller
    {
        // GET: Terkin
        public ActionResult Index(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                // id parametresi alındı ve gelenid değişkenine int olarak atandı
                int.TryParse(id, out var gelenid);
                // Eğer gelen id sıfırsa yeni bir Bilgisayar nesnesi oluşturulur, değilse veritabanından ilgili bilgisayar bilgileri alınır
                var veri = new AnaClass
                {
                    pcClass = gelenid == 0 ? new BilgisayarClass() : db.Bilgisayar.AsNoTracking().Where(x => x.BilgisayarID == gelenid && x.AktifMi == true).Select(x => new BilgisayarClass()
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
                    // Aktif olan personellerin listesi çekilir
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Aktif olan bilgisayar türleri çekilir
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(),
                    // Aktif bütçe listesi çekilir
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),

                };
                return View(veri);

            }

        }
        // Yazıcı listesi görüntülenir
        public ActionResult yaziciListe(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                // Yazıcı ve ilgili diğer bilgilerin listesi oluşturulur
                var veri = new AnaClass
                {
                    yaziciList = db.Yazici.AsNoTracking().Where(x => x.Durum == 0).Select(x => new yaziciClass
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
                    // Aktif personellerin listesi çekilir
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(),
                    // Bilgisayar türü listesi ve diğer veriler
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
                    // Yazıcı türü listesi çekilir
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),
                };
                return View(veri);
            }
        }
        // Belirtilen yazıcıyı geri getirir
        public ActionResult GeriDon(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                if (id > 0)
                {
                    // Veritabanından yazıcı bulunur
                    var blogdb = db.Yazici.FirstOrDefault(x => x.YaziciID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound();
                    }

                    blogdb.AktifMi = true;
                    blogdb.Durum = 2;
                    db.SaveChanges();

                    // Ambar bölümüne yönlendirilir
                    return RedirectToAction("yaziciListe", "Terkin");
                }
                else
                {
                    return RedirectToAction("yaziciListe", "Terkin");
                }
            }
        }
        // Monitör listesi görüntülenir
        public ActionResult monitorListe(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                // Monitör ve ilgili bilgilerin listesi oluşturulur
                var veri = new AnaClass
                {
                    monitorList = db.Monitor.AsNoTracking().Where(x => x.Durum == 0).Select(x => new MonitorClass
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
                    // Personel, bilgisayar türü ve bütçe listeleri de eklenir
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
        // Monitörü aktif hale getirir
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

                    blogdb.AktifMi = true;
                    blogdb.Durum = 2;
                    db.SaveChanges();


                    return RedirectToAction("monitorListe", "Terkin");
                }
                else
                {
                    return RedirectToAction("monitorListe", "Terkin");
                }
            }
        }
        // Bilgisayar listesini görüntülemek için gerekli olan ActionResult
        public ActionResult pcListe(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            // Veritabanı bağlantısını başlat
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                // AnaClass nesnesi oluştur ve bilgisayar listesini al
                var veri = new AnaClass
                {
                    pcList = db.Bilgisayar.AsNoTracking().Where(x => x.Durum == 0).Select(x => new BilgisayarClass
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
                    }).ToList(), // Bilgisayarların listesini oluştur
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad + " " + x.Soyad,
                    }).ToList(), // Aktif personellerin listesini al
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).ToList(), // Aktif bilgisayar türlerinin listesini al
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(), // Aktif bütçelerin listesini al
                    yaziciTurList = db.YaziciTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new YaziciTurClass
                    {
                        yaziciTurID = x.YaziciTurID,
                        yaziciTurAd = x.YaziciTurAdi,
                    }).ToList(),  // Aktif yazıcı türlerinin listesini al
                };
                return View(veri);  // Veriyi view'e gönder
            }
        }
        // Bilgisayarın durumunu geri döndürmek için gerekli olan ActionResult
        public ActionResult pcGeriDon(int id)
        {
            // Kullanıcının oturum açıp açmadığını kontrol et
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            // Veritabanı bağlantısını başlat
            using (var db = new OGMDatabase1Entities1())
            {
                if (id > 0)
                {
                    var blogdb = db.Bilgisayar.FirstOrDefault(x => x.BilgisayarID == id);
                    if (blogdb == null)
                    { 
                        return HttpNotFound();
                    }
                    // Bilgisayarın durumunu aktif hale getir ve güncellemeleri kaydet
                    blogdb.AktifMi = true;
                    blogdb.Durum = 2;
                    db.SaveChanges();

                    // Bilgisayar listesinin görüntülendiği sayfaya yönlendir
                    return RedirectToAction("pcListe", "Terkin");
                }
                else
                {
                    // Geçersiz id durumunda bilgisayar listesinin görüntülendiği sayfaya yönlendir
                    return RedirectToAction("pcListe", "Terkin");
                }
            }
        }
    }
}

    