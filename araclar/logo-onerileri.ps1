# Taşımacılık logo önerileri sayfasını üretir: docs/tasimacilik/logo-onerileri.html
# Yazı tipleri sayfaya gömülür, böylece dosya tek başına açılabilir.
$ErrorActionPreference = "Stop"
$kok = Split-Path $PSScriptRoot -Parent
$sablon = [IO.File]::ReadAllText((Join-Path $kok "docs\tasimacilik\logo-onerileri.sablon.html"))

# Semboller (viewBox 0 0 64 64). Renkler CSS değişkenlerinden gelir.
$semboller = [ordered]@{
  "selcuklu" = [ordered]@{
    "A" = @{ Ad = "Selçuklu yıldızı"; Not = "İç içe geçmiş iki kareden oluşan sekiz köşeli Selçuklu yıldızı; ortada altın halka ve küçük yıldız. Sade, köklü, kurumsal."
             Svg = '<rect x="13" y="13" width="38" height="38" rx="4" fill="var(--ana)"/><rect x="13" y="13" width="38" height="38" rx="4" fill="var(--ana)" transform="rotate(45 32 32)"/><circle cx="32" cy="32" r="13.5" fill="none" stroke="var(--vurgu)" stroke-width="2.6"/><rect x="26.5" y="26.5" width="11" height="11" rx="1" fill="var(--vurgu)"/><rect x="26.5" y="26.5" width="11" height="11" rx="1" fill="var(--vurgu)" transform="rotate(45 32 32)"/><circle cx="32" cy="32" r="2.8" fill="var(--koyu)"/>' }
    "B" = @{ Ad = "Taç kapı ve yol"; Not = "Selçuklu taç kapısının sivri kemeri; kapıdan ufka uzanan yol ve doğan güneş. Hem tarih hem yolculuk anlatır."
             Svg = '<path d="M9 61V28C9 14 19 6 32 2c13 4 23 12 23 26v33z" fill="var(--ana)"/><path d="M16 61V30c0-10 7-17 16-21 9 4 16 11 16 21v31" fill="none" stroke="var(--vurgu)" stroke-width="2.6"/><circle cx="32" cy="27" r="4" fill="var(--vurgu)"/><path d="M21 61l9-26h4l9 26z" fill="var(--koyu)"/><path d="M32 59V38" stroke="var(--vurgu)" stroke-width="2" stroke-dasharray="3.2 3"/>' }
    "C" = @{ Ad = "S monogramı"; Not = "Yuvarlak rozet içinde altın çizgili Selçuklu yıldızı ve ortasında markanın baş harfi S. Mühür gibi, ağırbaşlı."
             Svg = '<circle cx="32" cy="32" r="31" fill="var(--ana)"/><rect x="14.5" y="14.5" width="35" height="35" rx="2" fill="none" stroke="var(--vurgu)" stroke-width="1.8"/><rect x="14.5" y="14.5" width="35" height="35" rx="2" fill="none" stroke="var(--vurgu)" stroke-width="1.8" transform="rotate(45 32 32)"/><text x="32" y="43" text-anchor="middle" font-family="Playfair Display, Georgia, serif" font-size="31" font-weight="700" fill="#fff">S</text>' }
  }
  "otobus" = [ordered]@{
    "A" = @{ Ad = "Otobüs önü"; Not = "Kırmızı rozet içinde önden görünen otobüs: güzergâh tabelası, ön cam, farlar. Ne iş yaptığı ilk bakışta anlaşılır."
             Svg = '<rect width="64" height="64" rx="16" fill="var(--ana)"/><path d="M14 19h-2.5v8M50 19h2.5v8" fill="none" stroke="#fff" stroke-width="2.4" stroke-linecap="round"/><rect x="14" y="9" width="36" height="41" rx="7" fill="#fff"/><rect x="18" y="13" width="28" height="5" rx="1.5" fill="var(--vurgu)"/><rect x="18" y="20" width="28" height="14" rx="2.5" fill="var(--koyu)"/><circle cx="21.5" cy="41.5" r="2.7" fill="var(--vurgu)"/><circle cx="42.5" cy="41.5" r="2.7" fill="var(--vurgu)"/><rect x="27" y="40.2" width="10" height="2.6" rx="1.3" fill="var(--koyu)"/><rect x="16.5" y="50" width="7" height="6" rx="2" fill="#fff"/><rect x="40.5" y="50" width="7" height="6" rx="2" fill="#fff"/>' }
    "B" = @{ Ad = "Yoldaki otobüs"; Not = "Yuvarlak rozet içinde yandan, hızla ilerleyen bir otobüs ve hız çizgileri. Hareketli, enerjik."
             Svg = '<circle cx="32" cy="32" r="31" fill="var(--ana)"/><path d="M5 27h6M3 32h8M6 37h5" stroke="#fff" stroke-width="2.2" stroke-linecap="round" opacity=".85"/><path d="M14 41V26c0-3.3 2.7-6 6-6h27c3 0 5.4 2 6.2 4.9L56 35v6z" fill="#fff"/><path d="M18 23.5h28.5l2.6 8.5H18z" fill="var(--koyu)"/><rect x="14" y="35" width="42" height="2.6" fill="var(--vurgu)"/><circle cx="23" cy="42" r="4.6" fill="var(--koyu)"/><circle cx="23" cy="42" r="1.7" fill="#fff"/><circle cx="47" cy="42" r="4.6" fill="var(--koyu)"/><circle cx="47" cy="42" r="1.7" fill="#fff"/><path d="M12 50h44" stroke="var(--vurgu)" stroke-width="2.4" stroke-dasharray="6 4" stroke-linecap="round"/>' }
    "C" = @{ Ad = "Ankara Kalesi"; Not = "Ankara Kalesi'nin burç ve mazgalları, altında sarı bir yol. Şehirle bağı öne çıkarır, yerel ve akılda kalıcı."
             Svg = '<rect width="64" height="64" rx="16" fill="var(--ana)"/><path d="M12 38V20h6v5h4v-5h6v5h8v-5h6v5h4v-5h6v18z" fill="#fff"/><path d="M28 38v-6a4 4 0 0 1 8 0v6z" fill="var(--ana)"/><rect x="17" y="27" width="3" height="4" rx="1" fill="var(--ana)"/><rect x="44" y="27" width="3" height="4" rx="1" fill="var(--ana)"/><path d="M8 52c10-6 18-8 24-8s14 2 24 8" fill="none" stroke="var(--vurgu)" stroke-width="4" stroke-linecap="round"/><path d="M24 48.6c2.6-.8 5.2-1.2 8-1.2s5.4.4 8 1.2" fill="none" stroke="var(--koyu)" stroke-width="1.6" stroke-dasharray="3 3"/>' }
  }
  "transfer" = [ordered]@{
    "A" = @{ Ad = "Konum ve yön"; Not = "Konum işaretinin içinde ileri ok: bir noktadan alıp diğerine götürmek. Transferi en sade şekilde anlatır."
             Svg = '<path d="M32 3C20.4 3 11 12.2 11 23.5 11 38.5 32 61 32 61s21-22.5 21-37.5C53 12.2 43.6 3 32 3z" fill="var(--ana)"/><circle cx="32" cy="23.5" r="12.5" fill="#fff"/><path d="M24.5 23.5h14M33.5 18l5.5 5.5-5.5 5.5" fill="none" stroke="var(--vurgu)" stroke-width="3.4" stroke-linecap="round" stroke-linejoin="round"/>' }
    "B" = @{ Ad = "Uçak ve yol"; Not = "Havalanan uçak ve altında kıvrılan yol: havalimanından adrese. Esenboğa transferini doğrudan çağrıştırır."
             Svg = '<rect width="64" height="64" rx="16" fill="var(--ana)"/><g transform="translate(16 4) scale(1.45) rotate(42 12 12)"><path d="M10.5 3.5c0-1.4 3-1.4 3 0V9l7.5 4.5v2.2L13.5 13v4.5l2.5 2V21l-4-1-4 1v-1.5l2.5-2V13L3 15.7v-2.2L10.5 9z" fill="#fff"/></g><path d="M7 53c8-7 17-10 27-10 8 0 15 2 23 6" fill="none" stroke="var(--vurgu)" stroke-width="4.5" stroke-linecap="round"/><path d="M17 47.5c5-2.6 11-4 17-4" fill="none" stroke="var(--koyu)" stroke-width="1.5" stroke-dasharray="3 3"/>' }
    "C" = @{ Ad = "Gidiş ve dönüş"; Not = "İki nokta arasında birbirini tamamlayan iki ok: gidiş ve dönüş. Modern, sembolik, her transfer türüne uyar."
             Svg = '<circle cx="32" cy="32" r="31" fill="var(--ana)"/><path d="M17 29a15.5 15.5 0 0 1 27.5-7.5" fill="none" stroke="var(--vurgu)" stroke-width="4.2" stroke-linecap="round"/><path d="M45.8 14.5l-.6 8.2-8.1-1" fill="none" stroke="var(--vurgu)" stroke-width="4.2" stroke-linecap="round" stroke-linejoin="round"/><path d="M47 35a15.5 15.5 0 0 1-27.5 7.5" fill="none" stroke="#fff" stroke-width="4.2" stroke-linecap="round"/><path d="M18.2 49.5l.6-8.2 8.1 1" fill="none" stroke="#fff" stroke-width="4.2" stroke-linecap="round" stroke-linejoin="round"/><circle cx="32" cy="32" r="4" fill="#fff"/>' }
  }
}
$markalar = [ordered]@{
  "selcuklu" = @{ Sinif = "m-selcuklu"; Ad = "Selçuklu Turizm"; Ust = "SELÇUKLU"; Alt = "TURİZM"; Not = "Tur ve gezi taşımacılığı. Turkuaz ve altın renkler, klasik serifli yazı." }
  "otobus"   = @{ Sinif = "m-otobus"; Ad = "Ankara Otobüs Kirala"; Ust = "ANKARA"; Alt = "OTOBÜS KİRALA"; Not = "Şoförlü araç kiralama. Kırmızı ve sarı, dar ve güçlü yazı." }
  "transfer" = @{ Sinif = "m-transfer"; Ad = "Ankara Turizm Transfer"; Ust = "ANKARA TURİZM"; Alt = "TRANSFER"; Not = "Havalimanı, otel ve şehirlerarası transfer. Lacivert ve nane yeşili, modern yazı." }
}

