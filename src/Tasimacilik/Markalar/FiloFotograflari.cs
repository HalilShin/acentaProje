namespace Tasimacilik.Markalar;

/// <summary>
/// Sitede kullanılan bir fotoğraf. Kendi fotoğraflarımızda (Kendi = true) yazar ve lisans boştur, atıf gösterilmez;
/// diğerleri Wikimedia Commons'tan serbest lisanslıdır ve /fotograf-kaynaklari sayfasında atıfları vardır.
/// </summary>
public record Fotograf(string Dosya, string Konu, string Yazar, string Lisans, string LisansAdresi, string KaynakAdresi, bool Kendi = false)
{
    public static Fotograf Bizim(string dosya, string konu) => new(dosya, konu, "", "", "", "", Kendi: true);

    /// <summary>
    /// Sayfada kullanılacak adres. Kendi fotoğraflarımızın her marka için ayrı sürümü vardır
    /// (araçta o markanın logosu ve plakada site adresi): /img/filo/{marka}/dosya.jpg.
    /// </summary>
    public string Yol(string markaKodu) =>
        Kendi ? Dosya.Replace("/img/filo/", $"/img/filo/{markaKodu}/") : Dosya;
}

/// <summary>
/// Sitedeki tüm fotoğraflar. Kendi fotoğraflarımız firmanın gönderdiği fotoğraflardır; arka planları düzenlendi,
/// araçlara her markanın logosu ve plakalara site adresi işlendi (markaya göre ayrı dosya, bkz. Fotograf.Yol).
/// Diğer araç fotoğrafları araç modellerini tanıtır; onlarda plakalar ve başka firmalara ait yazılar bulanıktır.
/// </summary>
public static class FiloFotograflari
{
    private const string BySa4 = "https://creativecommons.org/licenses/by-sa/4.0/";
    private const string BySa3 = "https://creativecommons.org/licenses/by-sa/3.0/";
    private const string By4 = "https://creativecommons.org/licenses/by/4.0/";
    private const string By3 = "https://creativecommons.org/licenses/by/3.0/";
    private const string Cc0 = "https://creativecommons.org/publicdomain/zero/1.0/";
    private const string Commons = "https://commons.wikimedia.org/wiki/File:";

    /// <summary>Araç modeli → fotoğraf.</summary>
    private static readonly Dictionary<string, Fotograf> Araclar = new()
    {
        ["Mercedes-Benz Vito"] = new("/img/filo/vito.jpg", "Mercedes-Benz Vito", "Dinkun Chen", "CC BY-SA 4.0", BySa4, Commons + "MERCEDES-BENZ_VITO_(W447)_China_(4).jpg"),
        ["Mercedes-Benz Sprinter"] = Fotograf.Bizim("/img/filo/sprinter.jpg", "Mercedes-Benz Sprinter"),
        ["Otokar Sultan"] = Fotograf.Bizim("/img/filo/sultan.jpg", "Otokar Sultan"),
        ["Mercedes-Benz Travego"] = Fotograf.Bizim("/img/filo/travego.jpg", "Mercedes-Benz Travego"),
        ["Temsa Safir"] = Fotograf.Bizim("/img/filo/temsa-safir.jpg", "Temsa Safir"),
        ["Mercedes-Benz Tourismo"] = new("/img/filo/tourismo.jpg", "Mercedes-Benz Tourismo", "Matti Blume", "CC BY-SA 4.0", BySa4, Commons + "Mercedes-Benz_Tourismo,_Berlin_(20180910_131307).jpg"),
    };

    /// <summary>Yer kodu → fotoğraf.</summary>
    private static readonly Dictionary<string, Fotograf> Yerler = new()
    {
        ["pamukkale"] = new("/img/yerler/pamukkale.jpg", "Pamukkale", "rheins", "CC BY 3.0", By3, Commons + "Pamukkale_Travertines_-_2014.10_-_panoramio.jpg"),
        ["gobeklitepe"] = new("/img/yerler/gobeklitepe.jpg", "Göbeklitepe", "Teomancimit", "CC BY-SA 3.0", BySa3, Commons + "G%C3%B6bekli_Tepe,_Urfa.jpg"),
        ["uzungol"] = new("/img/yerler/uzungol.jpg", "Uzungöl", "İhsan Deniz Kılıçoğlu", "CC BY-SA 3.0", BySa3, Commons + "Uzung%C3%B6l_lake_and_town.jpg"),
        ["ishakpasa"] = new("/img/yerler/ishakpasa.jpg", "İshak Paşa Sarayı", "W. Bulach", "CC BY-SA 4.0", BySa4, Commons + "00_2404_Ishak_Pasha_Palace_-_Turkey.jpg"),
        ["kapadokya-otobus"] = Fotograf.Bizim("/img/filo/kapadokya-otobus.jpg", "Göreme'de tur otobüsü"),
        ["otobusler"] = Fotograf.Bizim("/img/filo/otobusler.jpg", "Kruvaziyer limanında tur otobüsleri"),
        ["servis"] = Fotograf.Bizim("/img/filo/sprinter-kendi.jpg", "Servis minibüsü"),
        ["safranbolu"] = new("/img/yerler/safranbolu.jpg", "Safranbolu", "Hamdigumus", "CC0", Cc0, Commons + "Safranbolu_Evleri_2014-8.jpg"),
        ["esenboga"] = new("/img/yerler/esenboga.jpg", "Esenboğa Havalimanı", "Dosseman", "CC BY-SA 4.0", BySa4, Commons + "Ankara_Esenbo%C4%9Fa_Airport_in_2011_03.jpg"),
    };

