using System.Text.RegularExpressions;

namespace Acenta.Veri;

/// <summary>Panelden yüklenen tur fotoğraflarını App_Data/fotograflar klasöründe saklar.</summary>
public partial class FotografDeposu(IWebHostEnvironment ortam)
{
    public const long EnBuyukBoyut = 6 * 1024 * 1024;

    private string Klasor => Path.Combine(ortam.ContentRootPath, "App_Data", "fotograflar");

    [GeneratedRegex("^[a-f0-9]{32}\\.(jpg|png|webp)$")]
    private static partial Regex AdDeseni();

    public static bool GecerliAd(string ad) => AdDeseni().IsMatch(ad);

    public static string Tur(string ad) => Path.GetExtension(ad) switch
    {
        ".png" => "image/png",
        ".webp" => "image/webp",
        _ => "image/jpeg",
    };

    /// <summary>Dosyayı içeriğine bakarak doğrular ve kaydeder; yeni dosya adını döner. Geçersizse null.</summary>
    public async Task<string?> KaydetAsync(IFormFile dosya, CancellationToken iptal)
    {
        if (dosya.Length is 0 or > EnBuyukBoyut) return null;

        var bas = new byte[12];
        await using (var okuyucu = dosya.OpenReadStream())
            if (await okuyucu.ReadAtLeastAsync(bas, bas.Length, false, iptal) < bas.Length) return null;

        var uzanti =
            bas[0] == 0xFF && bas[1] == 0xD8 && bas[2] == 0xFF ? "jpg" :
            bas[0] == 0x89 && bas[1] == 0x50 && bas[2] == 0x4E && bas[3] == 0x47 ? "png" :
            bas[0] == 'R' && bas[1] == 'I' && bas[2] == 'F' && bas[3] == 'F' && bas[8] == 'W' && bas[9] == 'E' && bas[10] == 'B' && bas[11] == 'P' ? "webp" :
            null;
        if (uzanti is null) return null;

        Directory.CreateDirectory(Klasor);
        var ad = $"{Guid.NewGuid():N}.{uzanti}";
        await using var hedef = File.Create(Path.Combine(Klasor, ad));
        await using var kaynak = dosya.OpenReadStream();
        await kaynak.CopyToAsync(hedef, iptal);
        return ad;
    }

    /// <summary>Fotoğrafın kopyasını yeni bir adla oluşturur (kopyalanan tur kendi dosyasını silebilsin diye).</summary>
    public string? Kopyala(string? ad)
    {
        if (string.IsNullOrEmpty(ad) || !GecerliAd(ad)) return null;
        var kaynak = Path.Combine(Klasor, ad);
        if (!File.Exists(kaynak)) return null;
        var yeni = $"{Guid.NewGuid():N}{Path.GetExtension(ad)}";
        File.Copy(kaynak, Path.Combine(Klasor, yeni));
        return yeni;
    }

    public void Sil(string? ad)
    {
        if (string.IsNullOrEmpty(ad) || !GecerliAd(ad)) return;
        var yol = Path.Combine(Klasor, ad);
        if (File.Exists(yol)) File.Delete(yol);
    }
}
