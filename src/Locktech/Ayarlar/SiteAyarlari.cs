namespace Locktech.Ayarlar;

/// <summary>Panelden değiştirilen site ayarları. App_Data/ayarlar.json dosyasında tutulur.</summary>
public record SiteAyarlari
{
    // E-posta (SMTP)
    public string SmtpSunucu { get; set; } = "";
    public int SmtpPort { get; set; } = 587;
    /// <summary>Auto, SslOnConnect, StartTls veya None.</summary>
    public string SmtpGuvenlik { get; set; } = "Auto";
    public string SmtpKullanici { get; set; } = "";
    /// <summary>Bellekte düz metin; dosyaya şifrelenerek yazılır.</summary>
    public string SmtpSifre { get; set; } = "";
    public string GonderenAdres { get; set; } = "";
    public string GonderenAd { get; set; } = "Locktech Web Sitesi";
    /// <summary>Teklif formlarının gideceği adres.</summary>
    public string AliciAdres { get; set; } = "info@locktech.com.tr";

    // WhatsApp
    public bool WhatsAppGoster { get; set; } = true;
    public string WhatsAppNumara { get; set; } = "905424321010";
    public string WhatsAppMesaj { get; set; } = "Merhaba, Locktech ürünleri hakkında bilgi almak istiyorum.";

    public bool PostaHazir =>
        !string.IsNullOrWhiteSpace(SmtpSunucu) &&
        !string.IsNullOrWhiteSpace(GonderenAdres) &&
        !string.IsNullOrWhiteSpace(AliciAdres);

    /// <summary>wa.me bağlantısı; numara geçersizse null.</summary>
    public string? WhatsAppBaglantisi
    {
        get
        {
            var numara = NumaraDuzenle(WhatsAppNumara);
            if (numara is null) return null;
            var mesaj = string.IsNullOrWhiteSpace(WhatsAppMesaj) ? "" : "?text=" + Uri.EscapeDataString(WhatsAppMesaj.Trim());
            return $"https://wa.me/{numara}{mesaj}";
        }
    }

    /// <summary>"0542 432 10 10", "+90 542..." gibi yazımları 905424321010 biçimine çevirir.</summary>
    public static string? NumaraDuzenle(string? girdi)
    {
        var rakamlar = new string((girdi ?? "").Where(char.IsDigit).ToArray());
        if (rakamlar.Length == 11 && rakamlar.StartsWith('0')) rakamlar = "9" + rakamlar;
        else if (rakamlar.Length == 10 && rakamlar.StartsWith('5')) rakamlar = "90" + rakamlar;
        return rakamlar.Length is >= 10 and <= 15 ? rakamlar : null;
    }
}
