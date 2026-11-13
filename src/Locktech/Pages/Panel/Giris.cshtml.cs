using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;

namespace Locktech.Pages.Panel;

[EnableRateLimiting("giris")]
public class GirisModel(IConfiguration yapilandirma, ILogger<GirisModel> log) : PageModel
{
    [BindProperty] public string Sifre { get; set; } = "";
    public string? Hata { get; private set; }
    public bool SifreTanimli => !string.IsNullOrEmpty(yapilandirma["Panel:Sifre"]);

    public IActionResult OnGet() =>
        User.Identity?.IsAuthenticated == true ? RedirectToPage("/Panel/Index") : Page();

    public async Task<IActionResult> OnPostAsync()
    {
        var dogru = yapilandirma["Panel:Sifre"];
        if (string.IsNullOrEmpty(dogru)) return Page();

        // Uzunluk ve içerik sızdırmamak için özetleri sabit sürede karşılaştır.
        var esit = CryptographicOperations.FixedTimeEquals(
            SHA256.HashData(Encoding.UTF8.GetBytes(Sifre ?? "")),
            SHA256.HashData(Encoding.UTF8.GetBytes(dogru)));

        if (!esit)
        {
            log.LogWarning("Panel girişi başarısız. IP: {Ip}", HttpContext.Connection.RemoteIpAddress);
            await Task.Delay(800);
            Hata = "Şifre hatalı.";
            return Page();
        }

        var kimlik = new ClaimsIdentity([new Claim(ClaimTypes.Name, "yonetici")], CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(new ClaimsPrincipal(kimlik));
        return RedirectToPage("/Panel/Index");
    }
}
