using Acenta.Veri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages.Panel;

/// <summary>Bir kalkışın kayıtları, tur günü yoklaması, yazdırılabilir yolcu listesi , Excel ve PDF dökümü.</summary>
public class KalkisModel(AcentaVeri db) : PageModel
{
    public TurTarihi Kalkis { get; private set; } = null!;
    public List<Kayit> Kayitlar { get; private set; } = [];
    [TempData] public string? Basari { get; set; }

    public IEnumerable<Kayit> Aktifler => Kayitlar.Where(k => !k.Iptal);
    public int YolcuSayisi => Aktifler.Sum(k => k.Yolcular.Count);
    public int Binen => Aktifler.Sum(k => k.Yolcular.Count(y => y.Bindi == true));
    public int Gelmeyen => Aktifler.Sum(k => k.Yolcular.Count(y => y.Bindi == false));

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken iptal)
    {
        if (!await YukleAsync(id, iptal)) return NotFound();
        return Page();
    }

    /// <summary>Yoklama: yolcuyu "bindi", "gelmedi" ya da "bekleniyor" olarak işaretler. JavaScript ile çağrılır; JS yoksa sayfaya döner.</summary>
    public async Task<IActionResult> OnPostYoklamaAsync(int id, int yolcuId, string durum, CancellationToken iptal)
    {
        var yolcu = await db.Yolcular.Include(y => y.Kayit).FirstOrDefaultAsync(y => y.Id == yolcuId && y.Kayit.TurTarihiId == id, iptal);
        if (yolcu is null) return NotFound();
        yolcu.Bindi = durum switch { "bindi" => true, "gelmedi" => false, _ => null };
        yolcu.YoklamaZamani = yolcu.Bindi is null ? null : DateTime.Now;
        await db.SaveChangesAsync(iptal);
        if (Request.Headers.XRequestedWith == "fetch") return new JsonResult(new { durum });
        return Redirect($"/panel/kalkis/{id}#y{yolcuId}");
    }

    private static readonly Sutun[] YolcuSutunlari =
    [
        new("#", 0.4f), new("Ad Soyad", 2.2f), new("TC / Pasaport", 1.3f), new("Rezervasyon", 1.8f), new("Telefon", 1.4f),
        new("Biniş", 1.1f), new("Ödeme", 0.9f), new("Kalan", 1, Para: true), new("Not", 1.6f), new("Yoklama", 0.9f),
    ];

    private IEnumerable<object?[]> YolcuSatirlari()
    {
        var sira = 1;
        foreach (var k in Aktifler)
        {
            var ilk = true;
            foreach (var y in k.Yolcular.OrderBy(y => y.Sira))
            {
                // Rezervasyon bilgileri yalnızca grubun ilk yolcusunda yazılır.
                yield return [sira++, y.AdSoyad, y.KimlikNo, ilk ? k.IletisimAd : "", ilk ? k.Telefon : "", ilk ? k.BinisYeri : "",
                    ilk ? k.OdemeDurumu : "", ilk ? k.Kalan : null, ilk ? k.Not : "",
                    y.Bindi switch { true => "Bindi", false => "Gelmedi", _ => "" }];
                ilk = false;
            }
        }
    }

    private string DosyaAdi(string uzanti) => $"yolcu-listesi-{Kalkis.Tarih:yyyy-MM-dd}-{Kalkis.Tur.Kod}.{uzanti}";
    private string AltBaslik => $"{TurServisi.UzunTarih(Kalkis.Tarih)} · {YolcuSayisi} / {Kalkis.Kontenjan} yolcu"
        + (string.IsNullOrWhiteSpace(Kalkis.Tur.Bulusma) ? "" : $" · Buluşma: {Kalkis.Tur.Bulusma}");

    /// <summary>Yolcu listesi Excel dosyası.</summary>
    public async Task<IActionResult> OnGetExcelAsync(int id, CancellationToken iptal)
    {
        if (!await YukleAsync(id, iptal)) return NotFound();
        var veri = Dokum.Excel("Yolcu listesi", Kalkis.Tur.Ad, AltBaslik, YolcuSutunlari, YolcuSatirlari());
        return File(veri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DosyaAdi("xlsx"));
    }

    /// <summary>Yolcu listesi PDF: tur günü yoklama için imza sütunlu.</summary>
    public async Task<IActionResult> OnGetPdfAsync(int id, CancellationToken iptal)
    {
        if (!await YukleAsync(id, iptal)) return NotFound();
        Sutun[] sutunlar = [.. YolcuSutunlari[..^1], new("İmza", 1.2f, Bos: true)];
        var satirlar = YolcuSatirlari().Select(s => s[..^1].Append(null).ToArray());
        var veri = Dokum.Pdf($"Yolcu listesi · {Kalkis.Tur.Ad}", AltBaslik, sutunlar, satirlar);
        return File(veri, "application/pdf", DosyaAdi("pdf"));
    }

    private async Task<bool> YukleAsync(int id, CancellationToken iptal)
    {
        var k = await db.TurTarihleri.AsNoTracking().Include(t => t.Tur).FirstOrDefaultAsync(t => t.Id == id, iptal);
        if (k is null) return false;
        Kalkis = k;
        Kayitlar = await db.Kayitlar.AsNoTracking().Include(x => x.Yolcular)
            .Where(x => x.TurTarihiId == id).OrderBy(x => x.Iptal).ThenBy(x => x.Olusturma).ToListAsync(iptal);
        return true;
    }
}
