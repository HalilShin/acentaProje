using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Locktech;
using Locktech.Ayarlar;
using Locktech.Cozumler;
using Locktech.Urunler;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Sunucuya özel, repoya girmeyen ayarlar (panel şifresi gibi): appsettings.<Ortam>.local.json
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.local.json", optional: true, reloadOnChange: true);

var veriKlasoru = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(veriKlasoru);

// Oturum çerezi ve SMTP şifresi için anahtarlar uygulama yeniden başlasa da kalsın.
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(veriKlasoru, "anahtarlar")))
    .SetApplicationName("Locktech");

builder.Services.AddSingleton<AyarDeposu>();
builder.Services.AddScoped<PostaServisi>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/panel/giris";
        o.Cookie.Name = "locktech_panel";
        o.ExpireTimeSpan = TimeSpan.FromHours(8);
        o.SlidingExpiration = true;
    });

// Panel girişinde şifre denemelerini IP başına sınırla.
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy("giris", ctx => RateLimitPartition.GetFixedWindowLimiter(
        ctx.Connection.RemoteIpAddress?.ToString() ?? "bilinmiyor",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(1) }));
});

builder.Services.AddRazorPages(o =>
{
    var k = o.Conventions;
    k.AuthorizeFolder("/Panel");
    k.AllowAnonymousToPage("/Panel/Giris");

    // İngilizce adresler: aynı sayfalar /en altında da çalışır (bkz. Yollar).
    k.AddPageRoute("/Index", "en");
    k.AddPageRoute("/Urunler/Index", "en/products");
    k.AddPageRoute("/Urunler/Detay", "en/products/{kod}");
    k.AddPageRoute("/Urunler/Pdf", "en/products/{kod}/pdf");
    k.AddPageRoute("/Cozumler/Index", "en/solutions");
    k.AddPageRoute("/Cozumler/Detay", "en/solutions/{kod}");
    k.AddPageRoute("/Hakkimizda", "en/about");
    k.AddPageRoute("/Iletisim", "en/contact");
    k.AddPageRoute("/Kvkk", "en/privacy");
    k.AddPageRoute("/CerezPolitikasi", "en/cookies");
});

var app = builder.Build();

// Dil: adres /en ile başlıyorsa İngilizce, değilse Türkçe.
var turkce = new CultureInfo("tr-TR");
var ingilizce = new CultureInfo("en-US");
app.Use(async (ctx, next) =>
{
    var yol = ctx.Request.Path.Value ?? "";
    var en = yol.Equals("/en", StringComparison.OrdinalIgnoreCase) || yol.StartsWith("/en/", StringComparison.OrdinalIgnoreCase);
    CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = en ? ingilizce : turkce;
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/hata/500");
}
app.UseStatusCodePagesWithReExecute("/hata/{0}");

app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapGet("/robots.txt", () => Results.Text(
    $"User-agent: *\nDisallow: /panel\n\nSitemap: {Site.Adres}/sitemap.xml\n", "text/plain", Encoding.UTF8));

app.MapGet("/sitemap.xml", () =>
{
    var sayfalar = new List<(string Tr, string En)>
    {
        ("/", "/en"), ("/urunler", "/en/products"), ("/cozumler", "/en/solutions"),
        ("/hakkimizda", "/en/about"), ("/iletisim", "/en/contact"),
        ("/kvkk", "/en/privacy"), ("/cerez-politikasi", "/en/cookies"),
    };
    sayfalar.AddRange(UrunKatalogu.Tumu.Select(u => ($"/urunler/{u.Kod}", $"/en/products/{u.Kod}")));
    sayfalar.AddRange(CozumKatalogu.Tumu.Select(c => ($"/cozumler/{c.KodTr}", $"/en/solutions/{c.KodEn}")));

    var xml = new StringBuilder("""<?xml version="1.0" encoding="UTF-8"?>""" + "\n");
    xml.Append("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" xmlns:xhtml="http://www.w3.org/1999/xhtml">""" + "\n");
    foreach (var (tr, en) in sayfalar)
    {
        foreach (var yol in new[] { tr, en })
        {
            xml.Append($"  <url><loc>{Site.Adres}{yol}</loc>");
            xml.Append($"""<xhtml:link rel="alternate" hreflang="tr" href="{Site.Adres}{tr}"/>""");
            xml.Append($"""<xhtml:link rel="alternate" hreflang="en" href="{Site.Adres}{en}"/>""");
            xml.Append("</url>\n");
        }
    }
    xml.Append("</urlset>\n");
    return Results.Content(xml.ToString(), "application/xml", Encoding.UTF8);
});

app.Run();
