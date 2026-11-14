using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Tasimacilik.Ayarlar;

/// <summary>Paneldeki SMTP ayarlarıyla e-posta gönderir.</summary>
public class PostaServisi(AyarDeposu depo)
{
    /// <summary>Alıcı adresine (teklif kutusu) düz metin e-posta gönderir.</summary>
    public async Task GonderAsync(string konu, string govde, string? yanitAdresi = null, CancellationToken iptal = default)
    {
        var a = depo.Al();
        if (!a.PostaHazir)
            throw new InvalidOperationException("E-posta ayarları eksik: sunucu, gönderen ve alıcı adresi girilmeli.");

        var ileti = new MimeMessage();
        ileti.From.Add(new MailboxAddress(a.GonderenAd, a.GonderenAdres.Trim()));
        foreach (var adres in a.AliciAdres.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            ileti.To.Add(MailboxAddress.Parse(adres));
        if (!string.IsNullOrWhiteSpace(yanitAdresi) && MailboxAddress.TryParse(yanitAdresi, out var yanit))
            ileti.ReplyTo.Add(yanit);
        ileti.Subject = konu;
        ileti.Body = new TextPart("plain") { Text = govde };

        var guvenlik = Enum.TryParse<SecureSocketOptions>(a.SmtpGuvenlik, out var g) ? g : SecureSocketOptions.Auto;

        using var istemci = new SmtpClient { Timeout = 20_000 };
        await istemci.ConnectAsync(a.SmtpSunucu.Trim(), a.SmtpPort, guvenlik, iptal);
        if (!string.IsNullOrWhiteSpace(a.SmtpKullanici))
            await istemci.AuthenticateAsync(a.SmtpKullanici.Trim(), a.SmtpSifre, iptal);
        await istemci.SendAsync(new MailKit.SendRequest(ileti), iptal);
        await istemci.DisconnectAsync(true, iptal);
    }
}
