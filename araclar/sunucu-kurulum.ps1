<#
Site kurulumu — Windows Server 2019/2022 üzerinde, YÖNETİCİ olarak açılmış PowerShell'de çalıştırılır.
-Site Locktech (varsayılan), -Site Tasimacilik ya da -Site Acenta.
Acenta kurulumu ayrıca SQL Server Express'i kurar, veritabanını hazırlar ve gece yedeğini zamanlar.

Hazırlık: geliştirme bilgisayarındaki publish\sunucu klasörünün içeriğini (bu betik + locktech.zip / tasimacilik.zip / acenta.zip)
uzak masaüstüyle sunucuda C:\kurulum klasörüne kopyalayın.

  1) İlk kurulum (IIS, .NET 8, site, panel şifresi, güvenlik duvarı):
       cd C:\kurulum
       powershell -ExecutionPolicy Bypass -File .\sunucu-kurulum.ps1 -Site Tasimacilik

  2) DNS kayıtları sunucuya yönlendikten sonra SSL sertifikası:
       powershell -ExecutionPolicy Bypass -File .\sunucu-kurulum.ps1 -Site Tasimacilik -Islem Ssl -Eposta siz@ornek.com
     Her alan adı (www'li haliyle) ayrı sertifika alır; DNS'i henüz yönlenmemiş alan adları atlanır,
     betik sonra yeniden çalıştırılabilir. Sertifikası olan alan adları yeniden alınmaz.

  3) Siteyi güncellemek (yeni zip kopyalandıktan sonra):
       powershell -ExecutionPolicy Bypass -File .\sunucu-kurulum.ps1 -Site Tasimacilik -Islem Guncelle

Betik tekrar çalıştırılabilir: kurulu olanı atlar, eksik olanı tamamlar.
Uzak masaüstü (RDP) ayarlarına ve Windows güncellemelerine dokunmaz.
#>
param(
    [ValidateSet("Locktech", "Tasimacilik", "Acenta")][string]$Site = "Locktech",
    [ValidateSet("Kur", "Guncelle", "Ssl")][string]$Islem = "Kur",
    # Boş bırakılırsa bu betikle aynı klasördeki <site>.zip kullanılır.
    [string]$Paket = "",
    # Let's Encrypt'in sertifika süresi uyarılarını göndereceği adres (yalnızca -Islem Ssl için)
    [string]$Eposta = "",
    [string]$SunucuIp = "185.33.234.70"
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# Her site: IIS adı, klasör ve alan adı grupları. Her grup ayrı SSL sertifikası alır; ilk alan adı grubun ana adresidir.
$Siteler = @{
    "Locktech"    = @{ Klasor = "C:\siteler\locktech"; Gruplar = @(
        , @("locktech.com.tr") ) }
    "Tasimacilik" = @{ Klasor = "C:\siteler\tasimacilik"; Gruplar = @(
        @("selcukluturizm.com", "selcukluturizm.com.tr"),
        @("ankaraotobuskirala.com", "ankaraotobuskirala.com.tr", "ankaraotobuskiralama.com.tr"),
        @("ankaraturizmtransfer.com", "ankaraturizmtransfer.com.tr") ) }
    "Acenta"      = @{ Klasor = "C:\siteler\acenta"; Veritabani = "AnkaraKulturGezi"; Gruplar = @(
        , @("ankarakulturgezi.com", "ankarakulturgezi.com.tr", "kulturgezi.com.tr") ) }
}
$SiteAdi = $Site
$SiteKlasoru = $Siteler[$Site].Klasor
# Her alan adı (www'li haliyle) ayrı bir sertifika grubudur; böylece DNS'i önce yayılan alan adı
# diğerlerini beklemeden sertifika alır. Örn. .com yönlendiyse .com.tr'yi beklemez.
$Gruplar = @($Siteler[$Site].Gruplar | ForEach-Object { $_ } | ForEach-Object { , @($_, "www.$_") })
$AlanAdlari = @($Gruplar | ForEach-Object { $_ })
if (-not $Paket) { $Paket = Join-Path $PSScriptRoot "$($Site.ToLowerInvariant()).zip" }
$Indirilenler = "C:\kurulum\indirilenler"
$BarindirmaPaketi = "https://aka.ms/dotnet/8.0/dotnet-hosting-win.exe"
$WinAcme = "https://github.com/win-acme/win-acme/releases/download/v2.4.1/win-acme.v2.4.1.2444.x64.pluggable.zip"
$WinAcmeKlasoru = "C:\araclar\win-acme"
$HavuzKimligi = "IIS AppPool\$SiteAdi"
$Veritabani = $Siteler[$Site].Veritabani
$SqlOrnegi = ".\SQLEXPRESS"
# SQL Server 2022 Express web kurucusu (Microsoft İndirme Merkezi, id=108838). Genel fwlink artık 2025'e yönleniyor.
$SqlKurucu = "https://download.microsoft.com/download/29654887-7cde-4397-bba3-d7f087970845/SQL2022-SSEI-Expr.exe"
$YedekKlasoru = "C:\yedekler\$($Site.ToLowerInvariant())"

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
    # Oturum anahtarları Windows DPAPI ile şifrelenir; bunun için havuzun kullanıcı profili yüklenmeli.
    Set-ItemProperty "IIS:\AppPools\$SiteAdi" -Name processModel.loadUserProfile -Value $true

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
    if (-not (Test-Path $Paket)) { throw "Paket bulunamadı: $Paket  ($(Split-Path $Paket -Leaf) bu betikle aynı klasörde olmalı)" }
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
    if (Test-Path $dosya) {
        Tamam "Panel şifresi zaten tanımlı (değiştirmek için $dosya dosyasını silip betiği yeniden çalıştırın)."
        BaglantiAyariniYaz $dosya
        return
    }

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
    BaglantiAyariniYaz $dosya
}