$html = New-Object Text.StringBuilder
foreach ($k in $markalar.Keys) {
  $m = $markalar[$k]
  [void]$html.Append("<section class=`"$($m.Sinif)`"><h2>$($m.Ad)</h2><p>$($m.Not)</p><div class=`"secenekler`">`n")
  foreach ($s in $semboller[$k].Keys) {
    $o = $semboller[$k][$s]
    $svg = "<svg viewBox=`"0 0 64 64`" aria-hidden=`"true`">$($o.Svg)</svg>"
    $kilit = "<div class=`"lockup`">$svg<span><small>$($m.Ust)</small><b>$($m.Alt)</b></span></div>"
    $kucuk = "<svg viewBox=`"0 0 64 64`" width=`"32`" height=`"32`">$($o.Svg)</svg><svg viewBox=`"0 0 64 64`" width=`"16`" height=`"16`">$($o.Svg)</svg>"
    [void]$html.Append("<article class=`"kart`"><header><b>$s · $($o.Ad)</b><span>Seçenek $s</span></header><div class=`"zemin`">$kilit</div><div class=`"zemin koyu`">$kilit</div><div class=`"kucukler`">$kucuk<span>Sekme simgesi (32 ve 16 piksel)</span></div><p class=`"aciklama`">$($o.Not)</p></article>`n")
  }
  [void]$html.Append("</div></section>`n")
}

# Yazı tiplerini gömülü @font-face olarak ekle
$fontlar = [IO.File]::ReadAllText((Join-Path $kok "src\Tasimacilik\wwwroot\css\fonts.css"))
$fontlar = [regex]::Replace($fontlar, 'url\(\.\./fonts/([^)]+)\)', { param($e) "url(data:font/woff2;base64," + [Convert]::ToBase64String([IO.File]::ReadAllBytes((Join-Path $kok "src\Tasimacilik\wwwroot\fonts\$($e.Groups[1].Value)"))) + ")" })

$sonuc = $sablon.Replace("/*FONTLAR*/", $fontlar).Replace("<!--MARKALAR-->", $html.ToString())
$cikti = Join-Path $kok "docs\tasimacilik\logo-onerileri.html"
[IO.File]::WriteAllText($cikti, $sonuc, (New-Object Text.UTF8Encoding $false))
"{0}  ({1:N0} KB)" -f $cikti, ((Get-Item $cikti).Length / 1KB)
