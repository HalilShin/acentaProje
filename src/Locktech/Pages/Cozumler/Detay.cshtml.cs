using Locktech.Cozumler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locktech.Pages.Cozumler;

public class DetayModel : PageModel
{
    public Cozum Cozum { get; private set; } = null!;
    public IEnumerable<Cozum> Digerleri => CozumKatalogu.Tumu.Where(c => c != Cozum);

    public IActionResult OnGet(string kod)
    {
        var cozum = CozumKatalogu.Bul(kod);
        if (cozum is null) return NotFound();
        if (kod != cozum.Kod) return RedirectPermanent(cozum.Adres);

        Cozum = cozum;
        return Page();
    }
}