# Veritabanı kullanan sitede bağlantı ayarı yerel ayar dosyasına eklenir (Windows kimliğiyle, şifresiz).
function BaglantiAyariniYaz($dosya) {
    if (-not $Veritabani) { return }
    $ayar = Get-Content $dosya -Raw -Encoding UTF8 | ConvertFrom-Json
    if ($ayar.ConnectionStrings -and $ayar.ConnectionStrings.$SiteAdi) { return }
    $baglanti = "Server=$SqlOrnegi;Database=$Veritabani;Trusted_Connection=True;TrustServerCertificate=True"
    if (-not $ayar.ConnectionStrings) { $ayar | Add-Member -NotePropertyName ConnectionStrings -NotePropertyValue ([pscustomobject]@{}) -Force }
    $ayar.ConnectionStrings | Add-Member -NotePropertyName $SiteAdi -NotePropertyValue $baglanti -Force
    $ayar | ConvertTo-Json -Depth 5 | Set-Content -Path $dosya -Encoding UTF8
    Tamam "Veritabanı bağlantı ayarı eklendi."
}

function SqlKomut($sql, $veritabani = "master") {
    $baglanti = New-Object System.Data.SqlClient.SqlConnection "Server=$SqlOrnegi;Database=$veritabani;Integrated Security=True;Connect Timeout=30"
    $baglanti.Open()
    try {
        $komut = $baglanti.CreateCommand()
        $komut.CommandText = $sql
        $komut.CommandTimeout = 600
        [void]$komut.ExecuteNonQuery()
    } finally { $baglanti.Close() }
}

function SqlKur {
    Adim "SQL Server Express"
    if (Get-Service -Name 'MSSQL$SQLEXPRESS' -ErrorAction SilentlyContinue) {
        Start-Service -Name 'MSSQL$SQLEXPRESS'
        try { SqlKomut "SELECT 1" } catch {
            throw "SQLEXPRESS kurulu ama bu Windows kullanıcısıyla bağlanılamadı ($($_.Exception.Message)). Betiği SQL'i kuran yönetici hesabıyla çalıştırın."
        }
        Tamam "SQL Server Express zaten kurulu ve erişilebilir."
        return
    }

    $kurucu = Indir $SqlKurucu "SQL2022-SSEI-Expr.exe"
    Write-Host "   Kuruluyor (indirme dahil 5-15 dakika sürebilir)..."
    # Varsayılan örnek adı SQLEXPRESS; betiği çalıştıran yönetici SQL yöneticisi olur.
    $islem = Start-Process -FilePath $kurucu -ArgumentList "/ACTION=Install", "/QUIET", "/IACCEPTSQLSERVERLICENSETERMS", "/ENU" -Wait -PassThru
    if ($islem.ExitCode -eq 3010) {
        Uyari "SQL Server kuruldu ama Windows yeniden başlatma istiyor."
        throw "Sunucuyu yeniden başlatın, sonra aynı komutu yeniden çalıştırın (kurulan kısımlar atlanır)."
    }
    if ($islem.ExitCode -ne 0) {
        throw "SQL Server Express kurulamadı (çıkış kodu $($islem.ExitCode)). Kurulum günlüğü: C:\Program Files\Microsoft SQL Server\160\Setup Bootstrap\Log"
    }
    if (-not (Get-Service -Name 'MSSQL$SQLEXPRESS' -ErrorAction SilentlyContinue)) { throw "SQL Server Express hizmeti kurulumdan sonra bulunamadı." }
    Set-Service -Name 'MSSQL$SQLEXPRESS' -StartupType Automatic
    Start-Service -Name 'MSSQL$SQLEXPRESS'
    SqlKomut "SELECT 1"
    Tamam "SQL Server Express kuruldu (örnek: $SqlOrnegi). Dışarıdan bağlantıya kapalıdır."
}

