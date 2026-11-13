using System.ComponentModel.DataAnnotations;
using Locktech.Ayarlar;
using Locktech.Urunler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locktech.Pages;

public class IndexModel(PostaServisi posta, ILogger<IndexModel> log) : PageModel
{
    [BindProperty] public TeklifFormu Form { get; set; } = new();
    [TempData] public bool Gonderildi { get; set; }
    public string? Hata { get; private set; }

    /// <summary>Ürün sayfasından gelindiyse (/?urun=hb-a1sp) formda o ürün seçili gelir.</summary>
    public void OnGet(string? urun)
    {
        if (UrunKatalogu.Bul(urun) is { } secilen) Form.Urun = secilen.FormSecenegi;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken iptal)
    {
        // Gizli alan doluysa bot doldurmuştur; göndermeden başarılı gibi davran.
        if (!string.IsNullOrEmpty(Form.WebSitesi))
        {
            Gonderildi = true;
            return RedirectToPage(null, null, null, "teklif");
        }

        if (!ModelState.IsValid) return Page();

        var govde = $"""
            Web sitesinden yeni teklif talebi

            Ad Soyad : {Form.AdSoyad}
            Firma    : {Form.Firma}
            Telefon  : {Form.Telefon}
            E-posta  : {Form.Eposta}
            Ürün     : {Form.Urun}

            Mesaj:
            {Form.Mesaj}

            Gönderim: {DateTime.Now:dd.MM.yyyy HH:mm} · IP {HttpContext.Connection.RemoteIpAddress}
            """;

        try
        {
            await posta.GonderAsync($"Teklif talebi — {Form.AdSoyad}", govde, Form.Eposta, iptal);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Teklif formu gönderilemedi. Talep: {Ad} / {Telefon}", Form.AdSoyad, Form.Telefon);
            Hata = "Talebiniz şu an iletilemedi. Lütfen +90 542 432 10 10 numarasını arayın ya da info@locktech.com.tr adresine yazın.";
            return Page();
        }

        Gonderildi = true;
        return RedirectToPage(null, null, null, "teklif");
    }

    public class TeklifFormu
    {
        [Required(ErrorMessage = "Adınızı yazın."), StringLength(100)]
        public string? AdSoyad { get; set; }

        [StringLength(150)]
        public string? Firma { get; set; }

        [Required(ErrorMessage = "Telefon numaranızı yazın."), StringLength(30)]
        public string? Telefon { get; set; }

        [EmailAddress(ErrorMessage = "E-posta adresi geçerli değil."), StringLength(150)]
        public string? Eposta { get; set; }

        [StringLength(100)]
        public string? Urun { get; set; }

        [StringLength(2000)]
        public string? Mesaj { get; set; }

        /// <summary>Bot tuzağı; gerçek ziyaretçi bu alanı görmez.</summary>
        public string? WebSitesi { get; set; }
    }
}
