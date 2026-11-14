<#
Locktech sunucu kurulumu — Windows Server 2019/2022 üzerinde, YÖNETİCİ olarak açılmış PowerShell'de çalıştırılır.

Hazırlık: geliştirme bilgisayarındaki publish\sunucu klasörünün içeriğini (bu betik + locktech.zip)
uzak masaüstüyle sunucuda C:\kurulum klasörüne kopyalayın.

  1) İlk kurulum (IIS, .NET 8, site, panel şifresi, güvenlik duvarı):
       cd C:\kurulum
       powershell -ExecutionPolicy Bypass -File .\sunucu-kurulum.ps1

  2) DNS kayıtları sunucuya yönlendikten sonra SSL sertifikası:
       powershell -ExecutionPolicy Bypass -File .\sunucu-kurulum.ps1 -Islem Ssl -Eposta siz@ornek.com

  3) Siteyi güncellemek (yeni locktech.zip kopyalandıktan sonra):
       powershell -ExecutionPolicy Bypass -File .\sunucu-kurulum.ps1 -Islem Guncelle

Betik tekrar çalıştırılabilir: kurulu olanı atlar, eksik olanı tamamlar.
Uzak masaüstü (RDP) ayarlarına ve Windows güncellemelerine dokunmaz.
#>
param(
    [ValidateSet("Kur", "Guncelle", "Ssl")][string]$Islem = "Kur",
    [string]$Paket = (Join-Path $PSScriptRoot "locktech.zip"),
    # Let's Encrypt'in sertifika süresi uyarılarını göndereceği adres (yalnızca -Islem Ssl için)
    [string]$Eposta = "",
    [string]$SunucuIp = "185.33.234.70"
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$SiteAdi = "Locktech"
$SiteKlasoru = "C:\siteler\locktech"
$AlanAdlari = @("locktech.com.tr", "www.locktech.com.tr")
$Indirilenler = "C:\kurulum\indirilenler"
$BarindirmaPaketi = "https://aka.ms/dotnet/8.0/dotnet-hosting-win.exe"
$WinAcme = "https://github.com/win-acme/win-acme/releases/download/v2.4.1/win-acme.v2.4.1.2444.x64.pluggable.zip"
$WinAcmeKlasoru = "C:\araclar\win-acme"
$HavuzKimligi = "IIS AppPool\$SiteAdi"

function Adim($metin) { Write-Host ""; Write-Host "== $metin" -ForegroundColor Cyan }
function Tamam($metin) { Write-Host "   $metin" -ForegroundColor Green }
function Uyari($metin) { Write-Host "   $metin" -ForegroundColor Yellow }

$kimlik = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $kimlik.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    throw "Bu betik yönetici olarak çalıştırılmalı: PowerShell'e sağ tıklayıp 'Yönetici olarak çalıştır' seçin."
}

function Indir($adres, $dosya) {
    New-Item -ItemType Directory -Force $Indirilenler | Out-Null
    $yol = Join-Path $Indirilenler $dosya
    if (-not (Test-Path $yol)) {
        Write-Host "   İndiriliyor: $adres"
        Invoke-WebRequest -Uri $adres -OutFile $yol -UseBasicParsing
    }
    return $yol
}

function IisKur {
    Adim "IIS (web sunucusu)"
    $ozellikler = "Web-Server", "Web-Default-Doc", "Web-Static-Content", "Web-Http-Errors", "Web-Http-Logging",
                  "Web-Stat-Compression", "Web-Dyn-Compression", "Web-Filtering", "Web-Mgmt-Console"
    $eksik = Get-WindowsFeature -Name $ozellikler | Where-Object { -not $_.Installed }
    if ($eksik) {
        Install-WindowsFeature -Name ($eksik | ForEach-Object { $_.Name }) | Out-Null
        Tamam "IIS kuruldu."
    } else { Tamam "IIS zaten kurulu." }
}