function VeritabaniKur {
    Adim "Veritabanı ve izinler"
    $havuz = "IIS APPPOOL\$SiteAdi"
    # Yedek görevi SYSTEM hesabıyla çalışır; hesap adı Windows diline göre değişir (NT AUTHORITY / NT YETKİLİSİ).
    $sistem = (New-Object Security.Principal.SecurityIdentifier "S-1-5-18").Translate([Security.Principal.NTAccount]).Value
    # Tablolar site ilk açıldığında oluşturulur (EF Core göçleri); havuz kendi veritabanında db_owner olur.
    SqlKomut @"
IF DB_ID(N'$Veritabani') IS NULL CREATE DATABASE [$Veritabani];
IF SUSER_ID(N'$havuz') IS NULL CREATE LOGIN [$havuz] FROM WINDOWS WITH DEFAULT_DATABASE = [$Veritabani];
IF SUSER_ID(N'$sistem') IS NULL CREATE LOGIN [$sistem] FROM WINDOWS;
"@
    SqlKomut @"
IF USER_ID(N'$havuz') IS NULL CREATE USER [$havuz] FOR LOGIN [$havuz];
ALTER ROLE db_owner ADD MEMBER [$havuz];
IF USER_ID(N'$sistem') IS NULL CREATE USER [$sistem] FOR LOGIN [$sistem];
ALTER ROLE db_backupoperator ADD MEMBER [$sistem];
"@ $Veritabani
    Tamam "Veritabanı '$Veritabani' hazır; '$havuz' erişebilir."
}

