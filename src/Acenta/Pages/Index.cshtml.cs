using Acenta.Veri;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages;

public record AyOzeti(string Kod, string Ad, string KisaAd, int Sayi);

public class IndexModel(TurServisi turlar) : PageModel
{
    public List<Tur> Yaklasanlar { get; private set; } = [];
    public Dictionary<string, int> BolgeSayilari { get; private set; } = [];
    public List<AyOzeti> Aylar { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken iptal)
    {
        Yaklasanlar = await turlar.YaklasanlarAsync(6, iptal);
        var hepsi = await turlar.Yayindakiler().ToListAsync(iptal);
        BolgeSayilari = hepsi.GroupBy(t => t.Bolge).ToDictionary(g => g.Key, g => g.Count());
        Aylar = AylariHesapla(hepsi);
    }

    /// <summary>Gelecekteki kalkışları aylara göre sayar (yyyy-MM).</summary>
    public static List<AyOzeti> AylariHesapla(IEnumerable<Tur> turlar)
    {
        var bugun = DateTime.Today;
        return turlar.SelectMany(t => t.GelecekTarihler(bugun))
            .GroupBy(d => new DateTime(d.Tarih.Year, d.Tarih.Month, 1))
            .OrderBy(g => g.Key)
            .Select(g => new AyOzeti(g.Key.ToString("yyyy-MM"), g.Key.ToString("MMMM yyyy", TurServisi.Tr), g.Key.ToString("MMMM", TurServisi.Tr), g.Count()))
            .ToList();
    }
}