function BarindirmaPaketiKur {
    Adim ".NET 8 barındırma paketi"
    $modul = Join-Path $env:ProgramFiles "IIS\Asp.Net Core Module\V2\aspnetcorev2.dll"
    $calismaZamani = Get-ChildItem (Join-Path $env:ProgramFiles "dotnet\shared\Microsoft.AspNetCore.App") -Directory -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -like "8.*" }
    if ((Test-Path $modul) -and $calismaZamani) { Tamam ".NET 8 zaten kurulu ($($calismaZamani[-1].Name))."; return }

    $kurucu = Indir $BarindirmaPaketi "dotnet-hosting-8-win.exe"
    $islem = Start-Process -FilePath $kurucu -ArgumentList "/install", "/quiet", "/norestart" -Wait -PassThru
    if ($islem.ExitCode -ne 0 -and $islem.ExitCode -ne 3010) { throw ".NET barındırma paketi kurulamadı (çıkış kodu $($islem.ExitCode))." }
    # IIS'in yeni modülü görmesi için servisler yeniden başlatılır.
    & net.exe stop was /y | Out-Null
    & net.exe start w3svc | Out-Null
    Tamam ".NET 8 barındırma paketi kuruldu."
}

function SiteKur {
    Adim "IIS sitesi ve uygulama havuzu"
    Import-Module WebAdministration
    New-Item -ItemType Directory -Force $SiteKlasoru | Out-Null
    New-Item -ItemType Directory -Force (Join-Path $SiteKlasoru "App_Data") | Out-Null

    if (-not (Test-Path "IIS:\AppPools\$SiteAdi")) { New-WebAppPool -Name $SiteAdi | Out-Null }
    Set-ItemProperty "IIS:\AppPools\$SiteAdi" -Name managedRuntimeVersion -Value ""          # .NET (Core) için "Yönetilen kod yok"
    Set-ItemProperty "IIS:\AppPools\$SiteAdi" -Name startMode -Value "AlwaysRunning"
    Set-ItemProperty "IIS:\AppPools\$SiteAdi" -Name processModel.idleTimeout -Value ([TimeSpan]::Zero)

    if (-not (Get-Website -Name $SiteAdi -ErrorAction SilentlyContinue)) {
        New-Website -Name $SiteAdi -PhysicalPath $SiteKlasoru -ApplicationPool $SiteAdi -Port 80 -HostHeader $AlanAdlari[0] | Out-Null
    }
    foreach ($alan in $AlanAdlari) {
        if (-not (Get-WebBinding -Name $SiteAdi -Protocol http -HostHeader $alan)) {
            New-WebBinding -Name $SiteAdi -Protocol http -Port 80 -HostHeader $alan
        }
    }

    # Varsayılan IIS karşılama sitesi kapatılır; IP adresiyle gelenler boş sayfa görür.
    $varsayilan = Get-Website -Name "Default Web Site" -ErrorAction SilentlyContinue
    if ($varsayilan -and $varsayilan.State -eq "Started") {
        Stop-Website -Name "Default Web Site"
        Set-ItemProperty "IIS:\Sites\Default Web Site" -Name serverAutoStart -Value $false
    }

    # Uygulama havuzu site dosyalarını okur; yalnızca App_Data'ya (ayarlar, anahtarlar) yazar.
    & icacls.exe $SiteKlasoru /grant "${HavuzKimligi}:(OI)(CI)RX" /T /Q | Out-Null
    & icacls.exe (Join-Path $SiteKlasoru "App_Data") /grant "${HavuzKimligi}:(OI)(CI)M" /T /Q | Out-Null
    Tamam "Site '$SiteAdi' hazır: $SiteKlasoru"
}

