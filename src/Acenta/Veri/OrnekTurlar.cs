using Microsoft.EntityFrameworkCore;

namespace Acenta.Veri;

/// <summary>
/// Yalnızca geliştirme ortamında, veritabanı boşsa eklenen örnek turlar.
/// Canlı sitede turlar panelden girilir; bu veriler oraya gitmez.
/// </summary>
public static class OrnekTurlar
{
    public static async Task EkleAsync(AcentaVeri db)
    {
        if (await db.Turlar.AnyAsync()) return;
        var b = DateTime.Today;
        DateTime Cumartesi(int hafta) => b.AddDays(((int)DayOfWeek.Saturday - (int)b.DayOfWeek + 7) % 7 + 7 * hafta);

        db.Turlar.AddRange(
            new Tur
            {
                Kod = "konaklamali-kapadokya-ve-ihlara-vadisi-turu", Ad = "Konaklamalı Kapadokya ve Ihlara Vadisi Turu", Bolge = "kapadokya",
                Turu = TurTuru.Konaklamali, Sure = "1 gece 2 gün", Ulasim = "Otobüsle", Fiyat = 4950, OneCikan = true,
                Ozet = "Göreme, Uçhisar, Paşabağları ve Ihlara Vadisi'ni rehber eşliğinde iki günde gezin.",
                Program = "1. Gün: Ankara'dan hareket, Ihlara Vadisi yürüyüşü, Selime Katedrali, otele yerleşme\n2. Gün: Göreme Açık Hava Müzesi, Uçhisar Kalesi, Paşabağları, Avanos, Ankara'ya dönüş",
                Dahil = "Otobüsle ulaşım\n1 gece otel konaklaması (kahvaltı dahil)\nProfesyonel rehberlik\nSeyahat sigortası",
                Haric = "Müze giriş ücretleri\nÖğle ve akşam yemekleri\nBalon turu",
                Bulusma = "Kızılay Güvenpark önü, 07.00",
                Notlar = "Programdaki saatler yaklaşıktır. Balon turu isteğe bağlıdır.",
                Tarihler = [new() { Tarih = Cumartesi(0), Kontenjan = 46, Ayrilan = 40 }, new() { Tarih = Cumartesi(2), Kontenjan = 46, Ayrilan = 12 }, new() { Tarih = Cumartesi(4), Kontenjan = 46 }],
            },
            new Tur
            {
                Kod = "gunubirlik-safranbolu-ve-yoruk-koyu-turu", Ad = "Günübirlik Safranbolu ve Yörük Köyü Turu", Bolge = "bati-karadeniz",
                Turu = TurTuru.Gunubirlik, Sure = "Günübirlik", Ulasim = "Otobüsle", Fiyat = 1450,
                Ozet = "Osmanlı konaklarıyla ünlü Safranbolu çarşısı, Cinci Hanı ve Yörük Köyü.",
                Program = "07.00 Ankara'dan hareket\n10.30 Yörük Köyü\n12.30 Safranbolu çarşısı ve serbest zaman\n17.00 Ankara'ya dönüş",
                Dahil = "Otobüsle ulaşım\nRehberlik\nÖğle yemeği",
                Haric = "Müze ve ören yeri girişleri\nKişisel harcamalar",
                Tarihler = [new() { Tarih = Cumartesi(0).AddDays(1), Kontenjan = 46, Ayrilan = 20 }, new() { Tarih = Cumartesi(1).AddDays(1), Kontenjan = 46 }, new() { Tarih = Cumartesi(2).AddDays(1), Kontenjan = 46 }],
            },
            new Tur
            {
                Kod = "gobeklitepe-sanliurfa-ve-mardin-turu", Ad = "Göbeklitepe, Şanlıurfa ve Mardin Turu", Bolge = "gap",
                Turu = TurTuru.Konaklamali, Sure = "3 gece 4 gün", Ulasim = "Otobüsle", Fiyat = 12900, OneCikan = true,
                Ozet = "Göbeklitepe, Balıklıgöl, Harran, Mardin ve Midyat'ı kapsayan Güneydoğu turu.",
                Program = "1. Gün: Ankara'dan hareket, gece yolculuğu\n2. Gün: Göbeklitepe, Balıklıgöl, Harran\n3. Gün: Mardin, Deyrulzafaran Manastırı, Midyat\n4. Gün: Dara Antik Kenti, Ankara'ya dönüş",
                Dahil = "Otobüsle ulaşım\n2 gece otel konaklaması (kahvaltı dahil)\nProfesyonel rehberlik",
                Haric = "Müze girişleri\nÖğle ve akşam yemekleri",
                Tarihler = [new() { Tarih = Cumartesi(1).AddDays(-2), Kontenjan = 46, Ayrilan = 42 }, new() { Tarih = Cumartesi(3).AddDays(-2), Kontenjan = 46 }],
            },
            new Tur
            {
                Kod = "karadeniz-yaylalar-turu", Ad = "Karadeniz Yaylalar Turu: Uzungöl, Ayder, Sümela", Bolge = "karadeniz",
                Turu = TurTuru.Konaklamali, Sure = "3 gece 4 gün", Ulasim = "Otobüsle", Fiyat = 11500,
                Ozet = "Uzungöl, Ayder Yaylası, Sümela Manastırı ve Rize çay bahçeleri.",
                Program = "1. Gün: Ankara'dan hareket\n2. Gün: Sümela Manastırı, Trabzon\n3. Gün: Uzungöl, Rize\n4. Gün: Ayder Yaylası, dönüş",
                Dahil = "Otobüsle ulaşım\n2 gece otel konaklaması\nRehberlik",
                Haric = "Müze girişleri\nYemekler",
                Tarihler = [new() { Tarih = Cumartesi(5), Kontenjan = 46 }],
            });
        await db.SaveChangesAsync();

        // Doluluk örnek kayıtlardan gelsin: her dolu koltuk için 4'erli örnek rezervasyonlar.
        var rastgele = new Random(7);
        var tarihler = await db.TurTarihleri.Include(t => t.Tur).Where(t => t.Ayrilan > 0).ToListAsync();
        foreach (var d in tarihler)
        {
            var kalan = d.Ayrilan;
            for (var n = 1; kalan > 0; n++)
            {
                var kisi = Math.Min(kalan, rastgele.Next(1, 5));
                kalan -= kisi;
                var fiyat = d.Fiyat ?? d.Tur.Fiyat;
                var odeme = rastgele.Next(3);
                db.Kayitlar.Add(new Kayit
                {
                    TurTarihiId = d.Id, IletisimAd = $"Örnek Rezervasyon {n}", Telefon = "0500 000 00 00", BinisYeri = n % 2 == 0 ? "Batıkent" : "Kızılay",
                    Tutar = fiyat * kisi, Alinan = odeme switch { 0 => 0, 1 => 1000, _ => fiyat * kisi },
                    Yolcular = Enumerable.Range(1, kisi).Select(i => new Yolcu { AdSoyad = $"Örnek Yolcu {n}-{i}", Sira = i - 1 }).ToList(),
                });
            }
        }
        await db.SaveChangesAsync();
    }
}
