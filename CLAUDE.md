# acentaProje — 5 kurumsal site

Bu repo, Halil'in tek bir Windows VPS'te yayınlayacağı 5 sitenin kodunu tutar.
Ayrıntılı plan: `docs/PLAN.md`. Plan dokümanının canlı sürümü:
https://claude.ai/artifact/Gouz19yn7Tp6hGYV6JDgbs

## Mevcut durum ve sıra

1. **Locktech (locktech.com.tr)** — ŞU AN BUNUNLA BAŞLANIYOR. Önce bitirilecek.
2. Taşımacılık (3 marka, tek uygulama)
3. Acenta (Ankara Kültür Gezi)

Locktech'te ilk adım: ana sayfanın görsel tasarım taslağını hazırlayıp Halil'e onaylatmak, sonra koda dökmek.

## Teknik kararlar

- **Barındırma:** Tek Windows VPS (Windows Server 2022, IIS). Vercel kullanılmıyor.
- **Teknoloji:** ASP.NET Core (Razor Pages veya MVC), .NET 8 LTS. Hepsi C#.
- **Veritabanı:** SQL Server Express, yalnızca gereken yerde (acenta turları). Locktech ve taşımacılık statik içerik + iletişim formu.
- **SSL:** win-acme (Let's Encrypt). **Yönlendirmeler:** IIS URL Rewrite, ayrı bir "Redirects" sitesi.
- **Yayın:** GitHub Actions → VPS (Web Deploy). VPS henüz alınmadı.
- **Domainler:** İsimtescil'de kayıtlı, DNS oradan girilecek.

## Siteler

| Site | Ana domain | Yönlenecek domainler |
| --- | --- | --- |
| Locktech | locktech.com.tr | — |
| Selçuklu Turizm | selcukluturizm.com | selcukluturizm.com.tr |
| Ankara Otobüs Kirala | ankaraotobuskirala.com | ankaraotobuskirala.com.tr, ankaraotobuskiralama.com.tr |
| Ankara Turizm Transfer | ankaraturizmtransfer.com | ankaraturizmtransfer.com.tr |
| Ankara Kültür Gezi (acenta) | ankarakulturgezi.com | ankarakulturgezi.com.tr, kulturgezi.com.tr |

El yazısı domain listesi: `docs/notlar/domain-listesi-el-yazisi.jpg`

## Locktech

- Akıllı kilit / araç telematiği tanıtım ve teklif sitesi. **Türkçe + İngilizce** (`/en/`, hreflang). İngilizce çeviri tamamen Claude'da.
- **Sadece 4 ürün:** HB-A1SP (4G güneş enerjili kilit), HB-A1M (4G patlamaya dayanıklı kilit), HB-A1Ms (ikincil kilit / sub-lock), HB-VT08 (video telematik). Katalogdaki diğer ürünler SATILMIYOR, siteye konmaz.
- Teknik veriler: `docs/locktech/HBOIOT-katalog.pdf`. Türkçe föyler: `docs/locktech/foyler/`. Görseller, logo içeren banner'lar ve kartvizitler: `docs/locktech/gorseller/`.
- Sitede yalnızca Locktech markası kullanılır; üretici HBOIOT'un adı ve iletişim bilgisi sitede yer almaz.
- Marka: koyu lacivert/elektrik mavisi zemin, turuncu vurgu (kartvizit ve banner'larla uyumlu). Logo mevcut (banner/kartvizitte); temiz vektör logo yoksa Halil'den istenir.
- Marka sahibi şirket (KVKK'da veri sorumlusu): SELÇUKLU İMALAT PAZARLAMA MOBİLYA İNŞAAT İTHALAT İHRACAT SAN · İvedik VD 7601097707 · MERSİS 0760109770700001. KVKK metninin biçimi için örnek: selcuklusoba.com/kvkk.
- İletişim (kartvizitten): İvedik OSB 1362 Cadde No:34, Yenimahalle / ANKARA · +90 542 432 10 10 · info@locktech.com.tr · Satış: Serkan Gündoğdu.
- Sayfalar: Ana sayfa, Ürünler (her ürüne ayrı sayfa + PDF föy), Çözümler (konteyner, tanker, lojistik aracı, kasa, gümrük kargo takibi), Hakkımızda, İletişim / Teklif İste.

## Taşımacılık (sonra)

- Üç marka aynı firmanın yan markaları. Tek ASP.NET Core uygulaması, gelen Host'a göre marka (logo, ad, renk, metin) seçer.
- Telefon/WhatsApp/adres ortak. Her markanın metinleri FARKLI yazılmalı (kopya içerik riski). Tek Google İşletme profili.
- Logolar ve tasarım Claude'da.

## Acenta (sonra)

- Acenta unvanı: Anadolu Selçuklu Organizasyon Turizm · TÜRSAB belge no: 15385 · Telefon ve WhatsApp: +90 542 716 69 11.
- Örnek alınan siteler (yapı ve akış için, tasarım kopyalanmaz): https://binhantur.com/ ve https://turiz.com.tr/
- Online ödeme YOK. Rezervasyon WhatsApp üzerinden: her turda tur adı + tarih önceden yazılı "WhatsApp ile Rezervasyon" butonu.
- Tur listesi/detay, basit yönetim paneli (tur, tarih, kontenjan), KVKK ve TURSAB bilgisi.

## Çalışma kuralları

- Commit ve push Halil'in onayı olmadan yapılmaz; önce dosya listesi + Türkçe commit mesajı gösterilir.
- İlk commit dışında main'e doğrudan push yok; işler `ozellik/<ad>` branch'lerinde, PR ile birleşir.
- Tasarım: kodlamadan önce görsel taslak onayı alınır.
