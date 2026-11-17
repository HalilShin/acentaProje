using System.ComponentModel.DataAnnotations;
using Acenta.Veri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acenta.Pages.Panel;

/// <summary>Tur ekleme ve düzenleme: bilgiler, program, fotoğraf ve kalkış tarihleri (kontenjanla).</summary>
public class TurModel(AcentaVeri db, FotografDeposu fotograflar, ILogger<TurModel> log) : PageModel
{
    [BindProperty] public TurGirdisi Girdi { get; set; } = new();
    [BindProperty] public List<TarihGirdisi> Tarihler { get; set; } = [];
    [BindProperty] public IFormFile? YeniFotograf { get; set; }
    /// <summary>Form açıldığında var olan kalkışlar. Yalnızca bunlardan formda olmayanlar silinir; başka sekmede eklenenler korunur.</summary>
    [BindProperty] public List<int> BilinenTarihler { get; set; } = [];
    [BindProperty] public bool FotografiKaldir { get; set; }

    public int? Id { get; private set; }
    public string? MevcutFotograf { get; private set; }
    public string? Adres { get; private set; }
    [TempData] public string? Basari { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken iptal)
    {
        if (id is null)
        {
            Tarihler = [new TarihGirdisi { Kontenjan = 46 }];
            return Page();
        }
        var tur = await db.Turlar.AsNoTracking().Include(t => t.Tarihler).FirstOrDefaultAsync(t => t.Id == id, iptal);
        if (tur is null) return NotFound();

        Id = tur.Id;
        MevcutFotograf = tur.Fotograf is null ? null : "/foto/" + tur.Fotograf;
        Adres = tur.Adres;
        Girdi = new TurGirdisi
        {
            Ad = tur.Ad, Kod = tur.Kod, Bolge = tur.Bolge, Turu = tur.Turu, Sure = tur.Sure, Ulasim = tur.Ulasim,
            Ozet = tur.Ozet, Bulusma = tur.Bulusma, Program = tur.Program, Dahil = tur.Dahil, Haric = tur.Haric, Notlar = tur.Notlar,
            Fiyat = tur.Fiyat, Yayinda = tur.Yayinda, OneCikan = tur.OneCikan,
        };
        // Geçmiş tarihler de listelenir ki silinebilsin; en yenisi altta.
        BilinenTarihler = tur.Tarihler.Select(t => t.Id).ToList();
        Tarihler = tur.Tarihler.OrderBy(t => t.Tarih).Select(t => new TarihGirdisi
        {
            Id = t.Id, Tarih = t.Tarih, Kontenjan = t.Kontenjan, Ayrilan = t.Ayrilan, Fiyat = t.Fiyat, Not = t.Not,
        }).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id, CancellationToken iptal)
    {
        Tur? tur = null;
        if (id is not null)
        {
            tur = await db.Turlar.Include(t => t.Tarihler).FirstOrDefaultAsync(t => t.Id == id, iptal);
            if (tur is null) return NotFound();
            Id = tur.Id;
            MevcutFotograf = tur.Fotograf is null ? null : "/foto/" + tur.Fotograf;
            Adres = tur.Adres;
        }

        // Yeni ve boş bırakılan tarih satırları yok sayılır; mevcut bir kalkışın tarihi boşaltılamaz.
        if (Tarihler.Any(t => t.Id is not null && t.Tarih is null))
            ModelState.AddModelError("Tarihler", "Mevcut bir kalkışın tarihi boş bırakılamaz. Kalkışı kaldırmak için satırdaki \"Sil\" düğmesini kullanın.");
        Tarihler = Tarihler.Where(t => t.Tarih is not null || t.Id is not null).ToList();
        if (Tarihler.Where(t => t.Tarih is not null).GroupBy(t => t.Tarih!.Value.Date).Any(g => g.Count() > 1))
            ModelState.AddModelError("Tarihler", "Aynı tarih iki kez girilmiş.");
        if (tur is not null)
        {
            // Kaydı olan bir kalkış formdan kaldırılamaz; kontenjan kayıtlı yolcunun altına inemez.
            var kayitSayilari = await db.Kayitlar.Where(k => k.TurTarihi.TurId == tur.Id)
                .GroupBy(k => k.TurTarihiId).Select(g => g.Key).ToListAsync(iptal);
            var formdaki = Tarihler.Where(t => t.Id is not null).Select(t => t.Id!.Value).ToHashSet();
            foreach (var d in tur.Tarihler.Where(d => kayitSayilari.Contains(d.Id) && BilinenTarihler.Contains(d.Id) && !formdaki.Contains(d.Id)))
                ModelState.AddModelError("Tarihler", $"{d.Tarih:dd.MM.yyyy} kalkışında kayıt var; bu tarih silinemez. Önce kayıtları silin ya da başka tarihe taşıyın.");
            foreach (var g in Tarihler.Where(t => t.Id is not null))
                if (tur.Tarihler.FirstOrDefault(d => d.Id == g.Id) is { } d && g.Kontenjan < d.Ayrilan)
                    ModelState.AddModelError("Tarihler", $"{d.Tarih:dd.MM.yyyy}: kontenjan, kayıtlı {d.Ayrilan} yolcunun altına indirilemez.");
        }

        if (!Bolgeler.Tumu.Any(b => b.Kod == Girdi.Bolge))
            ModelState.AddModelError("Girdi.Bolge", "Bölge seçin.");

        var kod = TurServisi.KodUret(string.IsNullOrWhiteSpace(Girdi.Kod) ? Girdi.Ad ?? "" : Girdi.Kod);
        if (kod.Length > 120) kod = kod[..120].TrimEnd('-');
        if (kod.Length == 0) ModelState.AddModelError("Girdi.Ad", "Tur adını yazın.");
        else if (await db.Turlar.AnyAsync(t => t.Kod == kod && t.Id != (id ?? 0), iptal))
            ModelState.AddModelError("Girdi.Kod", $"\"{kod}\" adresi başka bir turda kullanılıyor; farklı bir adres yazın.");

        string? yeniFoto = null;
        if (YeniFotograf is { Length: > 0 })
        {
            yeniFoto = await fotograflar.KaydetAsync(YeniFotograf, iptal);
            if (yeniFoto is null)
                ModelState.AddModelError(nameof(YeniFotograf), "Fotoğraf JPG, PNG ya da WEBP olmalı ve 6 MB'ı geçmemeli.");
        }

        if (!ModelState.IsValid)
        {
            if (tur is not null)
                foreach (var g in Tarihler.Where(t => t.Id is not null))
                    g.Ayrilan = tur.Tarihler.FirstOrDefault(d => d.Id == g.Id)?.Ayrilan ?? 0;
            fotograflar.Sil(yeniFoto);
            return Page();
        }

        tur ??= db.Turlar.Add(new Tur()).Entity;
        tur.Ad = Girdi.Ad!.Trim();
        tur.Kod = kod;
        tur.Bolge = Girdi.Bolge!;
        tur.Turu = Girdi.Turu;
        tur.Sure = Girdi.Sure?.Trim() ?? "";
        tur.Ulasim = Girdi.Ulasim?.Trim() ?? "";
        tur.Ozet = Girdi.Ozet?.Trim() ?? "";
        tur.Bulusma = Girdi.Bulusma?.Trim() ?? "";
        tur.Program = Girdi.Program?.Trim() ?? "";
        tur.Dahil = Girdi.Dahil?.Trim() ?? "";
        tur.Haric = Girdi.Haric?.Trim() ?? "";
        tur.Notlar = Girdi.Notlar?.Trim() ?? "";
        tur.Fiyat = Girdi.Fiyat;
        tur.Yayinda = Girdi.Yayinda;
        tur.OneCikan = Girdi.OneCikan;

        var eskiFoto = tur.Fotograf;
        if (yeniFoto is not null) tur.Fotograf = yeniFoto;
        else if (FotografiKaldir) tur.Fotograf = null;

        // Tarihler: formda olmayanlar silinir, olanlar güncellenir, yeniler eklenir.
        var formdakiler = Tarihler.Where(t => t.Id is not null).Select(t => t.Id!.Value).ToHashSet();
        tur.Tarihler.RemoveAll(t => BilinenTarihler.Contains(t.Id) && !formdakiler.Contains(t.Id));
        foreach (var g in Tarihler)
        {
            var hedef = g.Id is null ? null : tur.Tarihler.FirstOrDefault(t => t.Id == g.Id);
            if (hedef is null) tur.Tarihler.Add(hedef = new TurTarihi());
            hedef.Tarih = g.Tarih!.Value.Date;
            hedef.Kontenjan = g.Kontenjan;
            hedef.Fiyat = g.Fiyat;
            hedef.Not = string.IsNullOrWhiteSpace(g.Not) ? null : g.Not.Trim();
        }

        try
        {
            await db.SaveChangesAsync(iptal);
        }
        catch (DbUpdateException ex)
        {
            fotograflar.Sil(yeniFoto);
            log.LogWarning(ex, "Tur kaydedilemedi.");
            ModelState.AddModelError("", "Tur kaydedilemedi. Adres başka bir turda kullanılıyor olabilir; farklı bir adres deneyin.");
            return Page();
        }
        if (eskiFoto != tur.Fotograf) fotograflar.Sil(eskiFoto);
        log.LogInformation("Tur kaydedildi: {Id} {Ad}", tur.Id, tur.Ad);

        Basari = "Tur kaydedildi.";
        return Redirect($"/panel/tur/{tur.Id}");
    }

