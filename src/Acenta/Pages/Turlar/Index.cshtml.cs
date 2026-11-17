using Acenta.Veri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages.Turlar;

public class IndexModel(TurServisi turlar) : PageModel
{
    [BindProperty(SupportsGet = true)] public string? Bolge { get; set; }
    /// <summary>yyyy-MM</summary>
    [BindProperty(SupportsGet = true)] public string? Ay { get; set; }
    /// <summary>gunubirlik | konaklamali</summary>
    [BindProperty(SupportsGet = true)] public string? Tur { get; set; }

    public List<Tur> Sonuclar { get; private set; } = [];
    /// <summary>Ay seçildiyse kartlar yalnızca o ayın kalkışlarını gösterir.</summary>
    public DateTime? SeciliAy { get; private set; }
    public List<AyOzeti> Aylar { get; private set; } = [];
    public bool Suzuldu => !string.IsNullOrEmpty(Bolge) || !string.IsNullOrEmpty(Ay) || !string.IsNullOrEmpty(Tur);

    public string Baslik =>
        (Tur switch { "gunubirlik" => "Günübirlik turlar", "konaklamali" => "Konaklamalı turlar", _ => "Turlar" })
        + (Bolgeler.Bul(Bolge) is { } b ? $" · {b.Ad}" : "");

    public async Task OnGetAsync(CancellationToken iptal)
    {
        var bugun = DateTime.Today;
        var hepsi = await turlar.Yayindakiler().ToListAsync(iptal);
        Aylar = Pages.IndexModel.AylariHesapla(hepsi);

        IEnumerable<Tur> s = hepsi;
        if (Bolgeler.Bul(Bolge) is { } bolge) s = s.Where(t => t.Bolge == bolge.Kod);
        if (Tur == "gunubirlik") s = s.Where(t => t.Turu == TurTuru.Gunubirlik);
        else if (Tur == "konaklamali") s = s.Where(t => t.Turu == TurTuru.Konaklamali);
        if (DateTime.TryParseExact(Ay + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var ay))
        {
            SeciliAy = ay;
            s = s.Where(t => t.GelecekTarihler(bugun).Any(d => d.Tarih.Year == ay.Year && d.Tarih.Month == ay.Month));
        }

        // Tarihi yaklaşanlar önce; tarihi olmayanlar en sonda.
        Sonuclar = s.OrderBy(t => t.GelecekTarihler(bugun).Select(d => (DateTime?)d.Tarih).FirstOrDefault() ?? DateTime.MaxValue)
            .ThenBy(t => t.Ad).ToList();
    }
}
