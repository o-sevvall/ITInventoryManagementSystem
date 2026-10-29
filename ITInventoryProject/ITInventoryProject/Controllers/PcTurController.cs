using ITInventoryProject.Models;
using ITInventoryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITInventoryProject.Controllers
{
    public class PcTurController : Controller
    {
        // GET: PcTur
        public ActionResult Index(string id)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                // ID değeri gelen parametreye göre integer'a çevriliyor.
                int.TryParse(id, out var gelenid);
                // AnaClass modeli oluşturuluyor. Eğer ID değeri sıfır ise boş bir BilgisarayTurClass oluşturuluyor.
                // Eğer ID sıfırdan büyükse veritabanından ilgili BilgisayarTuru bilgisi çekiliyor.
                var veri = new AnaClass
                {
                    // POST: PcTur Ekleme/Düzenleme işlemi
                    pcTurClass = gelenid == 0 ? new BilgisarayTurClass() : db.BilgisayarTuru.AsNoTracking().Where(x => x.BilgisayarTurID == gelenid).Select(x => new BilgisarayTurClass()
                    {
                        BilgisayarTurID = x.BilgisayarTurID,
                        BilgisayarTurAd = x.BilgisayarTurAd,
                    }).First(),
           

                };
                return View(veri);

            }

        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult EkleDuzenle(AnaClass pcTur)
        {
            if (Session["KullaniciID"] == null)
                return RedirectToAction("Login", "Home");
            using (var db = new OGMDatabase1Entities1())
            {
                try
                {
                    // Eğer BilgisayarTurID 0 ise yeni bir bilgisayar türü ekleniyor
                    if (pcTur.pcTurClass.BilgisayarTurID == 0)
                    {
                        var veri = new BilgisayarTuru()
                        {
                            BilgisayarTurAd = pcTur.pcTurClass.BilgisayarTurAd,
                           AktifMi=true,
                        };
                        // Veritabanına ekleme işlemi yapılıyor ve değişiklikler kaydediliyor.
                        db.BilgisayarTuru.Add(veri);
                        db.SaveChanges();
                        //TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Basarili, "Ekleme işlemi başarılı.");
                        return RedirectToAction("Index", "PcTur");
                    }
                    else
                    {
                        //var blog = db.BlogDT.First(x => x.ID == blogViewModel.BlogDclass.Id);
                        //blog.Baslik = blogViewModel.BlogDclass.Baslik;
                        //blog.KategoriID = blogViewModel.BlogDclass.BlogKategoriId;
                        //blog.Tarih = blogViewModel.BlogDclass.Tarih;
                        //blog.KisaAciklama = blogViewModel.BlogDclass.KisaAciklama;
                        //blog.SeoLink = IndexController.LinkDonustur(blogViewModel.BlogDclass.Baslik);
                        //blog.Aciklama = blogViewModel.BlogDclass.Aciklama;
                        //blog.Yazar = blogViewModel.BlogDclass.Yazar;
                        //blog.GuncelleyenKisiID = Guid.Parse(User.Identity.Name);
                        //blog.GuncellenmeTarihi = DateTime.Now;
                        //var fileanasayfa = Request.Files[0];
                        //if (fileanasayfa != null && (fileanasayfa.ContentType == "image/jpeg" || fileanasayfa.ContentType == "image/jpg" || fileanasayfa.ContentType == "image/png"))
                        //{
                        //    var dosyaYolu = ConfigurationManager.AppSettings["ResimBlog"];
                        //    var uploadfullpath = Server.MapPath(dosyaYolu);
                        //    if (Directory.Exists(uploadfullpath) == false) Directory.CreateDirectory(uploadfullpath);
                        //    var dosyaAdi = Path.GetRandomFileName().Replace(".", "");
                        //    var uzanti = Path.GetExtension(fileanasayfa.FileName);
                        //    var resimyolu = dosyaYolu + "/" + dosyaAdi + uzanti;
                        //    fileanasayfa.SaveAs(Server.MapPath(resimyolu));
                        //    blog.Resim = resimyolu;
                        //}
                        //db.Entry(blog).CurrentValues.SetValues(blog);
                        //db.SaveChanges();
                        //TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Basarili, "Güncelleme işlemi başarılı.");


                        // Güncelleme sonrası tanımlama sayfasına yönlendirme yapıldı
                        return RedirectToAction("Tanimla", "PcTur");
                    }

                }
                catch (Exception exc)
                {
                    // Hata durumunda aynı sayfaya geri dönülüyor.
                    //TempData["JavaScriptAlertYonetim"] = IndexController.AlertBildirim(IndexController.AlertTip.Hata, "Lütfen tekrar deneyiniz...");
                    return View("Tanimla");
                }
            }
        }

    }
}