function PaketiYukle {
    Adim "Site dosyaları"
    if (-not (Test-Path $Paket)) { throw "Paket bulunamadı: $Paket  (locktech.zip bu betikle aynı klasörde olmalı)" }
    Import-Module WebAdministration
    $cevrimdisi = Join-Path $SiteKlasoru "app_offline.htm"
    Set-Content -Path $cevrimdisi -Value "<!doctype html><meta charset='utf-8'><title>Bakım</title><p>Site güncelleniyor, lütfen birkaç saniye sonra yeniden deneyin.</p>" -Encoding UTF8

    if ((Get-WebAppPoolState -Name $SiteAdi).Value -ne "Stopped") { Stop-WebAppPool -Name $SiteAdi }
    for ($i = 0; $i -lt 30 -and (Get-WebAppPoolState -Name $SiteAdi).Value -ne "Stopped"; $i++) { Start-Sleep -Milliseconds 500 }

    # App_Data ve appsettings.Production.local.json pakette yoktur, bu yüzden korunur.
    Expand-Archive -Path $Paket -DestinationPath $SiteKlasoru -Force
    & icacls.exe $SiteKlasoru /grant "${HavuzKimligi}:(OI)(CI)RX" /T /Q | Out-Null
    & icacls.exe (Join-Path $SiteKlasoru "App_Data") /grant "${HavuzKimligi}:(OI)(CI)M" /T /Q | Out-Null

    Remove-Item $cevrimdisi -Force
    Start-WebAppPool -Name $SiteAdi
    Tamam "Paket yüklendi: $Paket"
}

function PanelSifresiBelirle {
    Adim "Yönetim paneli şifresi"
    $dosya = Join-Path $SiteKlasoru "appsettings.Production.local.json"
    if (Test-Path $dosya) { Tamam "Panel şifresi zaten tanımlı (değiştirmek için $dosya dosyasını silip betiği yeniden çalıştırın)."; return }

    function DuzMetin($guvenli) {
        $isaretci = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($guvenli)
        try { return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($isaretci) }
        finally { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($isaretci) }
    }
    while ($true) {
        $bir = DuzMetin (Read-Host "   Panel için yeni bir şifre yazın (en az 12 karakter)" -AsSecureString)
        $iki = DuzMetin (Read-Host "   Şifreyi yeniden yazın" -AsSecureString)
        if ($bir -ne $iki) { Uyari "Şifreler aynı değil, yeniden deneyin."; continue }
        if ($bir.Length -lt 12) { Uyari "Şifre en az 12 karakter olmalı."; continue }
        break
    }
    @{ Panel = @{ Sifre = $bir } } | ConvertTo-Json | Set-Content -Path $dosya -Encoding UTF8
    # Dosyayı yalnızca yöneticiler, sistem ve sitenin uygulama havuzu okuyabilsin.
    & icacls.exe $dosya /inheritance:r /grant "*S-1-5-32-544:F" "*S-1-5-18:F" "${HavuzKimligi}:R" /Q | Out-Null
    Tamam "Panel şifresi kaydedildi. Şifreyi güvenli bir yere not edin; panel adresi: /panel"
}

function GuvenlikDuvari {
    Adim "Güvenlik duvarı (80 ve 443)"
    foreach ($kural in @(@{ Ad = "IIS-WebServerRole-HTTP-In-TCP"; Port = 80 }, @{ Ad = "IIS-WebServerRole-HTTPS-In-TCP"; Port = 443 })) {
        $mevcut = Get-NetFirewallRule -Name $kural.Ad -ErrorAction SilentlyContinue
        if ($mevcut) { Enable-NetFirewallRule -Name $kural.Ad }
        elseif (-not (Get-NetFirewallRule -Name "Web-$($kural.Port)" -ErrorAction SilentlyContinue)) {
            New-NetFirewallRule -Name "Web-$($kural.Port)" -DisplayName "Web $($kural.Port)" -Direction Inbound -Protocol TCP -LocalPort $kural.Port -Action Allow | Out-Null
        }
    }
    Tamam "80 (http) ve 443 (https) portları açık."
}

