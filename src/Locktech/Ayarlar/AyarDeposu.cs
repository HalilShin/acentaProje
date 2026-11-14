using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

namespace Locktech.Ayarlar;

/// <summary>Site ayarlarını App_Data/ayarlar.json dosyasından okur ve oraya yazar.</summary>
public class AyarDeposu
{
    private static readonly JsonSerializerOptions JsonSecenekleri = new() { WriteIndented = true };

    private readonly string _dosya;
    private readonly IDataProtector _koruyucu;
    private readonly ILogger<AyarDeposu> _log;
    private readonly object _kilit = new();
    private SiteAyarlari _ayarlar;

    /// <summary>Sitede görünen e-posta; Site.Eposta buradan okur (depo henüz kurulmadıysa varsayılan).</summary>
    public static string GorunenEposta { get; private set; } = new SiteAyarlari().Eposta;

    public AyarDeposu(IWebHostEnvironment ortam, IDataProtectionProvider koruma, ILogger<AyarDeposu> log)
    {
        _dosya = Path.Combine(ortam.ContentRootPath, "App_Data", "ayarlar.json");
        _koruyucu = koruma.CreateProtector("Locktech.Ayarlar.SmtpSifre");
        _log = log;
        _ayarlar = Oku();
        GorunenEposta = _ayarlar.Eposta;
    }

    /// <summary>Geçerli ayarların bir kopyası.</summary>
    public SiteAyarlari Al()
    {
        lock (_kilit) return _ayarlar with { };
    }

    public void Kaydet(SiteAyarlari yeni)
    {
        lock (_kilit)
        {
            var dosyaya = yeni with
            {
                SmtpSifre = string.IsNullOrEmpty(yeni.SmtpSifre) ? "" : _koruyucu.Protect(yeni.SmtpSifre)
            };
            Directory.CreateDirectory(Path.GetDirectoryName(_dosya)!);
            var gecici = _dosya + ".tmp";
            File.WriteAllText(gecici, JsonSerializer.Serialize(dosyaya, JsonSecenekleri));
            File.Move(gecici, _dosya, overwrite: true);
            _ayarlar = yeni with { };
            GorunenEposta = yeni.Eposta;
        }
    }

    private SiteAyarlari Oku()
    {
        if (!File.Exists(_dosya)) return new SiteAyarlari();
        try
        {
            var a = JsonSerializer.Deserialize<SiteAyarlari>(File.ReadAllText(_dosya)) ?? new SiteAyarlari();
            if (!string.IsNullOrEmpty(a.SmtpSifre))
            {
                try { a.SmtpSifre = _koruyucu.Unprotect(a.SmtpSifre); }
                catch (CryptographicException)
                {
                    _log.LogWarning("SMTP şifresi çözülemedi (anahtar değişmiş olabilir); panelden yeniden girilmeli.");
                    a.SmtpSifre = "";
                }
            }
            return a;
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "ayarlar.json okunamadı; varsayılan ayarlar kullanılıyor.");
            return new SiteAyarlari();
        }
    }
}
