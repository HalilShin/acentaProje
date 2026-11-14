using System.ComponentModel.DataAnnotations;
using Locktech.Ayarlar;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locktech.Pages.Panel;

public class IndexModel(AyarDeposu depo, PostaServisi posta, ILogger<IndexModel> log) : PageModel
{
    [BindProperty] public AyarGirdisi Girdi { get; set; } = new();
    public bool SifreKayitli { get; private set; }

    [TempData] public string? Basari { get; set; }
    [TempData] public string? Hata { get; set; }

    public void OnGet()
    {
        var a = depo.Al();
        SifreKayitli = a.SmtpSifre.Length > 0;
        Girdi = new AyarGirdisi
        {
            Eposta = a.Eposta,
            SmtpSunucu = a.SmtpSunucu,
            SmtpPort = a.SmtpPort,
            SmtpGuvenlik = a.SmtpGuvenlik,
            SmtpKullanici = a.SmtpKullanici,
            GonderenAdres = a.GonderenAdres,
            GonderenAd = a.GonderenAd,
            AliciAdres = a.AliciAdres,
            WhatsAppGoster = a.WhatsAppGoster,
            WhatsAppNumara = a.WhatsAppNumara,
            WhatsAppMesaj = a.WhatsAppMesaj,
        };
    }

    public IActionResult OnPostKaydet()
    {
        if (!Kaydet()) return Page();
        Basari = "Ayarlar kaydedildi.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostTestAsync(CancellationToken iptal)
    {
        if (!Kaydet()) return Page();
        try
        {
            await posta.GonderAsync("Locktech panel — test e-postası",
                "Bu e-posta, Locktech yönetim panelindeki SMTP ayarlarının çalıştığını doğrulamak için gönderildi.", iptal: iptal);
            Basari = $"Ayarlar kaydedildi ve test e-postası {depo.Al().AliciAdres} adresine gönderildi.";
        }
        catch (Exception ex)
        {
            log.LogWarning(ex, "Test e-postası gönderilemedi.");
            Hata = "Ayarlar kaydedildi ancak test e-postası gönderilemedi: " + ex.Message;
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCikisAsync()
    {
        await HttpContext.SignOutAsync();
        return RedirectToPage("/Panel/Giris");
    }

    private bool Kaydet()
    {
        var onceki = depo.Al();
        SifreKayitli = onceki.SmtpSifre.Length > 0;

        var numara = SiteAyarlari.NumaraDuzenle(Girdi.WhatsAppNumara);
        if (Girdi.WhatsAppGoster && numara is null)
            ModelState.AddModelError("Girdi.WhatsAppNumara", "WhatsApp numarası geçerli değil.");
        if (!ModelState.IsValid) return false;

        depo.Kaydet(onceki with
        {
            Eposta = string.IsNullOrWhiteSpace(Girdi.Eposta) ? onceki.Eposta : Girdi.Eposta.Trim(),
            SmtpSunucu = Girdi.SmtpSunucu?.Trim() ?? "",
            SmtpPort = Girdi.SmtpPort,
            SmtpGuvenlik = Girdi.SmtpGuvenlik,
            SmtpKullanici = Girdi.SmtpKullanici?.Trim() ?? "",
            // Şifre alanı boş bırakıldıysa kayıtlı şifre korunur.
            SmtpSifre = string.IsNullOrEmpty(Girdi.SmtpSifre) ? onceki.SmtpSifre : Girdi.SmtpSifre,
            GonderenAdres = Girdi.GonderenAdres?.Trim() ?? "",
            GonderenAd = Girdi.GonderenAd?.Trim() ?? "",
            AliciAdres = Girdi.AliciAdres?.Trim() ?? "",
            WhatsAppGoster = Girdi.WhatsAppGoster,
            WhatsAppNumara = numara ?? Girdi.WhatsAppNumara?.Trim() ?? "",
            WhatsAppMesaj = Girdi.WhatsAppMesaj?.Trim() ?? "",
        });
        return true;
    }

    public class AyarGirdisi
    {
        [EmailAddress(ErrorMessage = "Sitede görünen e-posta geçerli değil."), StringLength(150)] public string? Eposta { get; set; }
        [StringLength(200)] public string? SmtpSunucu { get; set; }
        [Range(1, 65535, ErrorMessage = "Port 1–65535 arasında olmalı.")] public int SmtpPort { get; set; } = 587;
        [RegularExpression("Auto|StartTls|SslOnConnect|None")] public string SmtpGuvenlik { get; set; } = "Auto";
        [StringLength(200)] public string? SmtpKullanici { get; set; }
        [StringLength(200)] public string? SmtpSifre { get; set; }
        [EmailAddress(ErrorMessage = "Gönderen adres geçerli bir e-posta olmalı.")] public string? GonderenAdres { get; set; }
        [StringLength(100)] public string? GonderenAd { get; set; }
        [StringLength(500)] public string? AliciAdres { get; set; }
        public bool WhatsAppGoster { get; set; }
        [StringLength(30)] public string? WhatsAppNumara { get; set; }
        [StringLength(300)] public string? WhatsAppMesaj { get; set; }
    }
}
