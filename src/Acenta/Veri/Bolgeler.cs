namespace Acenta.Veri;

public record Bolge(string Kod, string Ad, string Fotograf);

/// <summary>Tur bölgeleri. Turun kendi fotoğrafı yoksa bölge fotoğrafı kullanılır.</summary>
public static class Bolgeler
{
    public static readonly Bolge[] Tumu =
    [
        new("kapadokya", "Kapadokya", "/img/yerler/kapadokya.jpg"),
        new("gap", "GAP", "/img/yerler/gobeklitepe.jpg"),
        new("karadeniz", "Karadeniz", "/img/yerler/uzungol.jpg"),
        new("ege", "Ege", "/img/yerler/pamukkale.jpg"),
        new("dogu", "Doğu Anadolu", "/img/yerler/ishakpasa.jpg"),
        new("bati-karadeniz", "Batı Karadeniz", "/img/yerler/safranbolu.jpg"),
    ];

    public static Bolge? Bul(string? kod) => Tumu.FirstOrDefault(b => b.Kod == kod);

    public static string Ad(string? kod) => Bul(kod)?.Ad ?? kod ?? "";
}
