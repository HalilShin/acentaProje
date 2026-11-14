namespace Locktech.Urunler;

public record TeknikGrup(Metin Baslik, (Metin Ad, Metin Deger)[] Satirlar);

public record Urun
{
    /// <summary>Adres çubuğundaki ad: /urunler/hb-a1sp</summary>
    public required string Kod { get; init; }
    public required string Model { get; init; }
    public required Metin Ad { get; init; }
    public required Metin Slogan { get; init; }
    public required Metin Ozet { get; init; }
    public required Metin Hakkinda { get; init; }
    public required Metin Etiket { get; init; }
    public bool EtiketVurgulu { get; init; }
    public required string Gorsel { get; init; }
    /// <summary>Teklif formundaki seçenek metni.</summary>
    public required Metin FormSecenegi { get; init; }
    public required Metin[] Baglanti { get; init; }
    public required Metin[] KartMaddeleri { get; init; }
    public required Metin[] OneCikanlar { get; init; }
    public required Metin[] KullanimAlanlari { get; init; }
    public required TeknikGrup[] Teknik { get; init; }
    public required (string Ad, Metin Aciklama)[] Sertifikalar { get; init; }

    public string Adres => Yollar.Urun(Kod);
    /// <summary>Locktech tasarımlı ürün PDF'i (geçerli dilde).</summary>
    public string Pdf => $"/pdf/Locktech-{Model}-{(Dil.En ? "en" : "tr")}.pdf";
}

/// <summary>Sitede satılan dört ürün. Veriler Türkçe föylerden alınmıştır.</summary>
public static class UrunKatalogu
{
    private static readonly (string, Metin) Ce = ("CE", ("Güvenlik ve çevre koruma standartlarına uygundur.", "Complies with safety and environmental protection standards."));
    private static readonly (string, Metin) Atex = ("ATEX", ("Patlayıcı ortamlarda çalışmaya uygundur.", "Suitable for use in explosive atmospheres."));
    private static readonly (string, Metin) Ip67 = ("IP67", ("Toz ve suya karşı dayanıklılık seviyesi standartlarına uygundur.", "Rated for protection against dust and water ingress."));

    private static readonly Metin[] KilitAlanlari =
    [
        ("Konteynerler", "Containers"), ("Tankerler", "Tankers"), ("Lojistik araçları", "Logistics vehicles"),
        ("Çelik kasalar", "Steel safes"), ("Yakıt depo kapakları", "Fuel tank caps"),
    ];

    private static readonly Metin Fiziksel = ("Fiziksel özellikler", "Physical");
    private static readonly Metin Gnss = ("GNSS konumlandırma", "GNSS positioning");
    private static readonly Metin Iletisim = ("İletişim", "Connectivity");
    private static readonly Metin AcmaModlari = ("Kilit açma modları", "Unlocking modes");
    private static readonly Metin Guc = ("Güç / batarya", "Power / battery");
    private static readonly Metin Ortam = ("Çalışma ortamı", "Operating environment");

    private static readonly Metin Boyut = ("Boyut", "Dimensions");
    private static readonly Metin Agirlik = ("Ağırlık", "Weight");
    private static readonly Metin MilDayanimi = ("Kilit mili dayanımı", "Shackle strength");
    private static readonly Metin Hassasiyet = ("İzleme hassasiyeti", "Tracking accuracy");
    private static readonly Metin Baslatma = ("Başlatma süreleri", "Time to first fix");
    private static readonly Metin BaglantiTeknolojisi = ("Bağlantı teknolojisi", "Network");
    private static readonly Metin VeriDestegi = ("Veri desteği", "Data protocols");
    private static readonly Metin Rfid = ("RFID (radyo frekansı)", "RFID");
    private static readonly Metin BleMesafe = ("BLE 4.0 (iletişim mesafesi 20 m)", "BLE 4.0 (range 20 m)");
    private static readonly Metin SinirYok = ("Mesafe sınırı yok", "No distance limit");
    private static readonly Metin AnaBatarya = ("Ana batarya", "Main battery");
    private static readonly Metin UsbSarj = ("USB şarj voltajı", "USB charging");
    private static readonly Metin SarjSuresi = ("Şarj süresi", "Charging time");
    private static readonly Metin BeklemeSuresi = ("Bekleme süresi", "Standby time");
    private static readonly Metin CalismaDepolama = ("Çalışma / depolama", "Operating / storage");
    private static readonly Metin SarjSicakligi = ("Şarj sıcaklığı", "Charging temperature");
    private static readonly Metin Montaj = ("Montaj", "Mounting");
    private static readonly Metin Miknatis = ("Mıknatıs ile yapışma", "Magnetic attachment");

