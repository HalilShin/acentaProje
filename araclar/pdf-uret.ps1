# Locktech ürün PDF'lerini üretir.
# Site çalışırken (http://localhost:5031) her ürünün /pdf sayfasını Edge ile A4 PDF olarak yazdırır
# ve src/Locktech/wwwroot/pdf klasörüne Locktech-<MODEL>-tr.pdf / -en.pdf adıyla kaydeder.
# Ürün verisi ya da PDF tasarımı değiştiğinde yeniden çalıştırın:  pwsh araclar/pdf-uret.ps1
param([string]$Site = "http://localhost:5031")

$ErrorActionPreference = "Stop"
$kok = Split-Path $PSScriptRoot -Parent
$hedef = New-Item -ItemType Directory -Force (Join-Path $kok "src\Locktech\wwwroot\pdf")

$edge = @("${env:ProgramFiles(x86)}\Microsoft\Edge\Application\msedge.exe", "$env:ProgramFiles\Microsoft\Edge\Application\msedge.exe") |
    Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $edge) { throw "Microsoft Edge bulunamadı." }

$profil = Join-Path ([IO.Path]::GetTempPath()) "locktech-pdf-profil"
$urunler = @{ "hb-a1sp" = "HB-A1SP"; "hb-a1m" = "HB-A1M"; "hb-a1ms" = "HB-A1Ms"; "hb-vt08" = "HB-VT08" }

foreach ($kod in $urunler.Keys | Sort-Object) {
    foreach ($dil in "tr", "en") {
        $adres = if ($dil -eq "en") { "$Site/en/products/$kod/pdf" } else { "$Site/urunler/$kod/pdf" }
        $dosya = Join-Path $hedef "Locktech-$($urunler[$kod])-$dil.pdf"
        Remove-Item $dosya -ErrorAction SilentlyContinue
        $arg = @("--headless=new", "--disable-gpu", "--no-pdf-header-footer", "--virtual-time-budget=10000",
                 "--user-data-dir=`"$profil`"", "--print-to-pdf=`"$dosya`"", $adres)
        Start-Process -FilePath $edge -ArgumentList $arg -Wait -WindowStyle Hidden
        if (-not (Test-Path $dosya)) { throw "PDF üretilemedi: $adres" }
        "{0}  {1:N0} KB" -f (Split-Path $dosya -Leaf), ((Get-Item $dosya).Length / 1KB)
    }
}
