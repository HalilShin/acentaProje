using Acenta.Veri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages.Panel;

public record KalkisSatiri(TurTarihi Kalkis, int Yolcu, int KayitSayisi, int IptalSayisi, decimal Tutar, decimal Alinan, int Binen)
{
    public decimal Kalan => Math.Max(0, Tutar - Alinan);
    public int Doluluk => Kalkis.Kontenjan == 0 ? 0 : (int)Math.Round(100.0 * Yolcu / Kalkis.Kontenjan);
}

/// <summary>Tarih aralığına göre kalkış raporu: doluluk, tahsilat, kalan ödemeler. CSV olarak da indirilir.</summary>
public class RaporModel(AcentaVeri db) : PageModel
{
    [BindProperty(SupportsGet = true)] public DateTime? Baslangic { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? Bitis { get; set; }
    [BindProperty(SupportsGet = true)] public int? TurId { get; set; }

    public List<KalkisSatiri> Satirlar { get; private set; } = [];
    public List<Kayit> Bekleyenler { get; private set; } = [];
    public List<Tur> Turlar { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken iptal) => await HesaplaAsync(iptal);

    private static readonly Sutun[] RaporSutunlari =
    [
        new("Tarih", 1), new("Tur", 3), new("Kontenjan", 0.9f), new("Dolu", 0.7f), new("Doluluk %", 0.9f), new("Kayıt", 0.7f),
        new("İptal", 0.6f), new("Bindi", 0.7f), new("Ücret", 1.2f, Para: true), new("Alınan", 1.2f, Para: true), new("Kalan", 1.2f, Para: true),
    ];

    private IEnumerable<object?[]> RaporSatirlari() => Satirlar.Select(s => new object?[]
    {
        s.Kalkis.Tarih, s.Kalkis.Tur.Ad, s.Kalkis.Kontenjan, s.Yolcu, s.Doluluk, s.KayitSayisi, s.IptalSayisi, s.Binen, s.Tutar, s.Alinan, s.Kalan,
    });

    private object?[] RaporToplami() =>
    [
        "Toplam", $"{Satirlar.Count} kalkış", Satirlar.Sum(s => s.Kalkis.Kontenjan), Satirlar.Sum(s => s.Yolcu), null,
        Satirlar.Sum(s => s.KayitSayisi), Satirlar.Sum(s => s.IptalSayisi), Satirlar.Sum(s => s.Binen),
        Satirlar.Sum(s => s.Tutar), Satirlar.Sum(s => s.Alinan), Satirlar.Sum(s => s.Kalan),
    ];

    private static readonly Sutun[] BekleyenSutunlari =
    [
        new("Kalkış", 1), new("Tur", 2.6f), new("Rezervasyon", 1.8f), new("Telefon", 1.3f), new("Kişi", 0.6f),
        new("Ücret", 1.1f, Para: true), new("Alınan", 1.1f, Para: true), new("Kalan", 1.1f, Para: true),
    ];

    private IEnumerable<object?[]> BekleyenSatirlari() => Bekleyenler.Select(k => new object?[]
    {
        k.TurTarihi.Tarih, k.TurTarihi.Tur.Ad, k.IletisimAd, k.Telefon, k.Yolcular.Count, k.Tutar, k.Alinan, k.Kalan,
    });

    private string Aralik => $"{Baslangic:dd.MM.yyyy} – {Bitis:dd.MM.yyyy}" + (Turlar.FirstOrDefault(t => t.Id == TurId) is { } t ? $" · {t.Ad}" : "");
    private string DosyaAdi(string uzanti) => $"kalkis-raporu-{Baslangic:yyyy-MM-dd}-{Bitis:yyyy-MM-dd}.{uzanti}";

    public async Task<IActionResult> OnGetExcelAsync(CancellationToken iptal)
    {
        await HesaplaAsync(iptal);
        var veri = Dokum.Excel("Kalkışlar", "Kalkış raporu", Aralik, RaporSutunlari, RaporSatirlari(), RaporToplami());
        return File(veri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DosyaAdi("xlsx"));
    }

    public async Task<IActionResult> OnGetPdfAsync(CancellationToken iptal)
    {
        await HesaplaAsync(iptal);
        var veri = Dokum.Pdf("Kalkış raporu", Aralik, RaporSutunlari, RaporSatirlari(), RaporToplami());
        return File(veri, "application/pdf", DosyaAdi("pdf"));
    }

    /// <summary>Ödemesi tamamlanmamış kayıtlar (tahsilat takibi).</summary>
    public async Task<IActionResult> OnGetBekleyenExcelAsync(CancellationToken iptal)
    {
        await HesaplaAsync(iptal);
        var veri = Dokum.Excel("Bekleyen ödemeler", "Ödemesi tamamlanmamış kayıtlar", Aralik, BekleyenSutunlari, BekleyenSatirlari(),
            ["Toplam", null, null, null, Bekleyenler.Sum(k => k.Yolcular.Count), Bekleyenler.Sum(k => k.Tutar), Bekleyenler.Sum(k => k.Alinan), Bekleyenler.Sum(k => k.Kalan)]);
        return File(veri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"bekleyen-odemeler-{Baslangic:yyyy-MM-dd}-{Bitis:yyyy-MM-dd}.xlsx");
    }

    private async Task HesaplaAsync(CancellationToken iptal)
    {
        // Varsayılan: geçen ayın başından üç ay sonrasına.
        var bugun = DateTime.Today;
        Baslangic ??= new DateTime(bugun.Year, bugun.Month, 1).AddMonths(-1);
        Bitis ??= bugun.AddMonths(3);
        if (Bitis < Baslangic) (Baslangic, Bitis) = (Bitis, Baslangic);

        Turlar = await db.Turlar.AsNoTracking().OrderBy(t => t.Ad).ToListAsync(iptal);

        var sorgu = db.TurTarihleri.AsNoTracking().Include(t => t.Tur)
            .Where(t => t.Tarih >= Baslangic.Value.Date && t.Tarih <= Bitis.Value.Date);
        if (TurId is not null) sorgu = sorgu.Where(t => t.TurId == TurId);
        var kalkislar = await sorgu.OrderBy(t => t.Tarih).ThenBy(t => t.Tur.Ad).ToListAsync(iptal);
        var idler = kalkislar.Select(k => k.Id).ToList();

        var kayitlar = await db.Kayitlar.AsNoTracking().Include(k => k.Yolcular).Include(k => k.TurTarihi).ThenInclude(t => t.Tur)
            .Where(k => idler.Contains(k.TurTarihiId)).ToListAsync(iptal);

        Satirlar = kalkislar.Select(k =>
        {
            var aktif = kayitlar.Where(x => x.TurTarihiId == k.Id && !x.Iptal).ToList();
            return new KalkisSatiri(k, aktif.Sum(x => x.Yolcular.Count), aktif.Count, kayitlar.Count(x => x.TurTarihiId == k.Id && x.Iptal),
                aktif.Sum(x => x.Tutar), aktif.Sum(x => x.Alinan), aktif.Sum(x => x.Yolcular.Count(y => y.Bindi == true)));
        }).ToList();

        Bekleyenler = kayitlar.Where(x => !x.Iptal && x.Kalan > 0).OrderBy(x => x.TurTarihi.Tarih).ThenBy(x => x.IletisimAd).ToList();
    }
}
