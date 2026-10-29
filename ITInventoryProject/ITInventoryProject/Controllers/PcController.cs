using ITInventoryProject.Models;
using ITInventoryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITInventoryProject.Controllers
{
    public class PcController : Controller
    {
        // GET: Pc  Bilgisayar verilerini görüntülemek için kullanılan metod 
        public ActionResult Index(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                // Gelen ID'yi sayıya çevirme
                int.TryParse(id, out var gelenid);
                // Veri modeli oluşturma
                var veri = new AnaClass
                {
                    // Eğer gelen ID sıfır ise yeni bir BilgisayarClass oluşturulur, değilse ilgili veriler DB'den çekilir
                    pcClass = gelenid == 0 ? new BilgisayarClass() : db.Bilgisayar.AsNoTracking().Where(x => x.BilgisayarID == gelenid&&x.Durum==2).Select(x => new BilgisayarClass()
                    {
                        BilgisayarId = x.BilgisayarID,
                        BilgisayarTurId = x.BilgisayarTurID,
                        ButceId = x.ButceID,
                        DomainNo = x.DomainNo,
                        HDD = x.HDD,
                        IslemciModeli = x.IslemciModeli,
                        IslemciTeknolojisi = x.IslemciTeknolojisi,
                        IslemciHizi = x.IslemciHizi,
                        aciklama=x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        PersonelId = x.PersonelID,
                        Ram = x.RAM,
                        SeriNo = x.SeriNo,
                        SSD = x.SSD
                    }).First(),
                    // Personel listesi dolduruldu
                    personellist = db.Personel.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new PersonelClass
                    {
                        PersonelId = x.PersonelID,
                        Ad = x.Ad+" "+x.Soyad,
                    }).ToList(),
                    // Bilgisayar türleri listesi dolduruldu
                    pcTurlist = db.BilgisayarTuru.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new BilgisarayTurClass
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd=x.BilgisayarTurAd,
                    }).ToList(),
                    // Bütçe listesi dolduruldu
                    butcelist = db.Butce.AsNoTracking().Where(x => x.AktifMi == true).Select(x => new ButceClass
                    {
                        ButceId = x.ButceID,
                        ButceAdi = x.ButceAdi,
                    }).ToList(),

                };
                return View(veri);

            }

        }
        // Bilgisayar ekleme veya düzenleme işlemi (POST)
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]

        public ActionResult EkleDuzenle(AnaClass pc)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                try
                {
                    // Yeni bilgisayar ekleme işlemi
                    if (pc.pcClass.BilgisayarId == 0)
                    {
                        var veri = new Bilgisayar()
                        {
                            BilgisayarTurID = pc.pcClass.BilgisayarTurId,
                            Aciklama = pc.pcClass.aciklama,
                            
                            DomainNo = pc.pcClass.DomainNo,
                            IslemciHizi = pc.pcClass.IslemciHizi,
                            ButceID = pc.pcClass.ButceId,
                            PersonelID = pc.pcClass.PersonelId,                           
                            SeriNo = pc.pcClass.SeriNo,

                            HDD = pc.pcClass.HDD,
                            SSD = pc.pcClass.SSD,
                            RAM = pc.pcClass.Ram,

                            Marka = pc.pcClass.Marka,
                            Model = pc.pcClass.Model,
                            IslemciModeli = pc.pcClass.IslemciModeli,
                            IslemciTeknolojisi = pc.pcClass.IslemciTeknolojisi,                           
                            AktifMi = true,
                            Durum=2,
                        };

                        db.Bilgisayar.Add(veri);
                        db.SaveChanges();
                        // TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Basarili, "Ekleme işlemi başarılı.");
                        return RedirectToAction("Liste", "Pc");
                    }
                    else
                    {
                        // Mevcut bilgisayar düzenleme işlemi
                        var bilgisayar = db.Bilgisayar.FirstOrDefault(x => x.BilgisayarID == pc.pcClass.BilgisayarId);
                        if (bilgisayar != null)
                        {
                            bilgisayar.BilgisayarTurID = pc.pcClass.BilgisayarTurId;
                            bilgisayar.Aciklama = pc.pcClass.aciklama;
                            bilgisayar.DomainNo = pc.pcClass.DomainNo;
                            bilgisayar.IslemciHizi = pc.pcClass.IslemciHizi;
                            bilgisayar.ButceID = pc.pcClass.ButceId;
                            bilgisayar.PersonelID = pc.pcClass.PersonelId;
                            bilgisayar.HDD = pc.pcClass.HDD;
                            bilgisayar.SeriNo = pc.pcClass.SeriNo;
                            bilgisayar.SSD = pc.pcClass.SSD;
                            bilgisayar.Marka = pc.pcClass.Marka;
                            bilgisayar.Model = pc.pcClass.Model;
                            bilgisayar.IslemciModeli = pc.pcClass.IslemciModeli;
                            bilgisayar.IslemciTeknolojisi = pc.pcClass.IslemciTeknolojisi;
                            bilgisayar.RAM = pc.pcClass.Ram;

                            db.SaveChanges();
                            return RedirectToAction("Liste", "Pc");
                        }
                        else
                        {
                        
                            return RedirectToAction("Liste", "Pc");
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
        // Bilgisayar listesini görüntüleme işlemi
        public ActionResult Liste(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                int.TryParse(id, out var gelenid);
                var veri = new AnaClass
                {
                    // Bilgisayar listesi çekildi
                    pcList = db.Bilgisayar.AsNoTracking().Where(x=>x.Durum==2).Select(x=> new BilgisayarClass
                    {
                        BilgisayarId = x.BilgisayarID,
                        BilgisayarTurAdd = x.BilgisayarTuru.BilgisayarTurAd,
                        personelBirim=x.Personel.Gorev.GorevAdi,
                        ButceAd = x.Butce.ButceAdi,
                        DomainNo = x.DomainNo,
                        HDD = x.HDD,
                        IslemciModeli = x.IslemciModeli,
                        IslemciTeknolojisi = x.IslemciTeknolojisi,
                        IslemciHizi = x.IslemciHizi,
                        aciklama = x.Aciklama,
                        Marka = x.Marka,
                        Model = x.Model,
                        personelAd = x.Personel.Ad+" "+x.Personel.Soyad,
                        Ram = x.RAM,
                        SeriNo = x.SeriNo,
                        SSD = x.SSD
                    }).ToList(),
                    // Diğer listeler (personel, bilgisayar türleri, bütçe) dolduruldu
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
                };
                return View(veri);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        // Bilgisayar silme işlemi (Durum değişikliği yaparak)
        public ActionResult Sil(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                if (id > 0) 
                {
                    var blogdb = db.Bilgisayar.FirstOrDefault(x => x.BilgisayarID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound(); 
                    }

                    // Silme işlemi
                    blogdb.Durum = 1;
                    db.SaveChanges();

                   
                    return RedirectToAction("pcListe", "Ambar"); 
                }
                else
                {
                    return RedirectToAction("pcListe", "Ambar");
                }
            }
        }
        // Bilgisayarı tekrar aktif hale getirme işlemi
        public ActionResult TerkinAt(int id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            // Veritabanı bağlantısı
            using (var db = new OGMDatabase1Entities1())
            {
                // Geçerli bir ID var mı kontrol edildi
                if (id > 0)
                {
                    var blogdb = db.Bilgisayar.FirstOrDefault(x => x.BilgisayarID == id);
                    if (blogdb == null)
                    {
                        return HttpNotFound();
                    }

                    // Silme işlemi 
                    blogdb.Durum = 0;
                    db.SaveChanges();


                    return RedirectToAction("pcListe", "Terkin");
                }
                else
                {
                    return RedirectToAction("pcListe", "Terkin");
                }
            }
        }

    }
}