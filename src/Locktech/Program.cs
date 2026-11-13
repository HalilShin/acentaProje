using System.Threading.RateLimiting;
using Locktech.Ayarlar;
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
    o.Conventions.AuthorizeFolder("/Panel");
    o.Conventions.AllowAnonymousToPage("/Panel/Giris");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
