using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.RateLimiting;
using Locktech.Ayarlar;
using Locktech.Urunler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Locktech.Dil;

namespace Locktech.Teklif;

/// <summary>Teklif formu bulunan sayfaların (ana sayfa, iletişim) ortak modeli.</summary>
public abstract class TeklifSayfasi(PostaServisi posta, ILogger log) : PageModel
{
    /// <summary>Aynı IP'den 10 dakikada en çok 5 teklif gönderimi.</summary>
    private static readonly PartitionedRateLimiter<string> GonderimSiniri = PartitionedRateLimiter.Create<string, string>(
        ip => RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(10) }));

    [BindProperty] public TeklifFormu Form { get; set; } = new();
    [TempData] public bool Gonderildi { get; set; }
    public string? Hata { get; private set; }

    /// <summary>Ürün sayfasından gelindiyse (?urun=hb-a1sp) formda o ürün seçili gelir.</summary>
    public void OnGet(string? urun)
    {
        if (UrunKatalogu.Bul(urun) is { } secilen) Form.Urun = secilen.FormSecenegi.ToString();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken iptal)
    {
        var geriDon = Redirect(Request.Path + "#teklif");

        // Gizli alan doluysa bot doldurmuştur; göndermeden başarılı gibi davran.
        if (!string.IsNullOrEmpty(Form.WebSitesi))
        {
            Gonderildi = true;
            return geriDon;
        }

        if (string.IsNullOrWhiteSpace(Form.AdSoyad))
            ModelState.AddModelError("Form.AdSoyad", S("Adınızı yazın.", "Please enter your name."));
        if (string.IsNullOrWhiteSpace(Form.Telefon))
            ModelState.AddModelError("Form.Telefon", S("Telefon numaranızı yazın.", "Please enter your phone number."));
        if (!string.IsNullOrWhiteSpace(Form.Eposta) && !MailAddress.TryCreate(Form.Eposta, out _))
            ModelState.AddModelError("Form.Eposta", S("E-posta adresi geçerli değil.", "The e-mail address is not valid."));
        if (!ModelState.IsValid) return Page();

        using var izin = GonderimSiniri.AttemptAcquire(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "bilinmiyor");
        if (!izin.IsAcquired)
        {
            Hata = S(
                $"Kısa sürede çok fazla talep gönderildi. Lütfen biraz sonra yeniden deneyin ya da {Site.Telefon} numarasını arayın.",
                $"Too many requests were sent in a short time. Please try again later or call {Site.Telefon}.");
            return Page();
        }

        var govde = $"""
            Web sitesinden yeni teklif talebi

            Ad Soyad : {Form.AdSoyad}
            Firma    : {Form.Firma}
            Telefon  : {Form.Telefon}
            E-posta  : {Form.Eposta}
            Ürün     : {Form.Urun}

            Mesaj:
            {Form.Mesaj}

            Gönderim: {DateTime.Now:dd.MM.yyyy HH:mm} · Sayfa {Request.Path} · Dil {(En ? "İngilizce" : "Türkçe")} · IP {HttpContext.Connection.RemoteIpAddress}
            """;

        try
        {
            await posta.GonderAsync($"Teklif talebi — {Form.AdSoyad}", govde, Form.Eposta, iptal);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Teklif formu gönderilemedi. Talep: {Ad} / {Telefon}", Form.AdSoyad, Form.Telefon);
            Hata = S(
                $"Talebiniz şu an iletilemedi. Lütfen {Site.Telefon} numarasını arayın ya da {Site.Eposta} adresine yazın.",
                $"We could not send your request right now. Please call {Site.Telefon} or write to {Site.Eposta}.");
            return Page();
        }

        Gonderildi = true;
        return geriDon;
    }

    public class TeklifFormu
    {
        [StringLength(100)] public string? AdSoyad { get; set; }
        [StringLength(150)] public string? Firma { get; set; }
        [StringLength(30)] public string? Telefon { get; set; }
        [StringLength(150)] public string? Eposta { get; set; }
        [StringLength(100)] public string? Urun { get; set; }
        [StringLength(2000)] public string? Mesaj { get; set; }

        /// <summary>Bot tuzağı; gerçek ziyaretçi bu alanı görmez.</summary>
        public string? WebSitesi { get; set; }
    }
}
