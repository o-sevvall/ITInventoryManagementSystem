using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Web;

namespace ITInventoryProject.ViewModel
{
    public class ViewModel
    {

    }
    // AnaClass, envanter yönetiminde kullanılacak ana verileri tutan sınıftır.
    public class AnaClass
    {
        // Tek bir bilgisayar bilgilerini tutar.
        public BilgisayarClass pcClass { get; set; }

        // Bilgisayar listesini tutar.
        public List<BilgisayarClass> pcList { get; set; }

        //public BilgisayarClass pcClasslist { get; set; }

        // Personel listesini tutar.
        public List<PersonelClass> personellist { get; set; }

        // Bilgisayar türleri listesini tutar.
        public List<BilgisarayTurClass> pcTurlist { get; set; }
        // Bütçeler listesini tutar.
        public List<ButceClass> butcelist { get; set; }

        // Tek bir bilgisayar türünü tutar.
        public BilgisarayTurClass pcTurClass { get; set; }

        // Tek bir yazıcı bilgisini tutar.
        public yaziciClass yaziciClass { get; set; }

        // Yazıcılar listesini tutar.
        public List<yaziciClass> yaziciList { get; set; }

        // Yazıcı türleri listesini tutar.
        public List<YaziciTurClass> yaziciTurList { get; set; }

        // Tek bir monitör bilgisini tutar.
        public MonitorClass MonitorClass { get; set; }

        // Monitörler listesini tutar.
        public List<MonitorClass> monitorList { get; set; }



    }

    // Yazıcı türü bilgilerini tutar.
    public class YaziciTurClass
    {
        public int yaziciTurID { get; set; }

        public string yaziciTurAd { get; set; }
        public bool AktifMi { get; set; }

    }

    // Bilgisayar bilgilerini tutar.
    public class BilgisayarClass
    {
        public bool AktifMi { get; set; }

        public int BilgisayarId { get; set; }
        public int? PersonelId { get; set; }
        public string personelAd { get; set; }
        public string personelBirim { get; set; }

        public int? BilgisayarTurId { get; set; }
        public string BilgisayarTurAdd { get; set; }

        public int? ButceId { get; set; }
        public string ButceAd { get; set; }

        public string DomainNo { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string SeriNo { get; set; }
        public string IslemciTeknolojisi { get; set; }
        public string IslemciModeli { get; set; }
        public string IslemciHizi { get; set; }
        public string aciklama { get; set; }

        public string Ram { get; set; }
        public string SSD { get; set; }
        public string HDD { get; set; }

    }

    // Monitör bilgilerini tutar.
    public class MonitorClass
    {
        public bool AktifMi { get; set; }

        public int MonitorId { get; set; }
        public int? PersonelId { get; set; }
        public string PersonelAd { get; set; }

        public int? BilgisayarId { get; set; }
        public string BilgisayarAd { get; set; }

        public int? ButceId { get; set; }
        public string ButceAd { get; set; }

        public string Marka { get; set; }
        public string Model { get; set; }
        public string SeriNo { get; set; }
        public string Boyut { get; set; }

        public string BagliSeriNo { get; set; }
   
        public string aciklama { get; set; }
        //public bool AktifMi { get; set; }

    }

    // Personel bilgilerini tutar.
    public class PersonelClass
    {
        public int PersonelId { get; set; }
        public int? GorevId { get; set; }
        public int? BolgeId { get; set; }
        public int? MudurlukId { get; set; }
        public int? SeflikId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public bool AktifMi { get; set; }

    }

    // Bilgisayar türü bilgilerini tutar.
    public class BilgisarayTurClass
    {
        public int BilgisayarTurID { get; set; }

        public string BilgisayarTurAd { get; set; }
        public bool AktifMi { get; set; }

    }

    // Bütçe bilgilerini tutar.
    public class ButceClass
    {
        public int ButceId { get; set; }

        public string ButceAdi { get; set; }
        public bool AktifMi { get; set; }

    }

    // Yazıcı bilgilerini tutar.
    public class yaziciClass
    {
        public bool AktifMi { get; set; }

        public int yaziciId { get; set; }
        public int? PersonelId { get; set; }
        public string personelAd { get; set; }
        public string Tip { get; set; }
        public string yaziciturAd { get; set; }

        public int? yaziciTurId { get; set; }

        public int? ButceId { get; set; }
        public string ButceAd { get; set; }

    
        public string Marka { get; set; }
        public string Ip { get; set; }

        public string Model { get; set; }
        public string SeriNo { get; set; }
        public string aciklama { get; set; }
        public string durum { get; set; }

   
    }

}