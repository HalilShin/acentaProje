# Bir sitenin yayın paketini üretir (geliştirme bilgisayarında çalıştırılır).
#   pwsh araclar/yayin-paketi.ps1            -> publish/sunucu/locktech.zip
#   pwsh araclar/yayin-paketi.ps1 -Proje Tasimacilik
# Paketle birlikte sunucu kurulum betiği de publish/sunucu klasörüne kopyalanır;
# bu klasörün içeriği uzak masaüstüyle sunucuda C:\kurulum klasörüne kopyalanır.
param([ValidateSet("Locktech", "Tasimacilik")][string]$Proje = "Locktech")

$ErrorActionPreference = "Stop"
$kok = Split-Path $PSScriptRoot -Parent
$gecici = Join-Path $kok "publish\$Proje"
$hedef = New-Item -ItemType Directory -Force (Join-Path $kok "publish\sunucu")
$zip = Join-Path $hedef "$($Proje.ToLowerInvariant()).zip"

if (Test-Path $gecici) { Remove-Item $gecici -Recurse -Force }
dotnet publish (Join-Path $kok "src\$Proje") -c Release -o $gecici --nologo -v q
if ($LASTEXITCODE -ne 0) { throw "dotnet publish başarısız oldu." }

# Pakete girmemesi gerekenler girmiş mi? (panel şifresi, yerel ayarlar, çalışma verileri)
$yasak = Get-ChildItem $gecici -Recurse -Force | Where-Object { $_.Name -like "appsettings.*.local.json" -or $_.Name -eq "appsettings.Development.json" -or $_.FullName -match "\\App_Data(\\|$)" }
if ($yasak) { throw "Pakette olmaması gereken dosyalar var: $($yasak.FullName -join ', ')" }

if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path (Join-Path $gecici "*") -DestinationPath $zip
Copy-Item (Join-Path $PSScriptRoot "sunucu-kurulum.ps1") $hedef -Force

"Paket hazır: {0}  ({1:N1} MB, {2} dosya)" -f $zip, ((Get-Item $zip).Length / 1MB), (Get-ChildItem $gecici -Recurse -File).Count
"Sunucuya kopyalanacak klasör: $hedef"
