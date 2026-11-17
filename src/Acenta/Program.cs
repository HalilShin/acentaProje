using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Acenta.Ayarlar;
using Acenta.Veri;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Sunucuya özel, repoya girmeyen ayarlar (panel şifresi, veritabanı bağlantısı): appsettings.<Ortam>.local.json
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.local.json", optional: true, reloadOnChange: true);

var veriKlasoru = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(Path.Combine(veriKlasoru, "fotograflar"));

var koruma = builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(veriKlasoru, "anahtarlar")))
    .SetApplicationName("Acenta");
// Anahtarlar diskte Windows DPAPI ile şifrelenir (sunucu Windows).
if (OperatingSystem.IsWindows()) koruma.ProtectKeysWithDpapi();

var baglanti = builder.Configuration.GetConnectionString("Acenta")
    ?? throw new InvalidOperationException("ConnectionStrings:Acenta tanımlı değil (appsettings.<Ortam>.local.json).");
builder.Services.AddDbContext<AcentaVeri>(o => o.UseSqlServer(baglanti));

builder.Services.AddSingleton<AyarDeposu>();
builder.Services.AddScoped<PostaServisi>();
builder.Services.AddScoped<TurServisi>();
builder.Services.AddSingleton<FotografDeposu>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/panel/giris";
        o.Cookie.Name = "acenta_panel";
        o.ExpireTimeSpan = TimeSpan.FromHours(8);
        o.SlidingExpiration = true;
    });

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy("giris", ctx => RateLimitPartition.GetFixedWindowLimiter(
        ctx.Connection.RemoteIpAddress?.ToString() ?? "bilinmiyor",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(1) }));
});

builder.Services.AddRazorPages(o =>
{
    o.Conventions.AuthorizeFolder("/Panel");
    o.Conventions.AllowAnonymousToPage("/Panel/Giris");
});

// PDF dökümleri (yolcu listesi, rapor) için QuestPDF; yıllık cirosu 1 milyon doların altındaki işletmeler için ücretsiz Community lisansı.
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var app = builder.Build();

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("tr-TR", useUserOverride: false);

// Veritabanı şemasını güncelle; geliştirmede boşsa örnek turları ekle.
using (var kapsam = app.Services.CreateScope())
{
    var db = kapsam.ServiceProvider.GetRequiredService<AcentaVeri>();
    db.Database.Migrate();
    if (app.Environment.IsDevelopment()) await OrnekTurlar.EkleAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/hata/500");

    // www'li adres ve ek alan adları (.com.tr, kulturgezi.com.tr) ana adrese kalıcı olarak yönlenir; yol korunur.
    app.Use(async (ctx, next) =>
    {
        var host = ctx.Request.Host.Host.ToLowerInvariant();
        var yalin = host.StartsWith("www.") ? host[4..] : host;
        if (host != SiteAyarlari.AnaAlanAdi && (yalin == SiteAyarlari.AnaAlanAdi || SiteAyarlari.EkAlanAdlari.Contains(yalin)))
        {
            ctx.Response.Redirect($"{ctx.Request.Scheme}://{SiteAyarlari.AnaAlanAdi}{ctx.Request.PathBase}{ctx.Request.Path}{ctx.Request.QueryString}", permanent: true);
            return;
        }
        await next();
    });

    app.UseHttpsRedirection();
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/hata/{0}");

app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// Panelden yüklenen tur fotoğrafları (App_Data/fotograflar). Yalnızca üretilmiş dosya adları kabul edilir.
app.MapGet("/foto/{ad}", (string ad, IWebHostEnvironment ortam) =>
{
    if (!FotografDeposu.GecerliAd(ad)) return Results.NotFound();
    var yol = Path.Combine(ortam.ContentRootPath, "App_Data", "fotograflar", ad);
    return File.Exists(yol)
        ? Results.File(yol, FotografDeposu.Tur(ad), enableRangeProcessing: false, lastModified: File.GetLastWriteTimeUtc(yol))
        : Results.NotFound();
});

app.MapGet("/robots.txt", () => Results.Text(
    $"User-agent: *\nDisallow: /panel\n\nSitemap: {SiteAyarlari.SiteAdresi}/sitemap.xml\n", "text/plain", Encoding.UTF8));

app.MapGet("/sitemap.xml", async (AcentaVeri db) =>
{
    var yollar = new List<string> { "/", "/turlar", "/tur-takvimi", "/hakkimizda", "/iletisim", "/kvkk", "/cerez-politikasi" };
    yollar.AddRange(await db.Turlar.Where(t => t.Yayinda).Select(t => "/turlar/" + t.Kod).ToListAsync());

    var xml = new StringBuilder("""<?xml version="1.0" encoding="UTF-8"?>""" + "\n");
    xml.Append("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""" + "\n");
    foreach (var yol in yollar) xml.Append($"  <url><loc>{SiteAyarlari.SiteAdresi}{yol}</loc></url>\n");
    xml.Append("</urlset>\n");
    return Results.Content(xml.ToString(), "application/xml", Encoding.UTF8);
});

app.Run();
