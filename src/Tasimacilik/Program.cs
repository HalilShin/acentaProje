using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Tasimacilik.Ayarlar;
using Tasimacilik.Markalar;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Sunucuya özel, repoya girmeyen ayarlar (panel şifresi gibi): appsettings.<Ortam>.local.json
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.local.json", optional: true, reloadOnChange: true);

var veriKlasoru = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(veriKlasoru);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(veriKlasoru, "anahtarlar")))
    .SetApplicationName("Tasimacilik");

builder.Services.AddSingleton<AyarDeposu>();
builder.Services.AddScoped<PostaServisi>();
builder.Services.AddScoped<MarkaBaglami>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/panel/giris";
        o.Cookie.Name = "tasimacilik_panel";
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

var app = builder.Build();

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("tr-TR");

// Marka seçimi: isteğin geldiği alan adına göre.
// Geliştirme ortamında alan adı localhost olduğu için ?marka=selcuklu|otobuskirala|transfer
// ile marka seçilir ve seçim bir çerezde hatırlanır.
app.Use(async (ctx, next) =>
{
    var marka = MarkaKatalogu.AlanAdinaGore(ctx.Request.Host.Host);
    if (marka is null && app.Environment.IsDevelopment())
    {
        if (MarkaKatalogu.KodaGore(ctx.Request.Query["marka"]) is { } secilen)
        {
            marka = secilen;
            ctx.Response.Cookies.Append("marka", secilen.Kod);
        }
        else
        {
            marka = MarkaKatalogu.KodaGore(ctx.Request.Cookies["marka"]);
        }
    }
    ctx.RequestServices.GetRequiredService<MarkaBaglami>().Marka = marka ?? MarkaKatalogu.Tumu[0];
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

app.MapGet("/robots.txt", (MarkaBaglami b) => Results.Text(
    $"User-agent: *\nDisallow: /panel\n\nSitemap: {b.Marka.SiteAdresi}/sitemap.xml\n", "text/plain", Encoding.UTF8));

app.MapGet("/sitemap.xml", (MarkaBaglami b) =>
{
    var m = b.Marka;
    var yollar = new List<string> { "/", "/hizmetler", "/arac-filosu", "/hakkimizda", "/iletisim", "/kvkk", "/cerez-politikasi" };
    yollar.AddRange(m.Hizmetler.Select(h => h.Adres));

    var xml = new StringBuilder("""<?xml version="1.0" encoding="UTF-8"?>""" + "\n");
    xml.Append("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""" + "\n");
    foreach (var yol in yollar) xml.Append($"  <url><loc>{m.SiteAdresi}{yol}</loc></url>\n");
    xml.Append("</urlset>\n");
    return Results.Content(xml.ToString(), "application/xml", Encoding.UTF8);
});

// Marka simgesi (favicon): her markanın kendi sembolü.
app.MapGet("/simge.svg", (MarkaBaglami b) =>
{
    var m = b.Marka;
    var svg = $"""<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 48 48" style="--ana:{m.RenkAna};--koyu:{m.RenkKoyu};--vurgu:{m.RenkVurgu}">{m.Sembol}</svg>""";
    return Results.Content(svg, "image/svg+xml", Encoding.UTF8);
});

app.Run();
