using System.Globalization;

namespace Locktech;

/// <summary>Geçerli isteğin dili. Adres /en ile başlıyorsa İngilizce, değilse Türkçe.</summary>
public static class Dil
{
    public static bool En => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en";

    /// <summary>Geçerli dile göre metni seçer.</summary>
    public static string S(string tr, string en) => En ? en : tr;
}

/// <summary>
/// İki dilli metin. Tek bir string verilirse iki dilde aynıdır (model adı, ölçü vb.);
/// ("Türkçe", "English") biçiminde verilirse dile göre seçilir.
/// </summary>
public readonly record struct Metin(string Tr, string En)
{
    public override string ToString() => Dil.En ? En : Tr;
    public static implicit operator Metin(string ortak) => new(ortak, ortak);
    public static implicit operator Metin((string Tr, string En) m) => new(m.Tr, m.En);
}

/// <summary>Sayfa adresleri; geçerli dile göre Türkçe ya da İngilizce yolu verir.</summary>
public static class Yollar
{
    public static string Ana => Dil.En ? "/en" : "/";
    public static string Urunler => Dil.En ? "/en/products" : "/urunler";
    public static string Urun(string kod) => $"{Urunler}/{kod}";
    public static string Cozumler => Dil.En ? "/en/solutions" : "/cozumler";
    public static string Cozum(string kod) => $"{Cozumler}/{kod}";
    public static string Hakkimizda => Dil.En ? "/en/about" : "/hakkimizda";
    public static string Iletisim => Dil.En ? "/en/contact" : "/iletisim";
    public static string Kvkk => Dil.En ? "/en/privacy" : "/kvkk";
    public static string Cerez => Dil.En ? "/en/cookies" : "/cerez-politikasi";

    /// <summary>İletişim sayfasındaki teklif formu; ürün kodu verilirse formda o ürün seçili gelir.</summary>
    public static string Teklif(string? urunKodu = null) =>
        Iletisim + (urunKodu is null ? "" : $"?urun={urunKodu}") + "#teklif";
}

public static class Site
{
    public const string Adres = "https://locktech.com.tr";
    public const string Telefon = "+90 542 432 10 10";
    public const string TelefonBaglantisi = "tel:+905424321010";
    /// <summary>Panelden değiştirilebilen iletişim e-postası.</summary>
    public static string Eposta => Ayarlar.AyarDeposu.GorunenEposta;
    public const string AcikAdres = "İvedik OSB 1362 Cadde No:34, Yenimahalle / ANKARA";

    // Locktech markasının sahibi olan şirket (KVKK metninde veri sorumlusu)
    public const string Unvan = "SELÇUKLU İMALAT PAZARLAMA MOBİLYA İNŞAAT İTHALAT İHRACAT SAN";
    public const string VergiDairesi = "İvedik";
    public const string VergiNo = "7601097707";
    public const string Mersis = "0760109770700001";
}
