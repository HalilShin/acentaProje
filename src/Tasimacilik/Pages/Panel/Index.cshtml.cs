using System.ComponentModel.DataAnnotations;
using Tasimacilik.Ayarlar;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tasimacilik.Pages.Panel;

public class IndexModel(AyarDeposu depo, PostaServisi posta, ILogger<IndexModel> log) : PageModel
{
    [BindProperty] public AyarGirdisi Girdi { get; set; } = new();
    public bool SifreKayitli { get; private set; }
    public bool IletisimEksik { get; private set; }

    [TempData] public string? Basari { get; set; }
    [TempData] public string? Hata { get; set; }

    public void OnGet()
    {
        var a = depo.Al();
        SifreKayitli = a.SmtpSifre.Length > 0;
        IletisimEksik = a.IletisimEksik;
        Girdi = new AyarGirdisi
        {
            Telefon = a.Telefon,
            Adres = a.Adres,
            Eposta = a.Eposta,
            WhatsAppGoster = a.WhatsAppGoster,
            WhatsAppNumara = a.WhatsAppNumara,
            SmtpSunucu = a.SmtpSunucu,
            SmtpPort = a.SmtpPort,
            SmtpGuvenlik = a.SmtpGuvenlik,
            SmtpKullanici = a.SmtpKullanici,
            GonderenAdres = a.GonderenAdres,
            GonderenAd = a.GonderenAd,
            AliciAdres = a.AliciAdres,
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
            await posta.GonderAsync("Taşımacılık paneli — test e-postası",
                "Bu e-posta, yönetim panelindeki SMTP ayarlarının çalıştığını doğrulamak için gönderildi.", iptal: iptal);
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
        IletisimEksik = onceki.IletisimEksik;

        if (SiteAyarlari.NumaraDuzenle(Girdi.Telefon) is null)
            ModelState.AddModelError("Girdi.Telefon", "Telefon numarası geçerli değil.");
        var whatsapp = SiteAyarlari.NumaraDuzenle(Girdi.WhatsAppNumara);
        if (!string.IsNullOrWhiteSpace(Girdi.WhatsAppNumara) && whatsapp is null)
            ModelState.AddModelError("Girdi.WhatsAppNumara", "WhatsApp numarası geçerli değil.");
        if (string.IsNullOrWhiteSpace(Girdi.Adres))
            ModelState.AddModelError("Girdi.Adres", "Adres boş olamaz.");
        if (!ModelState.IsValid) return false;

        depo.Kaydet(onceki with
        {
            Telefon = Girdi.Telefon!.Trim(),
            Adres = Girdi.Adres!.Trim(),
            Eposta = Girdi.Eposta?.Trim() ?? "",
            WhatsAppGoster = Girdi.WhatsAppGoster,
            WhatsAppNumara = whatsapp ?? "",
            SmtpSunucu = Girdi.SmtpSunucu?.Trim() ?? "",
            SmtpPort = Girdi.SmtpPort,
            SmtpGuvenlik = Girdi.SmtpGuvenlik,
            SmtpKullanici = Girdi.SmtpKullanici?.Trim() ?? "",
            // Şifre alanı boş bırakıldıysa kayıtlı şifre korunur.
            SmtpSifre = string.IsNullOrEmpty(Girdi.SmtpSifre) ? onceki.SmtpSifre : Girdi.SmtpSifre,
            GonderenAdres = Girdi.GonderenAdres?.Trim() ?? "",
            GonderenAd = Girdi.GonderenAd?.Trim() ?? "",
            AliciAdres = Girdi.AliciAdres?.Trim() ?? "",
        });
        return true;
    }

    public class AyarGirdisi
    {
        [StringLength(30)] public string? Telefon { get; set; }
        [StringLength(300)] public string? Adres { get; set; }
        [EmailAddress(ErrorMessage = "Sitede görünen e-posta geçerli değil."), StringLength(150)] public string? Eposta { get; set; }
        public bool WhatsAppGoster { get; set; }
        [StringLength(30)] public string? WhatsAppNumara { get; set; }
        [StringLength(200)] public string? SmtpSunucu { get; set; }
        [Range(1, 65535, ErrorMessage = "Port 1–65535 arasında olmalı.")] public int SmtpPort { get; set; } = 587;
        [RegularExpression("Auto|StartTls|SslOnConnect|None")] public string SmtpGuvenlik { get; set; } = "Auto";
        [StringLength(200)] public string? SmtpKullanici { get; set; }
        [StringLength(200)] public string? SmtpSifre { get; set; }
        [EmailAddress(ErrorMessage = "Gönderen adres geçerli bir e-posta olmalı.")] public string? GonderenAdres { get; set; }
        [StringLength(100)] public string? GonderenAd { get; set; }
        [StringLength(500)] public string? AliciAdres { get; set; }
    }
}