    /// <summary>Hizmet kodu → fotoğraf anahtarı (araç modeli ya da yer kodu).</summary>
    private static readonly Dictionary<string, string> Hizmetler = new()
    {
        // Selçuklu Turizm
        ["turizm-ve-gezi-tasimaciligi"] = "safranbolu",
        ["gunubirlik-turlar"] = "kapadokya-otobus",
        ["ege-turu"] = "pamukkale",
        ["gap-turu"] = "gobeklitepe",
        ["karadeniz-turu"] = "uzungol",
        ["dogu-turu"] = "ishakpasa",
        ["sehir-ici-ve-disi-toplu-tasima"] = "Otokar Sultan",
        // Ankara Otobüs Kirala
        ["kiralik-otobus"] = "otobusler",
        ["kiralik-minibus"] = "Mercedes-Benz Sprinter",
        ["soforlu-otobus-kiralama"] = "Temsa Safir",
        ["tur-otobusu-kiralama"] = "Mercedes-Benz Travego",
        ["vip-minibus-kiralama"] = "Mercedes-Benz Vito",
        ["d2-belgeli-otobus-kiralama"] = "Mercedes-Benz Tourismo",
        ["film-dizi-ozel-kiralama"] = "Mercedes-Benz Sprinter",
        ["personel-ve-ogrenci-servisi"] = "servis",
        ["okul-gezileri"] = "Otokar Sultan",
        // Ankara Turizm Transfer
        ["havalimani-transferi"] = "esenboga",
        ["otel-transferi"] = "Mercedes-Benz Vito",
        ["sehirlerarasi-yolcu-tasimaciligi"] = "Mercedes-Benz Travego",
        ["kurumsal-etkinlik-tasimaciligi"] = "Mercedes-Benz Sprinter",
        ["kongre-tasimaciligi"] = "Temsa Safir",
    };

    /// <summary>Marka kodu → ilk ekran fotoğrafı (1920×1080). Otobüs Kirala ilk ekranında fotoğraf yerine kapasite seçici var.</summary>
    private static readonly Dictionary<string, Fotograf> Vitrinler = new()
    {
        ["selcuklu"] = new("/img/yerler/kapadokya-genis.jpg", "Kapadokya", "Wolfgang Moroder", "CC BY-SA 3.0", BySa3, Commons + "Hot_air_balloon_ride_at_sunrise_in_Cappadocia.JPG"),
        ["transfer"] = new("/img/filo/vclass-genis.jpg", "Mercedes-Benz V-Class", "Mam16600 / Tripyana", "CC BY 4.0", By4, Commons + "Black_Mercedes-Benz_V-Class_outside_a_Mayfair_hotel,_Grosvenor_Square,_London_(Tripyana).jpg"),
    };

    public static Fotograf? Bul(string modelYaDaYer) =>
        Araclar.GetValueOrDefault(modelYaDaYer) ?? Yerler.GetValueOrDefault(modelYaDaYer);

    public static Fotograf? HizmetFotografi(string hizmetKodu) =>
        Hizmetler.TryGetValue(hizmetKodu, out var anahtar) ? Bul(anahtar) : null;

    public static (string Dosya, Fotograf Atif)? Vitrin(string markaKodu) =>
        Vitrinler.TryGetValue(markaKodu, out var v) ? (v.Dosya, v) : null;

    /// <summary>Atıf sayfası için lisanslı fotoğraflar (ilk ekranlar, araçlar, yerler); kendi fotoğraflarımız hariç.</summary>
    public static IEnumerable<Fotograf> Tumu => Vitrinler.Values.Concat(Araclar.Values).Concat(Yerler.Values).Where(f => !f.Kendi);
}
