# 5 Site Yayın Planı (11.11.2026)

Canlı doküman: https://claude.ai/artifact/Gouz19yn7Tp6hGYV6JDgbs

## Karar
5 site tek bir Windows VPS'te, ASP.NET Core ile. Vercel yok. Yıllık maliyet yaklaşık 210 $ + KDV (VPS).

## VPS
4 çekirdek / 8 GB RAM / 80 GB+ SSD, Türkiye lokasyonu, Windows lisansı dahil. Adaylar:
- eHost eH 3: 4 core / 8 GB / 120 GB, 17,33 $/ay, Windows Server 2022 lisansı dahil
- Hosting.com.tr VDS Ultra 120: 8 core / 8 GB / 120 GB, 17,49 $/ay (lisansı sor)
- Natro VDS Pro 4: 4 vCPU / 8 GB / 300 GB NVMe, 29,99 $/ay (yedek/snapshot dahil)

Sipariş öncesi sor: lisans dahil mi, yenileme fiyatı, yedek/snapshot, ek IP ücreti.

## Sunucu kurulumu
1. Windows güncellemeleri, RDP portu değişimi, güvenlik duvarında yalnız 80/443/RDP.
2. IIS + ASP.NET Core Hosting Bundle + URL Rewrite.
3. SQL Server Express (dışarı port açılmaz).
4. IIS'te 4 site (Locktech, Taşımacılık, Acenta) + "Redirects" sitesi, her birine ayrı App Pool.
5. win-acme ile 11 domainin www'li ve www'suz halleri için SSL.
6. Günlük SQL yedeği + site klasörleri sunucu dışına.
7. GitHub Actions ile Web Deploy.

## DNS (İsimtescil)
1. Her domain için `@` ve `www` A kaydı → VPS IP'si.
2. Ana domainler kendi IIS sitelerine; 3 taşımacılık .com domaini tek siteye.
3. Yönlenecek 6 domain "Redirects" sitesine; URL Rewrite ile 301, yol korunur.
4. www → www'suz, http → https.
5. Her ana domain Google Search Console'a, sitemap gönderilir.

## Sıra
1. Altyapı (VPS, DNS, SSL, "yakında" sayfaları)
2. Locktech
3. Taşımacılık (tek kod, üç marka)
4. Acenta
5. Kontrol ve yayın (mobil, hız, Search Console, yedek doğrulama)

## Sorumluluklar
- Logolar, tasarım, metinler, İngilizce çeviri, kod: Claude. Halil gerektiğinde düzeltme ister.
- VPS siparişi ve İsimtescil DNS kayıtları: Halil (kayıt listesi Claude hazırlar).
