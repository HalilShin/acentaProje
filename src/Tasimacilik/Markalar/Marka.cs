namespace Tasimacilik.Markalar;

public record Hizmet(string Kod, string Ad, string Ikon, string Ozet, string[] Paragraflar, string[] Maddeler)
{
    public string Adres => $"/hizmetler/{Kod}";
}

/// <summary>Tur: araç çiziminin türü (otobus, midibus, minibus, vip).</summary>
public record Arac(string Tur, string Ad, string Kapasite, string Aciklama, string[] Ozellikler);

/// <summary>
/// Bir markanın kimliği ve tüm metinleri. Üç marka aynı uygulamada çalışır;
/// hangisinin gösterileceği isteğin geldiği alan adına göre seçilir.
/// Metinler bilerek markadan markaya farklı yazılmıştır (kopya içerik olmaması için).
/// </summary>
public record Marka
{
    public required string Kod { get; init; }
    public required string Ad { get; init; }
    public required string Domain { get; init; }

    // Logo: sembol (SVG iç içeriği, 48x48) ve iki satırlı yazı
    public required string Sembol { get; init; }
    public required string LogoUst { get; init; }
    public required string LogoAlt { get; init; }

    // Görsel kimlik
    public required string RenkAna { get; init; }
    public required string RenkKoyu { get; init; }
    public required string RenkVurgu { get; init; }
    public required string RenkAcik { get; init; }
    public required string FontBaslik { get; init; }
    public required string FontAdresi { get; init; }
    /// <summary>Ana sayfa görselindeki araç türü.</summary>
    public required string VitrinAraci { get; init; }

    // Arama motoru
    public required string Baslik { get; init; }
    public required string Aciklama { get; init; }

    // Ana sayfa
    public required string Etiket { get; init; }
    public required string HeroBaslik { get; init; }
    public required string HeroVurgu { get; init; }
    public required string HeroMetin { get; init; }
    public required string[] Rozetler { get; init; }

    public required string HizmetlerBaslik { get; init; }
    public required string HizmetlerMetin { get; init; }
    public required Hizmet[] Hizmetler { get; init; }

    public required string NedenBaslik { get; init; }
    public required (string Baslik, string Metin)[] Nedenler { get; init; }

    public required string AdimlarBaslik { get; init; }
    public required (string Baslik, string Metin)[] Adimlar { get; init; }

    public required string AraclarBaslik { get; init; }
    public required string AraclarMetin { get; init; }
    public required Arac[] Araclar { get; init; }

    public required (string Soru, string Cevap)[] Sss { get; init; }

    public required string HakkimizdaBaslik { get; init; }
    public required string[] Hakkimizda { get; init; }

    public required string TeklifBaslik { get; init; }
    public required string TeklifMetin { get; init; }
    public required string WhatsAppMesaj { get; init; }

    public string SiteAdresi => $"https://{Domain}";
    public Hizmet? HizmetBul(string? kod) =>
        Hizmetler.FirstOrDefault(h => string.Equals(h.Kod, kod, StringComparison.OrdinalIgnoreCase));
}

/// <summary>Geçerli isteğin markası; ara katman doldurur, sayfalar @inject ile okur.</summary>
public class MarkaBaglami
{
    public Marka Marka { get; set; } = MarkaKatalogu.Tumu[0];
}
