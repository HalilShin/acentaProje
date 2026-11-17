using Acenta.Veri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages.Turlar;

public class DetayModel(TurServisi turlar) : PageModel
{
    public Tur Tur { get; private set; } = null!;
    public List<TurTarihi> Tarihler { get; private set; } = [];
    public List<Tur> Benzerler { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(string kod, CancellationToken iptal)
    {
        var tur = await turlar.Yayindakiler().FirstOrDefaultAsync(t => t.Kod == kod, iptal);
        if (tur is null) return NotFound();
        Tur = tur;
        Tarihler = tur.GelecekTarihler(DateTime.Today).ToList();

        var yaklasanlar = await turlar.YaklasanlarAsync(12, iptal);
        Benzerler = yaklasanlar.Where(t => t.Id != tur.Id)
            .OrderByDescending(t => t.Bolge == tur.Bolge)
            .Take(3).ToList();
        return Page();
    }
}
