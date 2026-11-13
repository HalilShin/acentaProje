using Locktech.Urunler;

namespace Locktech.Cozumler;

public record Cozum
{
    public required string KodTr { get; init; }
    public required string KodEn { get; init; }
    /// <summary>Layout'taki SVG simgesinin adı.</summary>
    public required string Ikon { get; init; }
    public required Metin Ad { get; init; }
    public required Metin Ozet { get; init; }
    public required Metin Baslik { get; init; }
    public required Metin Giris { get; init; }
    public required Metin[] Sorunlar { get; init; }
    public required (Metin Baslik, Metin Aciklama)[] Faydalar { get; init; }
    public required string[] UrunKodlari { get; init; }

    public string Kod => Dil.En ? KodEn : KodTr;
    public string Adres => Yollar.Cozum(Kod);
    public IEnumerable<Urun> OnerilenUrunler => UrunKodlari.Select(k => UrunKatalogu.Bul(k)!);
}

/// <summary>Çözümler: Locktech ürünlerinin kullanım alanına göre anlatımı.</summary>
public static class CozumKatalogu
{
    public static readonly IReadOnlyList<Cozum> Tumu =
    [
        new Cozum
        {
            KodTr = "konteyner",
            KodEn = "container",
            Ikon = "box",
            Ad = ("Konteyner", "Container"),
            Ozet = ("Limandan depoya kadar kapı mührü ve konum takibi tek cihazda.", "Door seal and location tracking in one device, from port to warehouse."),
            Baslik = ("Konteyneriniz limandan depoya kadar gözünüzün önünde", "Keep your container in sight from port to warehouse"),
            Giris = ("Locktech elektronik kilitleri konteyner kapısını mühürler, yol boyunca konumunu bildirir ve yalnızca yetkili kişiye açılır. Klasik mührün yapamadığını yapar: kapının ne zaman, nerede ve kim tarafından açıldığını gösterir.",
                     "Locktech electronic locks seal the container door, report its location along the route and open only for authorised people. They do what a conventional seal cannot: show when, where and by whom the door was opened."),
            Sorunlar =
            [
                ("Klasik mühür yalnızca koparıldığını gösterir; ne zaman ve nerede açıldığını söylemez.", "A conventional seal only shows that it was broken, not when or where."),
                ("Konteyner limanda, aktarmada ya da yolda beklerken kimse başında değildir.", "Nobody watches the container while it waits at the port, in transshipment or on the road."),
                ("Uzun yolculuklarda takip cihazının bataryası yolun ortasında bitebilir.", "On long journeys a tracker's battery can run out halfway."),
            ],
            Faydalar =
            [
                (("Mühür ve takip tek cihazda", "Seal and tracker in one"), ("Çelik halatlı kilit kapıyı kapatır; 4G ve GNSS ile konumunu ve kilit durumunu merkeze bildirir.", "The steel-cable lock secures the door and reports its location and lock status over 4G and GNSS.")),
                (("Yalnızca yetkiliyle açılır", "Opens only with authorisation"), ("Kilit; tanımlı RFID kartla, Bluetooth üzerinden mobil uygulamayla ya da 4G üzerinden uzaktan açılır.", "The lock opens with a registered RFID card, from the mobile app over Bluetooth, or remotely over 4G.")),
                (("Uzun yol için güneş enerjisi", "Solar power for long hauls"), ("HB-A1SP'nin 15.200 mAh bataryası güneş paneliyle şarj olur; güneş olmadan da 31 gün bekleme süresi sunar.", "The HB-A1SP's 15,200 mAh battery charges from its solar panel and still offers 31 days of standby without sun.")),
                (("Yetkisiz müdahalede alarm", "Alarm on tampering"), ("Akıllı alarm sistemi, kilide müdahale edildiğinde anında haber verir.", "The smart alarm system notifies you immediately when the lock is tampered with.")),
            ],
            UrunKodlari = ["hb-a1sp", "hb-a1m", "hb-a1ms"],
        },
        new Cozum
        {
            KodTr = "tanker",
            KodEn = "tanker",
            Ikon = "tanker",
            Ad = ("Tanker", "Tanker"),
            Ozet = ("Dolum ve boşaltma kapaklarında ATEX uyumlu, zincirleme kilitleme.", "ATEX-rated, cascaded locking for filling and discharge points."),
            Baslik = ("Tankerde her kapak ve vana kontrol altında", "Every hatch and valve on the tanker under control"),
            Giris = ("Akaryakıt ve kimyasal taşımacılığında kilit, patlayıcı ortama uygun olmak zorundadır. Locktech'in ATEX sertifikalı kilitleri dolum kapaklarını ve boşaltma vanalarını güvenceye alır; ana kilide bağlı ikincil kilitlerle tankerin tüm açılma noktaları birlikte yönetilir.",
                     "In fuel and chemical transport a lock has to be suitable for explosive atmospheres. Locktech's ATEX-certified locks secure filling hatches and discharge valves, and secondary locks linked to the main lock let you manage every opening on the tanker together."),
            Sorunlar =
            [
                ("Bir tankerde tek değil, birden çok açılma noktası vardır: dolum kapakları, boşaltma vanaları, yakıt deposu.", "A tanker has not one but several openings: filling hatches, discharge valves, the fuel tank."),
                ("Yolda yapılan yetkisiz boşaltma ancak teslimatta, eksik ürünle fark edilir.", "Unauthorised discharge on the road is only noticed at delivery, when product is missing."),
                ("Patlayıcı ortamda sıradan elektronik cihaz kullanılamaz.", "Ordinary electronics cannot be used in an explosive atmosphere."),
            ],
            Faydalar =
            [
                (("ATEX sertifikalı gövde", "ATEX-certified housing"), ("Kilitler patlamaya dayanıklı (Ex-proof) gövdeye sahiptir ve patlayıcı ortamlarda çalışmaya uygundur.", "The locks have an explosion-proof (Ex-proof) housing and are suitable for explosive atmospheres.")),
                (("Çoklu kilit zincirleme", "Multi-lock cascading"), ("Cascade özelliğiyle ana kilit ve ona Bluetooth üzerinden bağlı HB-A1Ms ikincil kilitler birlikte çalışır.", "With cascading, the main lock and HB-A1Ms secondary locks linked to it over Bluetooth work together.")),
                (("Yetkiyle açılan kapaklar", "Hatches that open with authorisation"), ("Kapaklar yalnızca tanımlı kartla, mobil uygulamayla ya da merkezden verilen komutla açılır.", "Hatches open only with a registered card, the mobile app, or a command from the control centre.")),
                (("Sahaya dayanıklı", "Built for the field"), ("IP67 su geçirmez gövde ve 1.000 kg kilit mili dayanımı, yol ve dolum sahası koşulları için tasarlanmıştır.", "The IP67 waterproof housing and 1,000 kg shackle strength are designed for road and loading-bay conditions.")),
            ],
            UrunKodlari = ["hb-a1m", "hb-a1ms", "hb-a1sp"],
        },
        new Cozum
        {
            KodTr = "lojistik-araci",
            KodEn = "logistics-vehicle",
            Ikon = "truck",
            Ad = ("Lojistik Aracı", "Logistics Vehicle"),
            Ozet = ("Kasa kapısı kilidi ve araç içi video telematik ile sürüş güvenliği.", "Cargo door lock plus in-vehicle video telematics for safer driving."),
            Baslik = ("Kasa kapısından sürücü koltuğuna tam görünürlük", "Full visibility from the cargo door to the driver's seat"),
            Giris = ("Kamyon ve tır filolarında iki soru önemlidir: yük güvende mi, araç güvenli sürülüyor mu? Locktech kasa kapısını akıllı kilitle, sürüşü video telematikle izler.",
                     "For truck fleets two questions matter: is the load safe, and is the vehicle being driven safely? Locktech watches the cargo door with a smart lock and the driving with video telematics."),
            Sorunlar =
            [
                ("Dağıtım rotasında kasa kapısı gün içinde defalarca açılır; hangisinin yetkili olduğu bilinmez.", "On a delivery route the cargo door opens many times a day, and you cannot tell which openings were authorised."),
                ("Kaza ya da şikâyet anında neyin yaşandığını gösteren bir kayıt yoktur.", "When there is an accident or a complaint, there is no record of what actually happened."),
                ("Yorgunluk ve dikkatsizlik gibi sürücü kaynaklı riskler ancak iş işten geçtikten sonra görülür.", "Driver risks such as fatigue and distraction are only seen when it is too late."),
            ],
            Faydalar =
            [
                (("Kasa kapısında akıllı kilit", "Smart lock on the cargo door"), ("HB-A1M kompakt gövdesiyle kasa kapısına takılır; konumu bildirir ve yalnızca yetkiliyle açılır.", "The compact HB-A1M fits on the cargo door, reports its location and opens only with authorisation.")),
                (("4 kanallı video kayıt", "4-channel video recording"), ("HB-VT08 aracın çevresini ve içini dört kameradan izler; 256 ya da 512 GB dahili depolamaya kaydeder.", "The HB-VT08 monitors around and inside the vehicle from four cameras and records to 256 or 512 GB of built-in storage.")),
                (("ADAS ve DSM", "ADAS and DSM"), ("Sürüş destek (ADAS) ve sürücü izleme (DSM) kameraları ile trafik işareti algılama, riskli durumları sürüş sırasında yakalar.", "Driver assistance (ADAS) and driver monitoring (DSM) cameras, together with traffic sign detection, catch risky situations while driving.")),
                (("Kablosuz kilit montajı", "No wiring for the lock"), ("Kilit mıknatısla yapışır ve kendi bataryasıyla çalışır; araçta kablolama gerektirmez.", "The lock attaches magnetically and runs on its own battery, with no wiring in the vehicle.")),
            ],
            UrunKodlari = ["hb-a1m", "hb-vt08", "hb-a1ms"],
        },
        new Cozum
        {
            KodTr = "kasa",
            KodEn = "safe",
            Ikon = "safe",
            Ad = ("Kasa", "Safe"),
            Ozet = ("Çelik kasalarda yetkili kişi dışında açılmayan, konumu izlenen kilit.", "A tracked lock for steel safes that opens only for authorised people."),
            Baslik = ("Değerli yük kasası yalnızca doğru kişiye açılsın", "A valuables safe that opens only for the right person"),
            Giris = ("Taşınan çelik kasalarda anahtar, kopyalanabilen ve elden ele geçen bir zayıf noktadır. Locktech kilitleri anahtarın yerini kişiye tanımlı kart, mobil uygulama ve merkezden verilen komutla değiştirir.",
                     "For steel safes in transit the key is a weak point that can be copied and passed around. Locktech locks replace the key with a personal card, the mobile app and commands from the control centre."),
            Sorunlar =
            [
                ("Anahtar kopyalanabilir ve kimin elinde olduğu takip edilemez.", "Keys can be copied, and you cannot track who is holding one."),
                ("Kasa yoldayken nerede olduğu bilinmez.", "While the safe is in transit, its location is unknown."),
                ("Zorlama girişimi ancak teslimatta fark edilir.", "A forced-entry attempt is only noticed at delivery."),
            ],
            Faydalar =
            [
                (("Anahtarsız, yetkiyle açma", "Keyless, authorised opening"), ("Kasa; tanımlı RFID kartla, Bluetooth üzerinden mobil uygulamayla ya da 4G üzerinden uzaktan açılır.", "The safe opens with a registered RFID card, from the mobile app over Bluetooth, or remotely over 4G.")),
                (("Yol boyunca konum", "Location along the route"), ("Kilit, GNSS konumunu 4G üzerinden bildirir; kasanın nerede olduğunu her an görürsünüz.", "The lock reports its GNSS position over 4G, so you can see where the safe is at any time.")),
                (("Akıllı alarm", "Smart alarm"), ("Kilide müdahale edildiğinde akıllı alarm sistemi anında haber verir.", "The smart alarm system notifies you immediately when the lock is tampered with.")),
                (("Küçük ve hafif", "Small and light"), ("544 gramlık HB-A1M, 124 × 95 × 47 mm boyutuyla kasa üzerinde yer kaplamaz.", "At 544 g and 124 × 95 × 47 mm, the HB-A1M takes up very little room on the safe.")),
            ],
            UrunKodlari = ["hb-a1m", "hb-a1ms"],
        },
        new Cozum
        {
            KodTr = "gumruk-kargo-takibi",
            KodEn = "customs-cargo-tracking",
            Ikon = "customs",
            Ad = ("Gümrük Kargo Takibi", "Customs Cargo Tracking"),
            Ozet = ("Transit yüklerde güzergâh boyunca kesintisiz izleme ve yetkili açma.", "Continuous monitoring and authorised opening for transit cargo."),
            Baslik = ("Transit yük, çıkıştan varışa kadar mühürlü ve izlenebilir", "Transit cargo, sealed and traceable from departure to arrival"),
            Giris = ("Gümrük gözetimindeki yüklerde aracın güzergâhta kalması ve yükün varış noktasına kadar açılmaması gerekir. Locktech elektronik kilitleri yükü mühürler, yol boyunca konumunu bildirir ve müdahalede alarm verir.",
                     "Cargo under customs supervision has to stay on its route and remain unopened until its destination. Locktech electronic locks seal the load, report its location along the way and raise an alarm on tampering."),
            Sorunlar =
            [
                ("Transit yükün varış noktasına kadar açılmadığını göstermek gerekir.", "You need to show that transit cargo stayed unopened until its destination."),
                ("Sınır kapıları ve uzun güzergâhlar günlerce süren bekleme demektir.", "Border crossings and long routes mean days of waiting."),
                ("Aracın güzergâh boyunca nerede olduğu anlık olarak görülemez.", "The vehicle's position along the route cannot be seen in real time."),
            ],
            Faydalar =
            [
                (("Elektronik mühür", "Electronic seal"), ("Kilit yalnızca yetkili kartla, mobil uygulamayla ya da merkezden verilen komutla açılır.", "The lock opens only with an authorised card, the mobile app, or a command from the control centre.")),
                (("Güzergâh boyunca konum", "Location along the route"), ("4G ve GNSS ile aracın konumu yol boyunca merkeze bildirilir.", "The vehicle's position is reported to the control centre along the route over 4G and GNSS.")),
                (("Uzun bekleme süresi", "Long standby time"), ("HB-A1SP güneş paneliyle şarj olur ve güneş olmadan 31 gün bekleme süresi sunar; sınır beklemelerinde şarj derdi olmaz.", "The HB-A1SP charges from its solar panel and offers 31 days of standby without sun, so border waits are not a charging problem.")),
                (("Müdahalede alarm", "Alarm on tampering"), ("Akıllı alarm sistemi, kilide müdahale edildiğinde anında haber verir.", "The smart alarm system notifies you immediately when the lock is tampered with.")),
            ],
            UrunKodlari = ["hb-a1sp", "hb-a1m"],
        },
    ];

    /// <summary>Geçerli dildeki adres koduna göre çözümü bulur.</summary>
    public static Cozum? Bul(string? kod) =>
        Tumu.FirstOrDefault(c => string.Equals(c.Kod, kod, StringComparison.OrdinalIgnoreCase));
}
