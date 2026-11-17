using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.RateLimiting;
using Acenta.Ayarlar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acenta.Pages;

public class IletisimModel(AyarDeposu depo, PostaServisi posta, ILogger<IletisimModel> log) : PageModel
{
    /// <summary>Aynı IP'den 10 dakikada en çok 5 gönderim.</summary>
    private static readonly PartitionedRateLimiter<string> GonderimSiniri = PartitionedRateLimiter.Create<string, string>(
        ip => RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(10) }));

    [BindProperty] public BilgiFormu Form { get; set; } = new();
    [TempData] public bool Gonderildi { get; set; }
    public string? Hata { get; private set; }

    /// <summary>Tur sayfasından gelindiyse (?tur=ad) formda tur adı yazılı gelir.</summary>
    public void OnGet(string? tur)
    {
        if (!string.IsNullOrWhiteSpace(tur)) Form.Tur = tur.Trim()[..Math.Min(tur.Trim().Length, 160)];
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken iptal)
    {
        var geriDon = Redirect("/iletisim#form");

        // Gizli alan doluysa bot doldurmuştur; göndermeden başarılı gibi davran.
        if (!string.IsNullOrEmpty(Form.WebSitesi))
        {
            Gonderildi = true;
            return geriDon;
        }

        if (string.IsNullOrWhiteSpace(Form.AdSoyad))
            ModelState.AddModelError("Form.AdSoyad", "Adınızı yazın.");
        if (string.IsNullOrWhiteSpace(Form.Telefon))
            ModelState.AddModelError("Form.Telefon", "Telefon numaranızı yazın.");
        if (!string.IsNullOrWhiteSpace(Form.Eposta) && !MailAddress.TryCreate(Form.Eposta, out _))
            ModelState.AddModelError("Form.Eposta", "E-posta adresi geçerli değil.");
        if (!ModelState.IsValid) return Page();

        var telefon = depo.Al().Telefon;
        using var izin = GonderimSiniri.AttemptAcquire(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "bilinmiyor");
        if (!izin.IsAcquired)
        {
            Hata = $"Kısa sürede çok fazla mesaj gönderildi. Lütfen biraz sonra yeniden deneyin ya da {telefon} numarasını arayın.";
            return Page();
        }

        var govde = $"""
            {Ayarlar.SiteAyarlari.SiteAdi} web sitesinden yeni mesaj

            Ad Soyad    : {Form.AdSoyad}
            Telefon     : {Form.Telefon}
            E-posta     : {Form.Eposta}
            Tur         : {Form.Tur}
            Kişi sayısı : {Form.KisiSayisi}

            Mesaj:
            {Form.Mesaj}

            Gönderim: {DateTime.Now:dd.MM.yyyy HH:mm} · IP {HttpContext.Connection.RemoteIpAddress}
            """;

        try
        {
            await posta.GonderAsync($"[{Ayarlar.SiteAyarlari.SiteAdi}] Mesaj — {Form.AdSoyad}", govde, Form.Eposta, iptal);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "İletişim formu gönderilemedi. İstek: {Istek}", HttpContext.TraceIdentifier);
            Hata = $"Mesajınız şu an iletilemedi. Lütfen {telefon} numarasını arayın ya da WhatsApp'tan yazın.";
            return Page();
        }

        Gonderildi = true;
        return geriDon;
    }

    public class BilgiFormu
    {
        [StringLength(100)] public string? AdSoyad { get; set; }
        [StringLength(30)] public string? Telefon { get; set; }
        [StringLength(150)] public string? Eposta { get; set; }
        [StringLength(160)] public string? Tur { get; set; }
        [Range(1, 999)] public int? KisiSayisi { get; set; }
        [StringLength(2000)] public string? Mesaj { get; set; }

        /// <summary>Bot tuzağı; gerçek ziyaretçi bu alanı görmez.</summary>
        public string? WebSitesi { get; set; }
    }
}
