using ITInventoryProject.Models;
using ITInventoryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITInventoryProject.Controllers
{
    public class MonitorController : Controller
    {
        // GET: Monitor:  Monitör listesini görüntüleyen metod
        public ActionResult Index(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                // ID değerini al ve sayıya dönüştür
                int.TryParse(id, out var gelenid);
                // Monitör verilerini ve diğer liste verilerini AnaClass modeline doldur
                var veri = new AnaClass
                {
                    MonitorClass = gelenid == 0 ? new MonitorClass() : db.Monitor.AsNoTracking().Where(x => x.MonitorID == gelenid && x.Durum == 2).Select(x => new MonitorClass()
                    {
                        MonitorId = x.MonitorID,
                        BilgisayarId = x.BilgisayarID,
                        ButceId = x.ButceID,
                        PersonelId = x.PersoneID,
                        Marka = x.Marka,
                        Model = x.Model,
                        SeriNo = x.SeriNo,
                        aciklama = x.Aciklama,
                        BagliSeriNo = x.BagliSeriNo,
                        Boyut=x.Boyut,   
                    

                    }).First(),
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
                    pcList = db.Bilgisayar.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisayarClass
                    {
                        BilgisayarId = x.BilgisayarID,
                        SeriNo = x.SeriNo,
                    }).ToList(),
                };
                return View(veri);

            }

        }
        // Monitör ekleme ve düzenleme işlemleri
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]

        public ActionResult EkleDuzenle(AnaClass monitor)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");


            using (var db = new OGMDatabase1Entities1())
            {
                try
                {
                    // Yeni monitör ekleme işlemi
                    if (monitor.MonitorClass.MonitorId == 0)
                    {
                        var monitore = db.Monitor.FirstOrDefault(x => x.MonitorID == monitor.MonitorClass.MonitorId);
                        var blogdb_1 = db.Bilgisayar.FirstOrDefault(x => x.BilgisayarID == monitor.MonitorClass.BilgisayarId);

                        var veri = new Monitor()
                        {   
                            
                            BilgisayarID = monitor.MonitorClass.BilgisayarId,
                            Aciklama = monitor.MonitorClass.aciklama,
                            ButceID = monitor.MonitorClass.ButceId,
                            PersoneID = monitor.MonitorClass.PersonelId,
                            Marka = monitor.MonitorClass.Marka,
                            Model = monitor.MonitorClass.Model,
                            Boyut = monitor.MonitorClass.Boyut,
                            SeriNo = monitor.MonitorClass.SeriNo,
                            BagliSeriNo = blogdb_1.SeriNo,
                            AktifMi = true,
                            Durum = 2,
                        };

                        db.Monitor.Add(veri);
                        db.SaveChanges();
                        // TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Basarili, "Ekleme işlemi başarılı.");
                        return RedirectToAction("Liste", "Monitor");
                    }
                
                    // Var olan monitörü düzenleme işlemi
                    else
                    {

                        var monitore = db.Monitor.FirstOrDefault(x => x.MonitorID == monitor.MonitorClass.MonitorId);

                        if (monitore != null)
                        {
                            monitore.BilgisayarID = monitor.MonitorClass.BilgisayarId;
                            monitore.Aciklama = monitor.MonitorClass.aciklama;
                            monitore.Boyut = monitor.MonitorClass.Boyut;
                            monitore.ButceID = monitor.MonitorClass.ButceId;
                            monitore.PersoneID = monitor.MonitorClass.PersonelId;
                            monitore.BagliSeriNo = monitor.MonitorClass.BagliSeriNo;
                            monitore.SeriNo = monitor.MonitorClass.SeriNo;
                            monitore.Marka = monitor.MonitorClass.Marka;
                            monitore.Model = monitor.MonitorClass.Model;

                            db.SaveChanges();

                            return RedirectToAction("Liste", "Monitor");
                        }
                        else
                        {

                            return RedirectToAction("Liste", "Monitor");
                        }
                    }
                }
                catch (Exception exc)
                {
                    // Hata yönetimi: Hata meydana geldi
                    // TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Hata, "Bir hata oluştu: " + exc.Message);
                    return View();
                }
            }
        } 
        // Monitör listesi
        public ActionResult Liste(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                var veri = new AnaClass
                {
                    monitorList = db.Monitor.AsNoTracking().Where(x => x.Durum == 2).Select(x => new MonitorClass
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

        // Monitör durumunu pasif yapma
        public ActionResult Ambarat(int id)
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

                    blogdb.Durum = 1;
                    db.SaveChanges();


                    return RedirectToAction("monitorListe", "Ambar");
                }
                else
                {
                    return RedirectToAction("monitorListe", "Ambar");
                }
            }
        }

        // Monitör durumunu aktif yapma
        public ActionResult terkinAt(int id)
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


                    blogdb.Durum = 0;
                    db.SaveChanges();


                    return RedirectToAction("monitorListe", "Terkin"); // Monitör listesine yönlendir
                }
                else
                {
                    return RedirectToAction("monitorListe", "Terkin");
                }
            }
        }
    }
}