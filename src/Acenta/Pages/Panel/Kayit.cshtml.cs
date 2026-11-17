using System.ComponentModel.DataAnnotations;
using Acenta.Veri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages.Panel;

/// <summary>Bir kalkışa rezervasyon ekleme ve düzenleme. Her yolcu ayrı satırdır; isim girilmeyen koltuklar da sayılır.</summary>
public class KayitModel(AcentaVeri db) : PageModel
{
    [BindProperty] public KayitGirdisi Girdi { get; set; } = new();
    [BindProperty] public List<YolcuGirdisi> Yolcular { get; set; } = [];

    public int? Id { get; private set; }
    public TurTarihi Kalkis { get; private set; } = null!;
    /// <summary>Başka tarihe taşımak için aynı turun diğer kalkışları.</summary>
    public List<TurTarihi> DigerKalkislar { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int? id, int? kalkis, CancellationToken iptal)
    {
        if (id is not null)
        {
            var k = await db.Kayitlar.AsNoTracking().Include(x => x.Yolcular).FirstOrDefaultAsync(x => x.Id == id, iptal);
            if (k is null) return NotFound();
            Id = k.Id;
            kalkis = k.TurTarihiId;
            Girdi = new KayitGirdisi
            {
                IletisimAd = k.IletisimAd, Telefon = k.Telefon, BinisYeri = k.BinisYeri, Tutar = k.Tutar, Alinan = k.Alinan,
                Not = k.Not, Iptal = k.Iptal, KalkisId = k.TurTarihiId,
            };
            Yolcular = k.Yolcular.OrderBy(y => y.Sira).Select(y => new YolcuGirdisi { Id = y.Id, AdSoyad = y.AdSoyad, KimlikNo = y.KimlikNo, Bindi = y.Bindi }).ToList();
        }
        if (!await KalkisYukleAsync(kalkis, iptal)) return NotFound();
        if (id is null)
        {
            Girdi.KalkisId = Kalkis.Id;
            Girdi.KisiBasiFiyat = Kalkis.Fiyat ?? Kalkis.Tur.Fiyat;
            Yolcular = [new YolcuGirdisi()];
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id, CancellationToken iptal)
    {
        Kayit? kayit = null;
        if (id is not null)
        {
            kayit = await db.Kayitlar.Include(x => x.Yolcular).FirstOrDefaultAsync(x => x.Id == id, iptal);
            if (kayit is null) return NotFound();
            Id = kayit.Id;
        }
        if (!await KalkisYukleAsync(Girdi.KalkisId, iptal)) return NotFound();

        // Boş satırlar atılır; hiç isim yoksa iletişim kişisi ilk yolcu sayılır.
        Yolcular = Yolcular.Where(y => !string.IsNullOrWhiteSpace(y.AdSoyad) || !string.IsNullOrWhiteSpace(y.KimlikNo)).ToList();
        var kisi = Math.Max(Girdi.KisiSayisi ?? 0, Yolcular.Count);
        if (kisi == 0) kisi = 1;
        if (Yolcular.Count == 0 && !string.IsNullOrWhiteSpace(Girdi.IletisimAd))
            Yolcular.Add(new YolcuGirdisi { AdSoyad = Girdi.IletisimAd });

        if (string.IsNullOrWhiteSpace(Girdi.IletisimAd)) ModelState.AddModelError("Girdi.IletisimAd", "İletişim kişisinin adını yazın.");
        if (string.IsNullOrWhiteSpace(Girdi.Telefon)) ModelState.AddModelError("Girdi.Telefon", "Telefon yazın.");
        if (Girdi.Alinan > Girdi.Tutar && Girdi.Tutar > 0) ModelState.AddModelError("Girdi.Alinan", "Alınan ödeme toplam ücretten fazla.");

        // Kontenjan: bu kayıt dışındaki dolu koltuk + bu kaydın koltukları.
        if (!Girdi.Iptal)
        {
            var digerleri = await db.Yolcular.CountAsync(y => y.Kayit.TurTarihiId == Kalkis.Id && !y.Kayit.Iptal && y.KayitId != (id ?? 0), iptal);
            if (digerleri + kisi > Kalkis.Kontenjan)
                ModelState.AddModelError("", $"Kontenjan yetmiyor: {Kalkis.Kontenjan} koltuğun {digerleri} tanesi dolu, bu kayıt {kisi} kişi. Kontenjanı tur sayfasından artırabilirsiniz.");
        }
        if (!ModelState.IsValid) return Page();

        var eskiKalkis = kayit?.TurTarihiId;
        kayit ??= db.Kayitlar.Add(new Kayit()).Entity;
        kayit.TurTarihiId = Kalkis.Id;
        kayit.IletisimAd = Girdi.IletisimAd!.Trim();
        kayit.Telefon = Girdi.Telefon!.Trim();
        kayit.BinisYeri = Bos(Girdi.BinisYeri);
        kayit.Tutar = Girdi.Tutar ?? (Girdi.KisiBasiFiyat ?? 0) * kisi;
        kayit.Alinan = Girdi.Alinan ?? 0;
        kayit.Not = Bos(Girdi.Not);
        kayit.Iptal = Girdi.Iptal;

        // Yolcular: isimli satırlar + isim verilmemiş koltuklar için "İletişim kişisi +n" satırları.
        var hedefler = Yolcular.Select(y => (y.Id, Ad: y.AdSoyad?.Trim() ?? "", Kimlik: Bos(y.KimlikNo), y.Bindi)).ToList();
        for (var n = hedefler.Count + 1; n <= kisi; n++) hedefler.Add((null, $"{kayit.IletisimAd} +{n - 1}", null, (bool?)null));
        var tutulacak = hedefler.Where(h => h.Id is not null).Select(h => h.Id!.Value).ToHashSet();
        kayit.Yolcular.RemoveAll(y => !tutulacak.Contains(y.Id));
        for (var i = 0; i < hedefler.Count; i++)
        {
            var h = hedefler[i];
            var y = h.Id is null ? null : kayit.Yolcular.FirstOrDefault(x => x.Id == h.Id);
            if (y is null) kayit.Yolcular.Add(y = new Yolcu());
            y.AdSoyad = h.Ad.Length > 100 ? h.Ad[..100] : h.Ad;
            y.KimlikNo = h.Kimlik;
            y.Bindi = h.Bindi;
            y.Sira = i;
        }

        await db.SaveChangesAsync(iptal);
        await db.AyrilaniGuncelleAsync(new[] { kayit.TurTarihiId, eskiKalkis ?? kayit.TurTarihiId }, iptal);

        TempData["Basari"] = $"{kayit.IletisimAd} kaydı kaydedildi ({hedefler.Count} kişi).";
        return Redirect($"/panel/kalkis/{kayit.TurTarihiId}");
    }

    public async Task<IActionResult> OnPostSilAsync(int id, CancellationToken iptal)
    {
        var kayit = await db.Kayitlar.FirstOrDefaultAsync(x => x.Id == id, iptal);
        if (kayit is null) return NotFound();
        db.Kayitlar.Remove(kayit);
        await db.SaveChangesAsync(iptal);
        await db.AyrilaniGuncelleAsync(new[] { kayit.TurTarihiId }, iptal);
        TempData["Basari"] = $"{kayit.IletisimAd} kaydı silindi.";
        return Redirect($"/panel/kalkis/{kayit.TurTarihiId}");
    }

    private async Task<bool> KalkisYukleAsync(int? kalkisId, CancellationToken iptal)
    {
        var k = await db.TurTarihleri.AsNoTracking().Include(t => t.Tur).FirstOrDefaultAsync(t => t.Id == kalkisId, iptal);
        if (k is null) return false;
        Kalkis = k;
        DigerKalkislar = await db.TurTarihleri.AsNoTracking().Where(t => t.TurId == k.TurId).OrderBy(t => t.Tarih).ToListAsync(iptal);
        return true;
    }

    private static string? Bos(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    public class KayitGirdisi
    {
        public int KalkisId { get; set; }
        [StringLength(100)] public string? IletisimAd { get; set; }
        [StringLength(30)] public string? Telefon { get; set; }
        [StringLength(120)] public string? BinisYeri { get; set; }
        /// <summary>İsim girilmeden ayrılacak koltuk sayısı; yolcu satırı daha fazlaysa o sayılır.</summary>
        [Range(1, 999, ErrorMessage = "Kişi sayısı 1–999 olmalı.")] public int? KisiSayisi { get; set; }
        /// <summary>Toplam ücret boşsa kişi başı fiyat × kişi sayısı yazılır.</summary>
        [Range(0, 10_000_000)] public decimal? Tutar { get; set; }
        public decimal? KisiBasiFiyat { get; set; }
        [Range(0, 10_000_000)] public decimal? Alinan { get; set; }
        [StringLength(500)] public string? Not { get; set; }
        public bool Iptal { get; set; }
    }

    public class YolcuGirdisi
    {
        public int? Id { get; set; }
        [StringLength(100)] public string? AdSoyad { get; set; }
        [StringLength(20)] public string? KimlikNo { get; set; }
        public bool? Bindi { get; set; }
    }
}
