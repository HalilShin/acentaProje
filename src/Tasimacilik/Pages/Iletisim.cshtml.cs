using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.RateLimiting;
using Tasimacilik.Ayarlar;
using Tasimacilik.Markalar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tasimacilik.Pages;

public class IletisimModel(MarkaBaglami baglam, AyarDeposu depo, PostaServisi posta, ILogger<IletisimModel> log) : PageModel
{
    /// <summary>Aynı IP'den 10 dakikada en çok 5 teklif gönderimi.</summary>
    private static readonly PartitionedRateLimiter<string> GonderimSiniri = PartitionedRateLimiter.Create<string, string>(
        ip => RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(10) }));

    [BindProperty] public TeklifFormu Form { get; set; } = new();
    [TempData] public bool Gonderildi { get; set; }
    public string? Hata { get; private set; }

    /// <summary>Hizmet sayfasından gelindiyse (?hizmet=kod) formda o hizmet seçili gelir.</summary>
    public void OnGet(string? hizmet)
    {
        if (baglam.Marka.HizmetBul(hizmet) is { } secilen) Form.Hizmet = secilen.Ad;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken iptal)
    {
        var marka = baglam.Marka;
        var geriDon = Redirect("/iletisim#teklif");

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
            Hata = $"Kısa sürede çok fazla talep gönderildi. Lütfen biraz sonra yeniden deneyin ya da {telefon} numarasını arayın.";
            return Page();
        }

        var govde = $"""
            {marka.Ad} web sitesinden yeni teklif talebi

            Ad Soyad    : {Form.AdSoyad}
            Telefon     : {Form.Telefon}
            E-posta     : {Form.Eposta}
            Hizmet      : {Form.Hizmet}
            Tarih       : {Form.Tarih:dd.MM.yyyy}
            Kişi sayısı : {Form.KisiSayisi}
            Nereden     : {Form.Nereden}
            Nereye      : {Form.Nereye}

            Not:
            {Form.Mesaj}

            Gönderim: {DateTime.Now:dd.MM.yyyy HH:mm} · Site {marka.Domain} · IP {HttpContext.Connection.RemoteIpAddress}
            """;

        try
        {
            await posta.GonderAsync($"[{marka.Ad}] Teklif talebi — {Form.AdSoyad}", govde, Form.Eposta, iptal);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Teklif formu gönderilemedi. Marka: {Marka}, talep: {Ad} / {Telefon}", marka.Kod, Form.AdSoyad, Form.Telefon);
            Hata = $"Talebiniz şu an iletilemedi. Lütfen {telefon} numarasını arayın.";
            return Page();
        }

        Gonderildi = true;
        return geriDon;
    }

    public class TeklifFormu
    {
        [StringLength(100)] public string? AdSoyad { get; set; }
        [StringLength(30)] public string? Telefon { get; set; }
        [StringLength(150)] public string? Eposta { get; set; }
        [StringLength(100)] public string? Hizmet { get; set; }
        [DataType(DataType.Date)] public DateTime? Tarih { get; set; }
        [Range(1, 999)] public int? KisiSayisi { get; set; }
        [StringLength(150)] public string? Nereden { get; set; }
        [StringLength(150)] public string? Nereye { get; set; }
        [StringLength(2000)] public string? Mesaj { get; set; }

        /// <summary>Bot tuzağı; gerçek ziyaretçi bu alanı görmez.</summary>
        public string? WebSitesi { get; set; }
    }
}
