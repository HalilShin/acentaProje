namespace Tasimacilik;

/// <summary>İç sayfaların başlık şeridi: başlık, kısa metin ve sayfa yolu (adresi null olan adım geçerli sayfadır).</summary>
public record SayfaBasi(string Baslik, string? Metin, params (string Ad, string? Adres)[] Yol);