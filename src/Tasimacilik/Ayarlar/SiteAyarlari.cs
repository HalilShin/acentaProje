namespace Tasimacilik.Ayarlar;

/// <summary>
/// Panelden değiştirilen ayarlar. Üç marka aynı firmaya ait olduğu için
/// telefon, WhatsApp, adres ve e-posta ayarları ortaktır. App_Data/ayarlar.json dosyasında tutulur.
/// </summary>
public record SiteAyarlari
{
    public const string OrnekTelefon = "0 (312) 000 00 00";
    public const string OrnekAdres = "Adres bilgisi eklenecek, Ankara";

    // İletişim (üç markada ortak)
    public string Telefon { get; set; } = OrnekTelefon;
    public string Adres { get; set; } = OrnekAdres;
    /// <summary>Sitede gösterilen e-posta adresi; boşsa gösterilmez.</summary>
    public string Eposta { get; set; } = "";

    // WhatsApp
    public bool WhatsAppGoster { get; set; } = true;
    public string WhatsAppNumara { get; set; } = "";

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
    /// <summary>Teklif formlarının gideceği adres.</summary>
    public string AliciAdres { get; set; } = "";

    public bool PostaHazir =>
        !string.IsNullOrWhiteSpace(SmtpSunucu) &&
        !string.IsNullOrWhiteSpace(GonderenAdres) &&
        !string.IsNullOrWhiteSpace(AliciAdres);

    /// <summary>Telefon ya da adres hâlâ örnek değerdeyse true; panelde uyarı gösterilir.</summary>
    public bool IletisimEksik => Telefon == OrnekTelefon || Adres == OrnekAdres;

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