    public static readonly IReadOnlyList<Urun> Tumu =
    [
        new Urun
        {
            Kod = "hb-a1sp",
            Model = "HB-A1SP",
            Ad = ("4G Güneş Enerjili Elektronik Kilit", "4G Solar-Powered Electronic Lock"),
            Slogan = ("Güneş enerjisiyle uzun süreli, güvenli ve akıllı kilit sistemi", "Long-lasting, secure and smart locking, powered by the sun"),
            Ozet = ("Güneş paneliyle şarj olan, 4G bağlantılı, patlamaya dayanıklı elektronik kilit. Konum takibi, uzaktan açma ve çoklu kilit zincirleme.",
                    "Solar-charged, 4G-connected, explosion-proof electronic lock with location tracking, remote unlocking and multi-lock cascading."),
            Hakkinda = ("HB-A1SP, lojistik ve endüstriyel uygulamalar için geliştirilmiş, 4G bağlantılı, güneş enerjisiyle şarj olabilen, patlamaya dayanıklı (ATEX) elektronik kilit sistemidir. Uzaktan izleme, konum takibi, çoklu kilit zincirleme (Cascade) ve akıllı alarm fonksiyonlarıyla yüksek güvenlik ve kontrol sağlar.",
                        "HB-A1SP is a 4G-connected, solar-rechargeable, explosion-proof (ATEX) electronic lock built for logistics and industrial use. Remote monitoring, location tracking, multi-lock cascading and smart alarms give you a high level of security and control."),
            Etiket = ("GÜNEŞ ENERJİLİ", "SOLAR-POWERED"),
            EtiketVurgulu = true,
            Gorsel = "/img/hb-a1sp.jpg",
            FormSecenegi = ("HB-A1SP — Güneş enerjili kilit", "HB-A1SP — Solar-powered lock"),
            Baglanti = ["4G", "GNSS", "Bluetooth", "RFID"],
            KartMaddeleri =
            [
                ("15.200 mAh batarya, güneş paneliyle şarj", "15,200 mAh battery with solar charging"),
                ("RFID kart, Bluetooth ve 4G uzaktan açma", "Unlock by RFID card, Bluetooth or 4G remote"),
                ("IP67, patlamaya dayanıklı (Ex) gövde", "IP67, explosion-proof (Ex) housing"),
            ],
            OneCikanlar =
            [
                ("Süper uzun bekleme süresi", "Extra-long standby time"),
                ("RFID kart ile kilit açma", "Unlocking by RFID card"),
                ("Bluetooth mobil uygulama ile kilit açma", "Unlocking from the mobile app over Bluetooth"),
                ("4G üzerinden uzaktan kilit açma", "Remote unlocking over 4G"),
                ("Çoklu kilit zincirleme çalışma (Cascade)", "Multi-lock cascading"),
                ("IP67 su geçirmezlik", "IP67 waterproof"),
                ("Akıllı alarm sistemi", "Smart alarm system"),
                ("Patlamaya dayanıklı (Ex) gövde", "Explosion-proof (Ex) housing"),
                ("Güneş paneli ile şarj (isteğe bağlı)", "Solar panel charging (optional)"),
            ],
            KullanimAlanlari = KilitAlanlari,
            Teknik =
            [
                new(Fiziksel,
                [
                    (Boyut, "209 × 115 × 51 mm"),
                    (("Halat uzunluğu", "Cable length"), "300 mm"),
                    (Agirlik, ("1.150 g", "1,150 g")),
                    (("Gövde", "Housing"), ("Yüksek ısıya dayanıklı PC", "Heat-resistant PC")),
                    (MilDayanimi, ("1.000 kg", "1,000 kg")),
                    (("Koruma sınıfı", "Protection rating"), "IP67"),
                ]),
                new(Gnss,
                [
                    (("Uydu sistemleri", "Satellite systems"), ("GPS, BeiDou (64 bağımsız sinyal)", "GPS, BeiDou (64 independent signals)")),
                    (Hassasiyet, "1 – 10 m"),
                    (Baslatma, ("Sıcak 1 sn · Ilık 13 sn · Soğuk 32 sn", "Hot 1 s · Warm 13 s · Cold 32 s")),
                ]),
                new(Iletisim,
                [
                    (BaglantiTeknolojisi, "LTE FDD / WCDMA / GSM"),
                    (VeriDestegi, "TCP / FTP / SMS"),
                ]),
                new(AcmaModlari,
                [
                    (Rfid, ("2,5 cm", "2.5 cm")),
                    ("Bluetooth", BleMesafe),
                    ("GPRS / SMS", SinirYok),
                ]),
                new(Guc,
                [
                    (AnaBatarya, ("15.200 mAh Li-ion · 3,7 V · 55,5 Wh", "15,200 mAh Li-ion · 3.7 V · 55.5 Wh")),
                    (UsbSarj, "5 V DC 2 A"),
                    (("Güneş şarj akımı", "Solar charging current"), ("250 mA/saat", "250 mA/h")),
                    (SarjSuresi, ("10 saat", "10 hours")),
                    (BeklemeSuresi, ("Güneş şarjı olmadan 31 gün (10 dakikada bir veri ile)", "31 days without solar charging (reporting every 10 minutes)")),
                ]),
                new(Ortam,
                [
                    (CalismaDepolama, ("−40 °C ile +85 °C", "−40 °C to +85 °C")),
                    (SarjSicakligi, ("0 °C ile +45 °C", "0 °C to +45 °C")),
                    (("Nem", "Humidity"), ("%0 – %100 (yoğuşmasız)", "0 – 100% (non-condensing)")),
                    (Montaj, Miknatis),
                ]),
            ],
            Sertifikalar = [Ce, Atex],
        },
        new Urun
        {
            Kod = "hb-a1m",
            Model = "HB-A1M",
            Ad = ("4G Patlamaya Dayanıklı Elektronik Kilit", "4G Explosion-Proof Electronic Lock"),
            Slogan = ("Kompakt, güvenli ve akıllı kilit sistemi", "A compact, secure and smart lock"),
            Ozet = ("Küçük boyutlu ve hafif, 4G bağlantılı, patlamaya dayanıklı elektronik kilit. Uzun bekleme süresi ve akıllı alarm.",
                    "Small, lightweight, 4G-connected explosion-proof electronic lock with long standby time and smart alarms."),
            Hakkinda = ("HB-A1M, lojistik ve endüstriyel uygulamalar için geliştirilmiş, 4G bağlantılı, patlamaya dayanıklı (ATEX) elektronik kilit sistemidir. Kompakt boyutu, hafif yapısı, uzun bekleme süresi ve akıllı alarm fonksiyonlarıyla yüksek güvenlik ve kontrol sağlar.",
                        "HB-A1M is a 4G-connected, explosion-proof (ATEX) electronic lock built for logistics and industrial use. Its compact size, light weight, long standby time and smart alarms give you a high level of security and control."),
            Etiket = ("KOMPAKT", "COMPACT"),
            Gorsel = "/img/hb-a1m.jpg",
            FormSecenegi = ("HB-A1M — Patlamaya dayanıklı kilit", "HB-A1M — Explosion-proof lock"),
            Baglanti = ["4G", "GNSS", "Bluetooth", "RFID"],
            KartMaddeleri =
            [
                "544 g, 124 × 95 × 47 mm",
                ("20 gün bekleme süresi", "20 days standby time"),
                ("Cascade ile çoklu kilit zincirleme", "Multi-lock cascading"),
            ],
            OneCikanlar =
            [
                ("Süper uzun bekleme süresi (20 gün)", "Extra-long standby time (20 days)"),
                ("Küçük boyut ve hafif tasarım", "Small and lightweight design"),
                ("Çoklu kilit zincirleme çalışma (Cascade)", "Multi-lock cascading"),
                ("IP67 su geçirmezlik", "IP67 waterproof"),
                ("Akıllı alarm sistemi ve Ex-proof gövde", "Smart alarm system and Ex-proof housing"),
            ],
            KullanimAlanlari = KilitAlanlari,
            Teknik =
            [
                new(Fiziksel,
                [
                    (Boyut, "124 × 95 × 47 mm"),
                    (Agirlik, "544 g"),
                    (MilDayanimi, ("1.000 kg", "1,000 kg")),
                ]),
                new(Gnss,
                [
                    (("Sinyal alma / izleme", "Acquisition / tracking sensitivity"), "−147 dBm / −166 dBm"),
                    (Hassasiyet, ("1,5 m", "1.5 m")),
                    (Baslatma, ("Sıcak 1 sn · Ilık 25 sn · Soğuk 28 sn", "Hot 1 s · Warm 25 s · Cold 28 s")),
                ]),
                new(Iletisim,
                [
                    (BaglantiTeknolojisi, "4G LTE / WCDMA / GSM"),
                    (VeriDestegi, "TCP / FTP / SMS"),
                ]),
                new(AcmaModlari,
                [
                    (Rfid, ("2,5 cm", "2.5 cm")),
                    ("Bluetooth", BleMesafe),
                    ("GPRS / SMS", SinirYok),
                ]),
                new(Guc,
                [
                    (AnaBatarya, ("2.950 mAh Li-ion · 3,7 V · 10,9 Wh", "2,950 mAh Li-ion · 3.7 V · 10.9 Wh")),
                    (UsbSarj, "5 V DC 2 A"),
                    (SarjSuresi, ("6 saat", "6 hours")),
                    (BeklemeSuresi, ("20 gün (10 dakikada bir veri ile)", "20 days (reporting every 10 minutes)")),
                ]),
                new(Ortam,
                [
                    (CalismaDepolama, ("−30 °C ile +70 °C", "−30 °C to +70 °C")),
                    (SarjSicakligi, ("0 °C ile +45 °C", "0 °C to +45 °C")),
                    (Montaj, Miknatis),
                ]),
            ],
            Sertifikalar = [Ce, Atex, Ip67],
        },
        new Urun
        {
            Kod = "hb-a1ms",
            Model = "HB-A1Ms",
            Ad = ("4G Patlamaya Dayanıklı İkincil Kilit (Sub-Lock)", "4G Explosion-Proof Secondary Lock (Sub-Lock)"),
            Slogan = ("Güvenli, akıllı ve yedek kilit çözümü", "A secure, smart backup lock"),
            Ozet = ("Ana kilitle Bluetooth üzerinden haberleşen ikincil kilit. 90 gün bekleme süresi, kompakt ve Ex-proof gövde.",
                    "Secondary lock that talks to the main lock over Bluetooth. 90 days standby, compact Ex-proof housing."),
            Hakkinda = ("HB-A1Ms, ana kilitle Bluetooth üzerinden haberleşen, 4G bağlantılı, patlamaya dayanıklı ikincil kilit (Sub-Lock) sistemidir. Kompakt ve hafif yapısı, uzun bekleme süresi ve Ex-proof korumasıyla zorlu endüstriyel koşullarda güvenli ve akıllı bir yedek kilit çözümü sunar.",
                        "HB-A1Ms is a 4G-connected, explosion-proof secondary lock (sub-lock) that communicates with the main lock over Bluetooth. Compact and light, with long standby time and Ex-proof protection, it is a secure, smart backup lock for demanding industrial conditions."),
            Etiket = ("İKİNCİL KİLİT", "SUB-LOCK"),
            Gorsel = "/img/hb-a1ms.jpg",
            FormSecenegi = ("HB-A1Ms — İkincil kilit", "HB-A1Ms — Secondary lock"),
            Baglanti = ["4G", "Bluetooth", "RFID"],
            KartMaddeleri =
            [
                ("Ana kilitle Bluetooth üzerinden haberleşir", "Talks to the main lock over Bluetooth"),
                ("90 gün bekleme süresi", "90 days standby time"),
                ("494 g, IP67, Ex-proof koruma", "494 g, IP67, Ex-proof protection"),
            ],
            OneCikanlar =
            [
                ("Süper uzun bekleme süresi (90 gün)", "Extra-long standby time (90 days)"),
                ("Ana kilit ile Bluetooth üzerinden haberleşme", "Communicates with the main lock over Bluetooth"),
                ("Küçük boyut, hafif yapı", "Small and lightweight"),
                ("IP67 su geçirmezlik", "IP67 waterproof"),
                ("Ex-proof koruma", "Ex-proof protection"),
            ],
            KullanimAlanlari = KilitAlanlari,
            Teknik =
            [
                new(("Boyutlar / ağırlık", "Dimensions / weight"),
                [
                    (Boyut, "124 × 95 × 47 mm"),
                    (Agirlik, "494 g"),
                ]),
                new(AcmaModlari,
                [
                    ("RFID", ("2,5 cm", "2.5 cm")),
                    ("Bluetooth", "4.0 BLE (20 m)"),
                ]),
                new(("Batarya", "Battery"),
                [
                    (("Batarya", "Battery"), ("2.950 mAh Li-ion · 3,7 V · 10,9 Wh", "2,950 mAh Li-ion · 3.7 V · 10.9 Wh")),
                    (BeklemeSuresi, ("3 ay (10 dakikada bir veri ile)", "3 months (reporting every 10 minutes)")),
                ]),
                new(("Şarj", "Charging"),
                [
                    (("Şarj voltajı", "Charging voltage"), "5 V DC 2 A"),
                    (SarjSuresi, ("3 saat", "3 hours")),
                ]),
            ],
            Sertifikalar = [Ce, Atex, Ip67],
        },
        new Urun
        {
            Kod = "hb-vt08",
            Model = "HB-VT08",
            Ad = ("Video Telematik Sistemi", "Video Telematics System"),
            Slogan = ("Araç filosu için akıllı video izleme çözümü", "Smart video monitoring for vehicle fleets"),
            Ozet = ("4 kanallı video izleme, ADAS ve DSM desteğiyle filo güvenliği, sürücü davranış analizi ve yol güvenliği.",
                    "4-channel video monitoring with ADAS and DSM for fleet safety, driver behaviour analysis and road safety."),
            Hakkinda = ("HB-VT08; 4 kanallı akıllı video izleme, ADAS, DSM ve yolcu izleme fonksiyonlarını bir araya getiren gelişmiş bir video telematik sistemidir. Filo güvenliği, sürücü davranış analizi ve yol güvenliği için kapsamlı ve etkin bir çözüm sunar.",
                        "HB-VT08 is an advanced video telematics system combining 4-channel smart video monitoring, ADAS, DSM and passenger monitoring. It is a comprehensive solution for fleet safety, driver behaviour analysis and road safety."),
            Etiket = ("FİLO", "FLEET"),
            Gorsel = "/img/hb-vt08.jpg",
            FormSecenegi = ("HB-VT08 — Video telematik", "HB-VT08 — Video telematics"),
            Baglanti = ["4G", "GPS", ("Video izleme", "Video monitoring")],
            KartMaddeleri =
            [
                ("4 kanallı akıllı video izleme", "4-channel smart video monitoring"),
                ("ADAS, DSM ve yolcu izleme kameraları", "ADAS, DSM and passenger monitoring cameras"),
                ("256 / 512 GB dahili depolama", "256 / 512 GB built-in storage"),
            ],
            OneCikanlar =
            [
                ("4 kanallı akıllı video izleme", "4-channel smart video monitoring"),
                ("ADAS, DSM ve yolcu izleme kameraları", "ADAS, DSM and passenger monitoring cameras"),
                ("Hız sınırı tabela tanıma (trafik işareti algılama)", "Speed limit sign recognition (traffic sign detection)"),
                ("IP65 koruma: toza ve düşük basınçlı su püskürtmelerine dayanıklı", "IP65: protected against dust and low-pressure water jets"),
                ("Dahili yedek batarya", "Built-in backup battery"),
            ],
            KullanimAlanlari =
            [
                ("Lojistik araçları", "Logistics vehicles"), ("Yolcu otobüsleri", "Passenger coaches"), ("Tankerler", "Tankers"),
                ("Ticari araçlar", "Commercial vehicles"), ("Servis araçları", "Shuttle vehicles"),
            ],
            Teknik =
            [
                new(("Dahili depolama", "Built-in storage"),
                [
                    (("Depolama seçenekleri", "Storage options"), "256 GB / 512 GB"),
                ]),
                new(("Batarya", "Battery"),
                [
                    (("Dahili yedek batarya", "Built-in backup battery"), ("2.950 mAh", "2,950 mAh")),
                ]),
                new(("Arayüzler", "Interfaces"),
                [
                    (("Dijital giriş", "Digital inputs"), "3"),
                    (("Dijital çıkış", "Digital outputs"), "1"),
                    ("RS232", "1"),
                    (("SOS butonu", "SOS button"), "1"),
                ]),
                new(Ortam,
                [
                    (("Çalışma sıcaklığı", "Operating temperature"), ("−30 °C ile +70 °C", "−30 °C to +70 °C")),
                    (("Koruma sınıfı", "Protection rating"), "IP65"),
                ]),
            ],
            Sertifikalar = [Ce],
        },
    ];

    public static Urun? Bul(string? kod) =>
        Tumu.FirstOrDefault(u => string.Equals(u.Kod, kod, StringComparison.OrdinalIgnoreCase));
}
