namespace Tasimacilik.Markalar;

/// <summary>Sitede kullanılan bir fotoğraf ve lisans atfı (Wikimedia Commons, serbest lisans).</summary>
public record Fotograf(string Dosya, string Konu, string Yazar, string Lisans, string LisansAdresi, string KaynakAdresi);

/// <summary>
/// Sitedeki tüm fotoğraflar. Araç fotoğrafları araç modellerini tanıtır, Halil'in araçları değildir;
/// plakalar, yüzler ve başka firmalara ait yazılar bulanıklaştırıldı, bazıları kırpıldı.
/// Yer fotoğrafları tur ve transfer hizmetlerini görselleştirir. Atıflar /fotograf-kaynaklari sayfasında.
/// Kendi fotoğraflar gelince bu liste değiştirilir.
/// </summary>
public static class FiloFotograflari
{
    private const string BySa4 = "https://creativecommons.org/licenses/by-sa/4.0/";
    private const string BySa3 = "https://creativecommons.org/licenses/by-sa/3.0/";
    private const string By3 = "https://creativecommons.org/licenses/by/3.0/";
    private const string Cc0 = "https://creativecommons.org/publicdomain/zero/1.0/";
    private const string Commons = "https://commons.wikimedia.org/wiki/File:";

    /// <summary>Araç modeli → fotoğraf.</summary>
    private static readonly Dictionary<string, Fotograf> Araclar = new()
    {
        ["Mercedes-Benz Vito"] = new("/img/filo/vito.jpg", "Mercedes-Benz Vito", "Dinkun Chen", "CC BY-SA 4.0", BySa4, Commons + "MERCEDES-BENZ_VITO_(W447)_China_(4).jpg"),
        ["Mercedes-Benz Sprinter"] = new("/img/filo/sprinter.jpg", "Mercedes-Benz Sprinter", "Ethan Llamas", "CC BY-SA 4.0", BySa4, Commons + "2016_Mercedes-Benz_Sprinter_Super_High_Minibus,_front_right.jpg"),
        ["Otokar Sultan"] = new("/img/filo/sultan.jpg", "Otokar Sultan", "Mr.choppers", "CC BY-SA 3.0", BySa3, Commons + "Otokar_Sultan_125_S_front.jpg"),
        ["Mercedes-Benz Travego"] = new("/img/filo/travego.jpg", "Mercedes-Benz Travego", "Anatoliy-024", "CC BY-SA 3.0", BySa3, Commons + "Mercedes-Benz_O580-16RHD_Travego_from_Poland,_Lviv_Liberty_Avenue.jpg"),
        ["MAN Lion's Coach"] = new("/img/filo/lions.jpg", "MAN Lion's Coach", "Anatoliy-024", "CC BY-SA 4.0", BySa4, Commons + "MAN_Lion%27s_Coach_Papenburger_Kanalreisen.jpg"),
        ["Mercedes-Benz Tourismo"] = new("/img/filo/tourismo.jpg", "Mercedes-Benz Tourismo", "Matti Blume", "CC BY-SA 4.0", BySa4, Commons + "Mercedes-Benz_Tourismo,_Berlin_(20180910_131307).jpg"),
    };

    /// <summary>Yer kodu → fotoğraf.</summary>
    private static readonly Dictionary<string, Fotograf> Yerler = new()
    {
        ["pamukkale"] = new("/img/yerler/pamukkale.jpg", "Pamukkale", "rheins", "CC BY 3.0", By3, Commons + "Pamukkale_Travertines_-_2014.10_-_panoramio.jpg"),
        ["gobeklitepe"] = new("/img/yerler/gobeklitepe.jpg", "Göbeklitepe", "Teomancimit", "CC BY-SA 3.0", BySa3, Commons + "G%C3%B6bekli_Tepe,_Urfa.jpg"),
        ["uzungol"] = new("/img/yerler/uzungol.jpg", "Uzungöl", "İhsan Deniz Kılıçoğlu", "CC BY-SA 3.0", BySa3, Commons + "Uzung%C3%B6l_lake_and_town.jpg"),
        ["ishakpasa"] = new("/img/yerler/ishakpasa.jpg", "İshak Paşa Sarayı", "W. Bulach", "CC BY-SA 4.0", BySa4, Commons + "00_2404_Ishak_Pasha_Palace_-_Turkey.jpg"),
        ["kapadokya"] = new("/img/yerler/kapadokya.jpg", "Kapadokya", "Bernard Gagnon", "CC BY-SA 3.0", BySa3, Commons + "Hot_air_balloon_in_Cappadocia_02.jpg"),
        ["safranbolu"] = new("/img/yerler/safranbolu.jpg", "Safranbolu", "Hamdigumus", "CC0", Cc0, Commons + "Safranbolu_Evleri_2014-8.jpg"),
        ["esenboga"] = new("/img/yerler/esenboga.jpg", "Esenboğa Havalimanı", "Dosseman", "CC BY-SA 4.0", BySa4, Commons + "Ankara_Esenbo%C4%9Fa_Airport_in_2011_03.jpg"),
    };