function SiteyiDene($sema) {
    Adim "Site denemesi ($sema)"
    try {
        # http: alan adı henüz yönlenmemiş olabilir, bu yüzden yerel adres + Host başlığı. https: sertifika alan adına bağlı.
        if ($sema -eq "https") { $istek = [Net.WebRequest]::Create("https://$($AlanAdlari[0])/") }
        else { $istek = [Net.WebRequest]::Create("http://127.0.0.1/"); $istek.Host = $AlanAdlari[0] }
        $istek.AllowAutoRedirect = $false
        $istek.Timeout = 60000
        if ($sema -eq "https") { [Net.ServicePointManager]::ServerCertificateValidationCallback = { $true } }
        $yanit = $istek.GetResponse()
        $kod = [int]$yanit.StatusCode
        $yanit.Close()
        if ($kod -eq 200 -or $kod -eq 301 -or $kod -eq 302 -or $kod -eq 307) { Tamam "Site yanıt veriyor (HTTP $kod)." }
        else { Uyari "Site beklenmeyen bir yanıt verdi (HTTP $kod)." }
    } catch {
        Uyari "Site yanıt vermedi: $($_.Exception.Message)"
        Uyari "Olay Görüntüleyicisi > Windows Günlükleri > Uygulama bölümünde 'IIS AspNetCore Module' kayıtlarına bakın."
    } finally { [Net.ServicePointManager]::ServerCertificateValidationCallback = $null }
}

function SslKur {
    Adim "DNS denetimi"
    if (-not $Eposta) { throw "Sertifika için bir e-posta adresi gerekli:  -Islem Ssl -Eposta siz@ornek.com" }
    foreach ($alan in $AlanAdlari) {
        $kayitlar = @(Resolve-DnsName -Name $alan -Type A -Server 1.1.1.1 -ErrorAction SilentlyContinue | Where-Object { $_.Type -eq "A" } | ForEach-Object { $_.IPAddress })
        if ($kayitlar -notcontains $SunucuIp) {
            throw "$alan henüz bu sunucuya ($SunucuIp) yönlenmiyor (bulunan: $($kayitlar -join ', ')). DNS kayıtlarını girip yayılmasını bekleyin, sonra yeniden deneyin."
        }
        Tamam "$alan -> $SunucuIp"
    }

    Adim "SSL sertifikası (Let's Encrypt, win-acme)"
    Import-Module WebAdministration
    $wacs = Join-Path $WinAcmeKlasoru "wacs.exe"
    if (-not (Test-Path $wacs)) {
        $zip = Indir $WinAcme "win-acme.zip"
        New-Item -ItemType Directory -Force $WinAcmeKlasoru | Out-Null
        Expand-Archive -Path $zip -DestinationPath $WinAcmeKlasoru -Force
    }
    $siteNo = (Get-Website -Name $SiteAdi).Id
    & $wacs --source iis --siteid $siteNo --host ($AlanAdlari -join ",") --validation selfhosting `
            --store certificatestore --installation iis --installationsiteid $siteNo `
            --accepttos --emailaddress $Eposta --setuptaskscheduler
    if ($LASTEXITCODE -ne 0) {
        Uyari "win-acme hata verdi. Elle denemek için: $wacs  (menüden 'N' ile yeni sertifika oluşturun)."
        throw "Sertifika alınamadı."
    }
    Tamam "Sertifika kuruldu; yenileme için zamanlanmış görev oluşturuldu."
    SiteyiDene "https"
}

switch ($Islem) {
    "Kur" {
        IisKur
        BarindirmaPaketiKur
        SiteKur
        PaketiYukle
        PanelSifresiBelirle
        GuvenlikDuvari
        # Şifre dosyasının okunması için havuz yeniden başlatılır.
        Import-Module WebAdministration
        Restart-WebAppPool -Name $SiteAdi
        SiteyiDene "http"
        Write-Host ""
        Write-Host "Kurulum tamamlandı. Sıradaki adımlar:" -ForegroundColor Cyan
        Write-Host "  1. İsimtescil'de locktech.com.tr için @ ve www A kayıtlarını $SunucuIp adresine yönlendirin."
        Write-Host "  2. Kayıtlar yayılınca: .\sunucu-kurulum.ps1 -Islem Ssl -Eposta siz@ornek.com"
        Write-Host "  3. https://locktech.com.tr/panel adresinden SMTP ve WhatsApp ayarlarını girin."
    }
    "Guncelle" {
        PaketiYukle
        SiteyiDene "http"
    }
    "Ssl" { SslKur }
}