    public class TurGirdisi
    {
        [Required(ErrorMessage = "Tur adını yazın."), StringLength(160)] public string? Ad { get; set; }
        [StringLength(120)] public string? Kod { get; set; }
        public string? Bolge { get; set; }
        public TurTuru Turu { get; set; } = TurTuru.Konaklamali;
        [StringLength(60)] public string? Sure { get; set; }
        [StringLength(60)] public string? Ulasim { get; set; } = "Otobüsle";
        [StringLength(300)] public string? Ozet { get; set; }
        [StringLength(120)] public string? Bulusma { get; set; }
        [StringLength(8000)] public string? Program { get; set; }
        [StringLength(4000)] public string? Dahil { get; set; }
        [StringLength(4000)] public string? Haric { get; set; }
        [StringLength(4000)] public string? Notlar { get; set; }
        [Range(0, 1_000_000, ErrorMessage = "Fiyat geçerli değil.")] public decimal Fiyat { get; set; }
        public bool Yayinda { get; set; } = true;
        public bool OneCikan { get; set; }
    }

    public class TarihGirdisi
    {
        public int? Id { get; set; }
        [DataType(DataType.Date)] public DateTime? Tarih { get; set; }
        [Range(0, 999, ErrorMessage = "Kontenjan 0–999 olmalı.")] public int Kontenjan { get; set; }
        /// <summary>Yalnızca gösterim için; kayıtlardan hesaplanır, formdan alınmaz.</summary>
        public int Ayrilan { get; set; }
        [Range(0, 1_000_000)] public decimal? Fiyat { get; set; }
        [StringLength(120)] public string? Not { get; set; }
    }
}