    /// <summary>Hizmet kodu → fotoğraf anahtarı (araç modeli ya da yer kodu).</summary>
    private static readonly Dictionary<string, string> Hizmetler = new()
    {
        // Selçuklu Turizm
        ["turizm-ve-gezi-tasimaciligi"] = "safranbolu",
        ["gunubirlik-turlar"] = "kapadokya",
        ["ege-turu"] = "pamukkale",
        ["gap-turu"] = "gobeklitepe",
        ["karadeniz-turu"] = "uzungol",
        ["dogu-turu"] = "ishakpasa",
        ["sehir-ici-ve-disi-toplu-tasima"] = "Otokar Sultan",
        // Ankara Otobüs Kirala
        ["kiralik-otobus"] = "Mercedes-Benz Tourismo",
        ["kiralik-minibus"] = "Mercedes-Benz Sprinter",
        ["soforlu-otobus-kiralama"] = "MAN Lion's Coach",
        ["tur-otobusu-kiralama"] = "Mercedes-Benz Travego",
        ["vip-minibus-kiralama"] = "Mercedes-Benz Vito",
        ["d2-belgeli-otobus-kiralama"] = "Mercedes-Benz Tourismo",
        ["film-dizi-ozel-kiralama"] = "Mercedes-Benz Sprinter",
        ["okul-gezileri"] = "Otokar Sultan",
        // Ankara Turizm Transfer
        ["havalimani-transferi"] = "esenboga",
        ["otel-transferi"] = "Mercedes-Benz Vito",
        ["sehirlerarasi-yolcu-tasimaciligi"] = "Mercedes-Benz Tourismo",
        ["kurumsal-etkinlik-tasimaciligi"] = "Mercedes-Benz Sprinter",
        ["kongre-tasimaciligi"] = "MAN Lion's Coach",
    };

    /// <summary>Marka kodu → ilk ekran fotoğrafı (geniş hali) ve atfı.</summary>
    private static readonly Dictionary<string, (string Dosya, string Model)> Vitrinler = new()
    {
        ["selcuklu"] = ("/img/filo/travego-genis.jpg", "Mercedes-Benz Travego"),
        ["otobuskirala"] = ("/img/filo/tourismo-genis.jpg", "Mercedes-Benz Tourismo"),
        ["transfer"] = ("/img/filo/vito-genis.jpg", "Mercedes-Benz Vito"),
    };

    public static Fotograf? Bul(string modelYaDaYer) =>
        Araclar.GetValueOrDefault(modelYaDaYer) ?? Yerler.GetValueOrDefault(modelYaDaYer);

    public static Fotograf? HizmetFotografi(string hizmetKodu) =>
        Hizmetler.TryGetValue(hizmetKodu, out var anahtar) ? Bul(anahtar) : null;

    public static (string Dosya, Fotograf Atif)? Vitrin(string markaKodu) =>
        Vitrinler.TryGetValue(markaKodu, out var v) && Bul(v.Model) is { } atif ? (v.Dosya, atif) : null;

    /// <summary>Atıf sayfası için tüm fotoğraflar (önce araçlar, sonra yerler).</summary>
    public static IEnumerable<Fotograf> Tumu => Araclar.Values.Concat(Yerler.Values);
}
