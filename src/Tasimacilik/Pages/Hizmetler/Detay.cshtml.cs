using Tasimacilik.Markalar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tasimacilik.Pages.Hizmetler;

public class DetayModel(MarkaBaglami baglam) : PageModel
{
    public Hizmet Hizmet { get; private set; } = null!;

    public IActionResult OnGet(string kod)
    {
        // Hizmet kodları markaya özeldir; başka markanın hizmeti bu alan adında 404 verir.
        var hizmet = baglam.Marka.HizmetBul(kod);
        if (hizmet is null) return NotFound();
        if (kod != hizmet.Kod) return RedirectPermanent(hizmet.Adres);

        Hizmet = hizmet;
        return Page();
    }
}