function YedekZamanla {
    Adim "Gece yedeği (veritabanı + tur fotoğrafları, 14 gün)"
    New-Item -ItemType Directory -Force $YedekKlasoru | Out-Null
    # SQL Server hizmeti yedek dosyasını bu klasöre yazar.
    & icacls.exe $YedekKlasoru /grant 'NT Service\MSSQL$SQLEXPRESS:(OI)(CI)M' /T /Q | Out-Null

    $betik = Join-Path "C:\araclar" "yedek-$($Site.ToLowerInvariant()).ps1"
    New-Item -ItemType Directory -Force "C:\araclar" | Out-Null
    $icerik = @"
# Otomatik oluşturuldu (sunucu-kurulum.ps1). Her gece SYSTEM hesabıyla çalışır.
`$ErrorActionPreference = "Stop"
`$gunluk = Join-Path "$YedekKlasoru" "yedek.log"
try {
    `$zaman = Get-Date -Format "yyyy-MM-dd_HHmm"
    `$dosya = Join-Path "$YedekKlasoru" "$Veritabani-`$zaman.bak"
    `$b = New-Object System.Data.SqlClient.SqlConnection "Server=$SqlOrnegi;Database=master;Integrated Security=True"
    `$b.Open()
    `$k = `$b.CreateCommand(); `$k.CommandTimeout = 1800
    # Express sıkıştırmalı yedek desteklemez; CHECKSUM bozuk sayfaları yakalar.
    `$k.CommandText = "BACKUP DATABASE [$Veritabani] TO DISK = N'`$dosya' WITH INIT, CHECKSUM"
    [void]`$k.ExecuteNonQuery()
    `$k.CommandText = "RESTORE VERIFYONLY FROM DISK = N'`$dosya' WITH CHECKSUM"
    [void]`$k.ExecuteNonQuery()
    `$b.Close()
    # Panelden yüklenen fotoğraflar ve site ayarları (silinen dosyalar yedekte kalır).
    robocopy "$SiteKlasoru\App_Data" "$YedekKlasoru\App_Data" /E /R:1 /W:1 /NFL /NDL /NJH /NJS | Out-Null
    if (`$LASTEXITCODE -ge 8) { throw "App_Data kopyalanamadı (robocopy `$LASTEXITCODE)." }
    Get-ChildItem "$YedekKlasoru" -Filter "*.bak" | Where-Object { `$_.LastWriteTime -lt (Get-Date).AddDays(-14) } | Remove-Item -Force
    Add-Content `$gunluk "`$(Get-Date -Format s) TAMAM `$dosya"
} catch {
    Add-Content `$gunluk "`$(Get-Date -Format s) HATA `$(`$_.Exception.Message)"
    exit 1
}
"@
    Set-Content -Path $betik -Value $icerik -Encoding UTF8
    & icacls.exe $betik /inheritance:r /grant "*S-1-5-32-544:F" "*S-1-5-18:F" /Q | Out-Null

    $gorevAdi = "Yedek - $SiteAdi"
    $eylem = New-ScheduledTaskAction -Execute "powershell.exe" -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$betik`""
    $zaman = New-ScheduledTaskTrigger -Daily -At 3am
    # SYSTEM hesabı SID'iyle verilir (Türkçe Windows'ta hesap adı farklıdır).
    $sistem = (New-Object Security.Principal.SecurityIdentifier "S-1-5-18").Translate([Security.Principal.NTAccount]).Value
    $kimlik = New-ScheduledTaskPrincipal -UserId $sistem -LogonType ServiceAccount -RunLevel Highest
    Register-ScheduledTask -TaskName $gorevAdi -Action $eylem -Trigger $zaman -Principal $kimlik -Force | Out-Null

    # İlk yedek hemen alınır ve sonucu denetlenir.
    Start-ScheduledTask -TaskName $gorevAdi
    for ($i = 0; $i -lt 120 -and (Get-ScheduledTask -TaskName $gorevAdi).State -eq "Running"; $i++) { Start-Sleep -Seconds 1 }
    $sonuc = (Get-ScheduledTaskInfo -TaskName $gorevAdi).LastTaskResult
    $son = Get-Content (Join-Path $YedekKlasoru "yedek.log") -Tail 1 -ErrorAction SilentlyContinue
    if ($sonuc -eq 0 -and $son -like "*TAMAM*") { Tamam "İlk yedek alındı. Her gece 03.00'te tekrarlanır: $YedekKlasoru" }
    else { Uyari "İlk yedek alınamadı (sonuç $sonuc). Ayrıntı: $son"; Uyari "Günlük: $(Join-Path $YedekKlasoru 'yedek.log')" }
    Uyari "Bu yedekler aynı sunucudadır. Ara sıra bu klasörü başka bir yere (bilgisayarınıza ya da buluta) kopyalayın."
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

function SiteyiDene($sema, $alan = $AlanAdlari[0]) {
    Adim "Site denemesi ($sema, $alan)"
    try {
        # http: alan adı henüz yönlenmemiş olabilir, bu yüzden yerel adres + Host başlığı. https: sertifika alan adına bağlı.
        if ($sema -eq "https") { $istek = [Net.WebRequest]::Create("https://$alan/") }
        else { $istek = [Net.WebRequest]::Create("http://127.0.0.1/"); $istek.Host = $alan }
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
    if (-not $Eposta) { throw "Sertifika için bir e-posta adresi gerekli:  -Islem Ssl -Eposta siz@ornek.com" }

    # Her alan adı grubu için DNS denetlenir; tüm alan adları bu sunucuya yönlenmiş gruplar sertifika alır.
    $hazir = @()
    foreach ($grup in $Gruplar) {
        Adim "DNS denetimi: $($grup[0])"
        $eksik = @()
        foreach ($alan in $grup) {
            # Genel DNS sunucuları eski kaydı bir süre önbellekte tutar; Let's Encrypt ise alan adının kendi
            # DNS'ine bakar. Üç genel DNS'ten herhangi biri yeni adresi görüyorsa kayıt değişmiş demektir.
            $kayitlar = @(foreach ($dns in "1.1.1.1", "8.8.8.8", "9.9.9.9") {
                Resolve-DnsName -Name $alan -Type A -Server $dns -DnsOnly -ErrorAction SilentlyContinue | Where-Object { $_.Type -eq "A" } | ForEach-Object { $_.IPAddress }
            }) | Select-Object -Unique
            if ($kayitlar -contains $SunucuIp) { Tamam "$alan -> $SunucuIp" }
            else { Uyari "$alan henüz bu sunucuya yönlenmiyor (bulunan: $($kayitlar -join ', '))"; $eksik += $alan }
        }
        if ($eksik) { Uyari "Bu grup atlandı. DNS yayılınca betiği yeniden çalıştırın." } else { $hazir += , $grup }
    }
    if (-not $hazir) { throw "Hiçbir alan adı grubu bu sunucuya ($SunucuIp) yönlenmiyor. DNS kayıtlarını girip yayılmasını bekleyin, sonra yeniden deneyin." }

    Adim "SSL sertifikası (Let's Encrypt, win-acme)"
    Import-Module WebAdministration
    $wacs = Join-Path $WinAcmeKlasoru "wacs.exe"
    if (-not (Test-Path $wacs)) {
        $zip = Indir $WinAcme "win-acme.zip"
        New-Item -ItemType Directory -Force $WinAcmeKlasoru | Out-Null
        Expand-Archive -Path $zip -DestinationPath $WinAcmeKlasoru -Force
    }
    $siteNo = (Get-Website -Name $SiteAdi).Id
    $basarisiz = @()
    foreach ($grup in $hazir) {
        Write-Host ""
        if (Get-WebBinding -Name $SiteAdi -Protocol https -HostHeader $grup[0]) {
            Tamam "$($grup[0]) için https bağlantısı zaten var, atlandı."
            continue
        }
        Write-Host "   Sertifika: $($grup -join ', ')"
        & $wacs --source iis --siteid $siteNo --host ($grup -join ",") --friendlyname "$SiteAdi - $($grup[0])" --validation selfhosting `
                --store certificatestore --installation iis --installationsiteid $siteNo `
                --accepttos --emailaddress $Eposta --setuptaskscheduler
        if ($LASTEXITCODE -ne 0) { Uyari "$($grup[0]) için sertifika alınamadı."; $basarisiz += $grup[0]; continue }
        Tamam "$($grup[0]) için sertifika kuruldu."
        SiteyiDene "https" $grup[0]
    }
    if ($basarisiz) {
        Uyari "win-acme hata verdi. Elle denemek için: $wacs  (menüden 'N' ile yeni sertifika oluşturun)."
        throw "Sertifika alınamayan gruplar: $($basarisiz -join ', ')"
    }
    Tamam "Yenileme için zamanlanmış görev hazır."
}

switch ($Islem) {
    "Kur" {
        IisKur
        BarindirmaPaketiKur
        if ($Veritabani) { SqlKur }
        SiteKur
        if ($Veritabani) { VeritabaniKur }
        # Ayar dosyası (şifre, bağlantı) site ilk kez başlamadan önce yazılır.
        PanelSifresiBelirle
        PaketiYukle
        GuvenlikDuvari
        # Şifre dosyasının okunması için havuz yeniden başlatılır.
        Import-Module WebAdministration
        Restart-WebAppPool -Name $SiteAdi
        SiteyiDene "http"
        # Veritabanı tabloları sitenin ilk açılışında oluşur; yedek ondan sonra zamanlanır.
        if ($Veritabani) { YedekZamanla }
        Write-Host ""
        Write-Host "Kurulum tamamlandı. Sıradaki adımlar:" -ForegroundColor Cyan
        Write-Host "  1. İsimtescil'de şu alan adlarının @ ve www A kayıtlarını $SunucuIp adresine yönlendirin:"
        foreach ($grup in $Siteler[$Site].Gruplar) { Write-Host "       $($grup -join ', ')" }
        Write-Host "  2. Kayıtlar yayılınca: .\sunucu-kurulum.ps1 -Site $Site -Islem Ssl -Eposta siz@ornek.com"
        Write-Host "  3. https://$($AlanAdlari[0])/panel adresinden iletişim, WhatsApp ve SMTP ayarlarını girin."
        if ($Veritabani) { Write-Host "  4. Panelde turları, tarihleri ve kontenjanları girin (site boş başlar)." }
    }
    "Guncelle" {
        PaketiYukle
        SiteyiDene "http"
    }
    "Ssl" { SslKur }
}
