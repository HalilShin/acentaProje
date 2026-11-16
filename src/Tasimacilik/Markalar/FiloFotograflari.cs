namespace Tasimacilik.Markalar;

/// <summary>Bir araç fotoğrafı ve lisans atfı (Wikimedia Commons, CC BY-SA).</summary>
public record FiloFotografi(string Dosya, string Yazar, string Lisans, string LisansAdresi, string KaynakAdresi);

/// <summary>
/// Filodaki araç modellerinin temsili fotoğrafları. Fotoğraflar Halil'in araçları değildir;
/// Wikimedia Commons'tan serbest lisansla alındı, plakalar ve başka firmalara ait yazılar
/// bulanıklaştırıldı, bazıları kırpıldı. CC BY-SA gereği atıf araç filosu sayfasında gösterilir.
/// Kendi araç fotoğrafları gelince bu liste değiştirilir.
/// </summary>
public static class FiloFotograflari
{
    private const string BySa4 = "https://creativecommons.org/licenses/by-sa/4.0/";
    private const string BySa3 = "https://creativecommons.org/licenses/by-sa/3.0/";

    private static readonly Dictionary<string, FiloFotografi> Modeller = new()
    {
        ["Mercedes-Benz Vito"] = new("/img/filo/vito.jpg", "Dinkun Chen", "CC BY-SA 4.0", BySa4,
            "https://commons.wikimedia.org/wiki/File:MERCEDES-BENZ_VITO_(W447)_China_(4).jpg"),
        ["Mercedes-Benz Sprinter"] = new("/img/filo/sprinter.jpg", "Ethan Llamas", "CC BY-SA 4.0", BySa4,
            "https://commons.wikimedia.org/wiki/File:2016_Mercedes-Benz_Sprinter_Super_High_Minibus,_front_right.jpg"),
        ["Otokar Sultan"] = new("/img/filo/sultan.jpg", "Mr.choppers", "CC BY-SA 3.0", BySa3,
            "https://commons.wikimedia.org/wiki/File:Otokar_Sultan_125_S_front.jpg"),
        ["Mercedes-Benz Travego"] = new("/img/filo/travego.jpg", "Anatoliy-024", "CC BY-SA 3.0", BySa3,
            "https://commons.wikimedia.org/wiki/File:Mercedes-Benz_O580-16RHD_Travego_from_Poland,_Lviv_Liberty_Avenue.jpg"),
        ["MAN Lion's Coach"] = new("/img/filo/lions.jpg", "Anatoliy-024", "CC BY-SA 4.0", BySa4,
            "https://commons.wikimedia.org/wiki/File:MAN_Lion%27s_Coach_Papenburger_Kanalreisen.jpg"),
        ["Mercedes-Benz Tourismo"] = new("/img/filo/tourismo.jpg", "Matti Blume", "CC BY-SA 4.0", BySa4,
            "https://commons.wikimedia.org/wiki/File:Mercedes-Benz_Tourismo,_Berlin_(20180910_131307).jpg"),
    };

    public static FiloFotografi? Bul(string model) => Modeller.GetValueOrDefault(model);
}
