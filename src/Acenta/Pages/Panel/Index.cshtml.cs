using Acenta.Veri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages.Panel;

public class IndexModel(AcentaVeri db, FotografDeposu fotograflar) : PageModel
{
    public List<Tur> Turlar { get; private set; } = [];
    [TempData] public string? Basari { get; set; }
    [TempData] public string? Hata { get; set; }

    public async Task OnGetAsync(CancellationToken iptal)
    {
        Turlar = await db.Turlar.AsNoTracking().Include(t => t.Tarihler)
            .OrderByDescending(t => t.Yayinda).ThenBy(t => t.Ad).ToListAsync(iptal);
    }

    /// <summary>
    /// Turu bütün bilgileri ve fotoğrafıyla kopyalar; tarihler ve kayıtlar kopyalanmaz.
    /// Kopya taslak olarak açılır, adı ve tarihleri düzenlendikten sonra yayına alınır.
    /// </summary>
    public async Task<IActionResult> OnPostKopyalaAsync(int id, CancellationToken iptal)
    {
        var kaynak = await db.Turlar.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, iptal);
        if (kaynak is null) return RedirectToPage();

        var kok = kaynak.Kod.Length > 100 ? kaynak.Kod[..100] : kaynak.Kod;
        var kod = kok + "-kopya";
        for (var n = 2; await db.Turlar.AnyAsync(t => t.Kod == kod, iptal); n++) kod = $"{kok}-kopya-{n}";

        var kopya = new Tur
        {
            Kod = kod, Ad = (kaynak.Ad.Length > 150 ? kaynak.Ad[..150] : kaynak.Ad) + " (kopya)", Bolge = kaynak.Bolge, Turu = kaynak.Turu, Sure = kaynak.Sure, Ulasim = kaynak.Ulasim,
            Ozet = kaynak.Ozet, Program = kaynak.Program, Dahil = kaynak.Dahil, Haric = kaynak.Haric, Notlar = kaynak.Notlar,
            Fiyat = kaynak.Fiyat, Fotograf = fotograflar.Kopyala(kaynak.Fotograf), Yayinda = false, OneCikan = false,
        };
        db.Turlar.Add(kopya);
        await db.SaveChangesAsync(iptal);
        TempData["Basari"] = "Tur kopyalandı. Adını ve tarihlerini düzenleyip \"Sitede yayında\" işaretleyerek yayına alın.";
        return Redirect($"/panel/tur/{kopya.Id}");
    }

    public async Task<IActionResult> OnPostSilAsync(int id, CancellationToken iptal)
    {
        var tur = await db.Turlar.FindAsync([id], iptal);
        if (tur is not null && await db.Kayitlar.AnyAsync(k => k.TurTarihi.TurId == id, iptal))
        {
            Basari = null;
            TempData["Hata"] = $"\"{tur.Ad}\" turunun kalkışlarında kayıt var; silinemez. Yayından kaldırmak için düzenleyip \"Sitede yayında\" işaretini kaldırın.";
            return RedirectToPage();
        }
        if (tur is not null)
        {
            db.Turlar.Remove(tur);
            await db.SaveChangesAsync(iptal);
            fotograflar.Sil(tur.Fotograf);
            Basari = $"\"{tur.Ad}\" silindi.";
        }
        return RedirectToPage();
    }
}
