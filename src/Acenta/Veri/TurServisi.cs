using System.Globalization;
using System.Text;
using Acenta.Ayarlar;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Veri;

/// <summary>Sitede gösterilen tur sorguları ve yardımcılar.</summary>
public class TurServisi(AcentaVeri db, AyarDeposu ayarlar)
{
    public static readonly CultureInfo Tr = new("tr-TR", useUserOverride: false);

    /// <summary>Yayındaki turlar, tarihleriyle.</summary>
    public IQueryable<Tur> Yayindakiler() =>
        db.Turlar.AsNoTracking().Include(t => t.Tarihler).Where(t => t.Yayinda);

    /// <summary>En yakın kalkış tarihine göre sıralı, gelecekte tarihi olan yayındaki turlar.</summary>
    public async Task<List<Tur>> YaklasanlarAsync(int adet, CancellationToken iptal = default)
    {
        var bugun = DateTime.Today;
        var turlar = await Yayindakiler().Where(t => t.Tarihler.Any(d => d.Tarih >= bugun)).ToListAsync(iptal);
        return turlar.OrderByDescending(t => t.OneCikan)
            .ThenBy(t => t.GelecekTarihler(bugun).First().Tarih)
            .Take(adet).ToList();
    }

    public static string Fotograf(Tur t) =>
        string.IsNullOrEmpty(t.Fotograf) ? Bolgeler.Bul(t.Bolge)?.Fotograf ?? "/img/yerler/kapadokya.jpg" : "/foto/" + t.Fotograf;

    public static string Para(decimal tutar) => tutar.ToString("#,0", Tr) + " ₺";

    public static string KisaTarih(DateTime d) => d.ToString("d MMM", Tr);
    public static string UzunTarih(DateTime d) => d.ToString("d MMMM yyyy dddd", Tr);
    public static string GunluTarih(DateTime d) => d.ToString("d MMMM ddd", Tr);

    /// <summary>Tur adı ve tarihi hazır yazılmış WhatsApp rezervasyon mesajı; WhatsApp kapalıysa null.</summary>
    public string? Rezervasyon(Tur t, TurTarihi? tarih)
    {
        var a = ayarlar.Al();
        if (!a.WhatsAppGoster) return null;
        var mesaj = tarih is null
            ? $"Merhaba, \"{t.Ad}\" için rezervasyon yaptırmak istiyorum.\nTarih: \nKişi sayısı: "
            : $"Merhaba, \"{t.Ad}\" için rezervasyon yaptırmak istiyorum.\nTarih: {UzunTarih(tarih.Tarih)}\nKişi sayısı: ";
        return a.WhatsAppBaglantisi(mesaj);
    }

    /// <summary>Türkçe karakterleri sadeleştirip adres için kısa ad üretir.</summary>
    public static string KodUret(string ad)
    {
        var s = ad.Trim().ToLower(Tr)
            .Replace('ı', 'i').Replace('ğ', 'g').Replace('ü', 'u').Replace('ş', 's').Replace('ö', 'o').Replace('ç', 'c');
        var sb = new StringBuilder();
        foreach (var c in s.Normalize(NormalizationForm.FormD))
        {
            if (char.IsAsciiLetterOrDigit(c)) sb.Append(c);
            else if ((c == ' ' || c == '-') && sb.Length > 0 && sb[^1] != '-') sb.Append('-');
        }
        return sb.ToString().Trim('-');
    }
}
