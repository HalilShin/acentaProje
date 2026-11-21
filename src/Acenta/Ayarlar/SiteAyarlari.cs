namespace Acenta.Ayarlar;

/// <summary>Panelden değiştirilen site ayarları. App_Data/ayarlar.json dosyasında tutulur.</summary>
public record SiteAyarlari
{
    /// <summary>Acentanın unvanı; KVKK metninde veri sorumlusu.</summary>
    public const string FirmaUnvani = "Anadolu Selçuklu Organizasyon Turizm";
    public const string SiteAdi = "Ankara Kültür Gezi";
    public const string AnaAlanAdi = "ankarakulturgezi.com";
    public const string SiteAdresi = "https://" + AnaAlanAdi;
    /// <summary>Ana alan adına kalıcı yönlenen alan adları.</summary>
    public static readonly string[] EkAlanAdlari = ["ankarakulturgezi.com.tr", "kulturgezi.com.tr"];
    public const string TursabBelgeNo = "15385";
    public const string WhatsAppMesaj = "Merhaba, turlarınız hakkında bilgi almak istiyorum.";

    // Eski örnek değerler; kayıtlı ayarlarda kaldıysa okunurken temizlenir.
    public const string EskiOrnekTelefon = "0 (312) 000 00 00";
    public const string EskiOrnekAdres = "Adres bilgisi eklenecek, Ankara";

    // İletişim
    public string Telefon { get; set; } = "+90 542 716 69 11";
    /// <summary>Açık adres; boşsa sitede adres gösterilmez.</summary>
    public string Adres { get; set; } = "İvedik OSB Mah. 1362 Cad. No:34, Yenimahalle / Ankara";
    /// <summary>Sitede gösterilen e-posta adresi; boşsa gösterilmez.</summary>
    public string Eposta { get; set; } = "selcuklutravel@gmail.com";

    // WhatsApp
    public bool WhatsAppGoster { get; set; } = true;
    public string WhatsAppNumara { get; set; } = "905427166911";

    // E-posta gönderimi (SMTP)
    public string SmtpSunucu { get; set; } = "";
    public int SmtpPort { get; set; } = 587;
    /// <summary>Auto, SslOnConnect, StartTls veya None.</summary>
    public string SmtpGuvenlik { get; set; } = "Auto";
    public string SmtpKullanici { get; set; } = "";
    /// <summary>Bellekte düz metin; dosyaya şifrelenerek yazılır.</summary>
    public string SmtpSifre { get; set; } = "";
    public string GonderenAdres { get; set; } = "";
    public string GonderenAd { get; set; } = "Web Sitesi";
    /// <summary>İletişim formlarının gideceği adres.</summary>
    public string AliciAdres { get; set; } = "selcuklutravel@gmail.com";

    public bool PostaHazir =>
        !string.IsNullOrWhiteSpace(SmtpSunucu) &&
        !string.IsNullOrWhiteSpace(GonderenAdres) &&
        !string.IsNullOrWhiteSpace(AliciAdres);

    /// <summary>Adres girilmemişse true; panelde hatırlatma gösterilir.</summary>
    public bool AdresEksik => string.IsNullOrWhiteSpace(Adres);

    /// <summary>tel: bağlantısı için yalnızca rakamlar (+90...).</summary>
    public string TelefonBaglantisi => "tel:+" + (NumaraDuzenle(Telefon) ?? "");

    /// <summary>wa.me bağlantısı; numara geçersizse null.</summary>
    public string? WhatsAppBaglantisi(string mesaj)
    {
        var numara = NumaraDuzenle(WhatsAppNumara);
        if (numara is null) return null;
        return $"https://wa.me/{numara}?text={Uri.EscapeDataString(mesaj)}";
    }

    /// <summary>"0542 432 10 10", "+90 542..." gibi yazımları 905424321010 biçimine çevirir.</summary>
    public static string? NumaraDuzenle(string? girdi)
    {
        var rakamlar = new string((girdi ?? "").Where(char.IsDigit).ToArray());
        if (rakamlar.Length == 11 && rakamlar.StartsWith('0')) rakamlar = "9" + rakamlar;
        else if (rakamlar.Length == 10) rakamlar = "90" + rakamlar;
        return rakamlar.Length is >= 10 and <= 15 ? rakamlar : null;
    }
}
