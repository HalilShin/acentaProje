namespace Acenta.Veri;

/// <summary>Sitedeki hazır fotoğraf ve lisans atfı (Wikimedia Commons, serbest lisans).</summary>
public record Fotograf(string Dosya, string Konu, string Yazar, string Lisans, string LisansAdresi, string KaynakAdresi);

/// <summary>
/// Sitede kullanılan hazır fotoğraflar (bölge ve ilk ekran). Panelden yüklenen tur fotoğrafları bu listede değildir.
/// Atıflar /fotograf-kaynaklari sayfasında gösterilir.
/// </summary>
public static class Fotograflar
{
    private const string BySa4 = "https://creativecommons.org/licenses/by-sa/4.0/";
    private const string BySa3 = "https://creativecommons.org/licenses/by-sa/3.0/";
    private const string By3 = "https://creativecommons.org/licenses/by/3.0/";
    private const string Cc0 = "https://creativecommons.org/publicdomain/zero/1.0/";
    private const string Commons = "https://commons.wikimedia.org/wiki/File:";

    public static readonly Fotograf Vitrin = new("/img/yerler/kapadokya-genis.jpg", "Kapadokya", "Wolfgang Moroder", "CC BY-SA 3.0", BySa3, Commons + "Hot_air_balloon_ride_at_sunrise_in_Cappadocia.JPG");

    public static readonly Fotograf[] Tumu =
    [
        Vitrin,
        new("/img/yerler/kapadokya.jpg", "Kapadokya", "Bernard Gagnon", "CC BY-SA 3.0", BySa3, Commons + "Hot_air_balloon_in_Cappadocia_02.jpg"),
        new("/img/yerler/gobeklitepe.jpg", "Göbeklitepe", "Teomancimit", "CC BY-SA 3.0", BySa3, Commons + "G%C3%B6bekli_Tepe,_Urfa.jpg"),
        new("/img/yerler/uzungol.jpg", "Uzungöl", "İhsan Deniz Kılıçoğlu", "CC BY-SA 3.0", BySa3, Commons + "Uzung%C3%B6l_lake_and_town.jpg"),
        new("/img/yerler/pamukkale.jpg", "Pamukkale", "rheins", "CC BY 3.0", By3, Commons + "Pamukkale_Travertines_-_2014.10_-_panoramio.jpg"),
        new("/img/yerler/ishakpasa.jpg", "İshak Paşa Sarayı", "W. Bulach", "CC BY-SA 4.0", BySa4, Commons + "00_2404_Ishak_Pasha_Palace_-_Turkey.jpg"),
        new("/img/yerler/safranbolu.jpg", "Safranbolu", "Hamdigumus", "CC0", Cc0, Commons + "Safranbolu_Evleri_2014-8.jpg"),
    ];
}
