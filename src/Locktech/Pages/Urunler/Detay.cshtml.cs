using Locktech.Urunler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locktech.Pages.Urunler;

public class DetayModel : PageModel
{
    public Urun Urun { get; private set; } = null!;
    public IEnumerable<Urun> Digerleri => UrunKatalogu.Tumu.Where(u => u != Urun);

    public IActionResult OnGet(string kod)
    {
        var urun = UrunKatalogu.Bul(kod);
        if (urun is null) return NotFound();

        // Adres tek biçimde kalsın: /urunler/HB-A1SP -> /urunler/hb-a1sp
        if (kod != urun.Kod) return RedirectPermanent(urun.Adres);

        Urun = urun;
        return Page();
    }
}
