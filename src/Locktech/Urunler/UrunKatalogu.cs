namespace Locktech.Urunler;

public record TeknikGrup(string Baslik, (string Ad, string Deger)[] Satirlar);

public record Urun
{
    /// <summary>Adres çubuğundaki ad: /urunler/hb-a1sp</summary>
    public required string Kod { get; init; }
    public required string Model { get; init; }
    public required string Ad { get; init; }
    public required string Slogan { get; init; }
    public required string Ozet { get; init; }
    public required string Hakkinda { get; init; }
    public required string Etiket { get; init; }
    public bool EtiketVurgulu { get; init; }
    public required string Gorsel { get; init; }
    /// <summary>Teklif formundaki seçenek metni.</summary>
    public required string FormSecenegi { get; init; }
    public required string[] Baglanti { get; init; }
    public required string[] KartMaddeleri { get; init; }
    public required string[] OneCikanlar { get; init; }
    public required string[] KullanimAlanlari { get; init; }
    public required TeknikGrup[] Teknik { get; init; }
    public required (string Ad, string Aciklama)[] Sertifikalar { get; init; }

    public string Adres => $"/urunler/{Kod}";
    public string Foy => $"/foyler/{Model}-foy.pdf";
}

/// <summary>Sitede satılan dört ürün. Veriler Türkçe föylerden alınmıştır.</summary>
public static class UrunKatalogu
{
    private static readonly (string, string) Ce = ("CE", "Güvenlik ve çevre koruma standartlarına uygundur.");
    private static readonly (string, string) Atex = ("ATEX", "Patlayıcı ortamlarda çalışmaya uygundur.");
    private static readonly (string, string) Ip67 = ("IP67", "Toz ve suya karşı dayanıklılık seviyesi standartlarına uygundur.");

    private static readonly string[] KilitAlanlari = ["Konteynerler", "Tankerler", "Lojistik araçları", "Çelik kasalar", "Yakıt depo kapakları"];

    public static readonly IReadOnlyList<Urun> Tumu =
    [
        new Urun
        {
            Kod = "hb-a1sp",
            Model = "HB-A1SP",
            Ad = "4G Güneş Enerjili Elektronik Kilit",
            Slogan = "Güneş enerjisiyle uzun süreli, güvenli ve akıllı kilit sistemi",
            Ozet = "Güneş paneliyle şarj olan, 4G bağlantılı, patlamaya dayanıklı elektronik kilit. Konum takibi, uzaktan açma ve çoklu kilit zincirleme.",
            Hakkinda = "HB-A1SP, lojistik ve endüstriyel uygulamalar için geliştirilmiş, 4G bağlantılı, güneş enerjisiyle şarj olabilen, patlamaya dayanıklı (ATEX) elektronik kilit sistemidir. Uzaktan izleme, konum takibi, çoklu kilit zincirleme (Cascade) ve akıllı alarm fonksiyonlarıyla yüksek güvenlik ve kontrol sağlar.",
            Etiket = "GÜNEŞ ENERJİLİ",
            EtiketVurgulu = true,
            Gorsel = "/img/hb-a1sp.jpg",
            FormSecenegi = "HB-A1SP — Güneş enerjili kilit",
            Baglanti = ["4G", "GNSS", "Bluetooth", "RFID"],
            KartMaddeleri = ["15.200 mAh batarya, güneş paneliyle şarj", "RFID kart, Bluetooth ve 4G uzaktan açma", "IP67, patlamaya dayanıklı (Ex) gövde"],
            OneCikanlar =
            [
                "Süper uzun bekleme süresi",
                "RFID kart ile kilit açma",
                "Bluetooth mobil uygulama ile kilit açma",
                "4G üzerinden uzaktan kilit açma",
                "Çoklu kilit zincirleme çalışma (Cascade)",
                "IP67 su geçirmezlik",
                "Akıllı alarm sistemi",
                "Patlamaya dayanıklı (Ex) gövde",
                "Güneş paneli ile şarj (isteğe bağlı)",
            ],
            KullanimAlanlari = KilitAlanlari,
            Teknik =
            [
                new("Fiziksel özellikler",
                [
                    ("Boyut", "209 × 115 × 51 mm"),
                    ("Halat uzunluğu", "300 mm"),
                    ("Ağırlık", "1.150 g"),
                    ("Gövde", "Yüksek ısıya dayanıklı PC"),
                    ("Kilit mili dayanımı", "1.000 kg"),
                    ("Koruma sınıfı", "IP67"),
                ]),
                new("GNSS konumlandırma",
                [
                    ("Uydu sistemleri", "GPS, BeiDou (64 bağımsız sinyal)"),
                    ("İzleme hassasiyeti", "1 – 10 m"),
                    ("Başlatma süreleri", "Sıcak 1 sn · Ilık 13 sn · Soğuk 32 sn"),
                ]),
                new("İletişim",
                [
                    ("Bağlantı teknolojisi", "LTE FDD / WCDMA / GSM"),
                    ("Veri desteği", "TCP / FTP / SMS"),
                ]),
                new("Kilit açma modları",
                [
                    ("RFID (radyo frekansı)", "2,5 cm"),
                    ("Bluetooth", "BLE 4.0 (iletişim mesafesi 20 m)"),
                    ("GPRS / SMS", "Mesafe sınırı yok"),
                ]),
                new("Güç / batarya",
                [
                    ("Ana batarya", "15.200 mAh Li-ion · 3,7 V · 55,5 Wh"),
                    ("USB şarj voltajı", "5 V DC 2 A"),
                    ("Güneş şarj akımı", "250 mA/saat"),
                    ("Şarj süresi", "10 saat"),
                    ("Bekleme süresi", "Güneş şarjı olmadan 31 gün (10 dakikada bir veri ile)"),
                ]),
                new("Çalışma ortamı",
                [
                    ("Çalışma / depolama", "−40 °C ile +85 °C"),
                    ("Şarj sıcaklığı", "0 °C ile +45 °C"),
                    ("Nem", "%0 – %100 (yoğuşmasız)"),
                    ("Montaj", "Mıknatıs ile yapışma"),
                ]),
            ],
            Sertifikalar = [Ce, Atex],
        },
        new Urun
        {
            Kod = "hb-a1m",
            Model = "HB-A1M",
            Ad = "4G Patlamaya Dayanıklı Elektronik Kilit",
            Slogan = "Kompakt, güvenli ve akıllı kilit sistemi",
            Ozet = "Küçük boyutlu ve hafif, 4G bağlantılı, patlamaya dayanıklı elektronik kilit. Uzun bekleme süresi ve akıllı alarm.",
            Hakkinda = "HB-A1M, lojistik ve endüstriyel uygulamalar için geliştirilmiş, 4G bağlantılı, patlamaya dayanıklı (ATEX) elektronik kilit sistemidir. Kompakt boyutu, hafif yapısı, uzun bekleme süresi ve akıllı alarm fonksiyonlarıyla yüksek güvenlik ve kontrol sağlar.",
            Etiket = "KOMPAKT",
            Gorsel = "/img/hb-a1m.jpg",
            FormSecenegi = "HB-A1M — Patlamaya dayanıklı kilit",
            Baglanti = ["4G", "GNSS", "Bluetooth", "RFID"],
            KartMaddeleri = ["544 g, 124 × 95 × 47 mm", "20 gün bekleme süresi", "Cascade ile çoklu kilit zincirleme"],
            OneCikanlar =
            [
                "Süper uzun bekleme süresi (20 gün)",
                "Küçük boyut ve hafif tasarım",
                "Çoklu kilit zincirleme çalışma (Cascade)",
                "IP67 su geçirmezlik",
                "Akıllı alarm sistemi ve Ex-proof gövde",
            ],
            KullanimAlanlari = KilitAlanlari,
            Teknik =
            [
                new("Fiziksel özellikler",
                [
                    ("Boyut", "124 × 95 × 47 mm"),
                    ("Ağırlık", "544 g"),
                    ("Kilit mili dayanımı", "1.000 kg"),
                ]),
                new("GNSS konumlandırma",
                [
                    ("Sinyal alma / izleme", "−147 dBm / −166 dBm"),
                    ("İzleme hassasiyeti", "1,5 m"),
                    ("Başlatma süreleri", "Sıcak 1 sn · Ilık 25 sn · Soğuk 28 sn"),
                ]),
                new("İletişim",
                [
                    ("Bağlantı teknolojisi", "4G LTE / WCDMA / GSM"),
                    ("Veri desteği", "TCP / FTP / SMS"),
                ]),
                new("Kilit açma modları",
                [
                    ("RFID (radyo frekansı)", "2,5 cm"),
                    ("Bluetooth", "BLE 4.0 (iletişim mesafesi 20 m)"),
                    ("GPRS / SMS", "Mesafe sınırı yok"),
                ]),
                new("Güç / batarya",
                [
                    ("Ana batarya", "2.950 mAh Li-ion · 3,7 V · 10,9 Wh"),
                    ("USB şarj voltajı", "5 V DC 2 A"),
                    ("Şarj süresi", "6 saat"),
                    ("Bekleme süresi", "22 gün (10 dakikada bir veri ile)"),
                ]),
                new("Çalışma ortamı",
                [
                    ("Çalışma / depolama", "−30 °C ile +70 °C"),
                    ("Şarj sıcaklığı", "0 °C ile +45 °C"),
                    ("Montaj", "Mıknatıs ile yapışma"),
                ]),
            ],
            Sertifikalar = [Ce, Atex, Ip67],
        },
        new Urun
        {
            Kod = "hb-a1ms",
            Model = "HB-A1Ms",
            Ad = "Patlamaya Dayanıklı İkincil Kilit (Sub-Lock)",
            Slogan = "Güvenli, akıllı ve yedek kilit çözümü",
            Ozet = "Ana kilitle Bluetooth üzerinden haberleşen ikincil kilit. 90 gün bekleme süresi, kompakt ve Ex-proof gövde.",
            Hakkinda = "HB-A1Ms, ana kilitle Bluetooth üzerinden haberleşen, patlamaya dayanıklı ikincil kilit (Sub-Lock) sistemidir. Kompakt ve hafif yapısı, uzun bekleme süresi ve Ex-proof korumasıyla zorlu endüstriyel koşullarda güvenli ve akıllı bir yedek kilit çözümü sunar.",
            Etiket = "İKİNCİL KİLİT",
            Gorsel = "/img/hb-a1ms.jpg",
            FormSecenegi = "HB-A1Ms — İkincil kilit",
            Baglanti = ["Bluetooth", "RFID"],
            KartMaddeleri = ["Ana kilitle Bluetooth üzerinden haberleşir", "90 gün bekleme süresi", "494 g, IP67, Ex-proof koruma"],
            OneCikanlar =
            [
                "Süper uzun bekleme süresi (90 gün)",
                "Ana kilit ile Bluetooth üzerinden haberleşme",
                "Küçük boyut, hafif yapı",
                "IP67 su geçirmezlik",
                "Ex-proof koruma",
            ],
            KullanimAlanlari = KilitAlanlari,
            Teknik =
            [
                new("Boyutlar / ağırlık",
                [
                    ("Boyutlar", "124 × 95 × 47 mm"),
                    ("Ağırlık", "494 g"),
                ]),
                new("Kilit açma modları",
                [
                    ("RFID", "2,5 cm"),
                    ("Bluetooth", "4.0 BLE (20 m)"),
                ]),
                new("Batarya",
                [
                    ("Batarya", "2.950 mAh Li-ion · 3,7 V · 10,9 Wh"),
                    ("Bekleme süresi", "3 ay (10 dakikada bir veri ile)"),
                ]),
                new("Şarj",
                [
                    ("Şarj voltajı", "5 V DC 2 A"),
                    ("Şarj süresi", "3 saat"),
                ]),
            ],
            Sertifikalar = [Ce, Atex, Ip67],
        },
        new Urun
        {
            Kod = "hb-vt08",
            Model = "HB-VT08",
            Ad = "Video Telematik Sistemi",
            Slogan = "Araç filosu için akıllı video izleme çözümü",
            Ozet = "4 kanallı video izleme, ADAS ve DSM desteğiyle filo güvenliği, sürücü davranış analizi ve yol güvenliği.",
            Hakkinda = "HB-VT08; 4 kanallı akıllı video izleme, ADAS, DSM ve yolcu izleme fonksiyonlarını bir araya getiren gelişmiş bir video telematik sistemidir. Filo güvenliği, sürücü davranış analizi ve yol güvenliği için kapsamlı ve etkin bir çözüm sunar.",
            Etiket = "FİLO",
            Gorsel = "/img/hb-vt08.jpg",
            FormSecenegi = "HB-VT08 — Video telematik",
            Baglanti = ["4G", "GPS", "Video izleme"],
            KartMaddeleri = ["4 kanallı akıllı video izleme", "ADAS, DSM ve yolcu izleme kameraları", "256 / 512 GB dahili depolama"],
            OneCikanlar =
            [
                "4 kanallı akıllı video izleme",
                "ADAS, DSM ve yolcu izleme kameraları",
                "Hız sınırı tabela tanıma (trafik işareti algılama)",
                "IP65 koruma: toza ve düşük basınçlı su püskürtmelerine dayanıklı",
                "Dahili yedek batarya",
            ],
            KullanimAlanlari = ["Lojistik araçları", "Yolcu otobüsleri", "Tankerler", "Ticari araçlar", "Servis araçları"],
            Teknik =
            [
                new("Dahili depolama",
                [
                    ("Depolama seçenekleri", "256 GB / 512 GB"),
                ]),
                new("Batarya",
                [
                    ("Dahili yedek batarya", "2.950 mAh"),
                ]),
                new("Arayüzler",
                [
                    ("Dijital giriş", "3 adet"),
                    ("Dijital çıkış", "1 adet"),
                    ("RS232", "1 adet"),
                    ("SOS butonu", "1 adet"),
                ]),
                new("Çalışma ortamı",
                [
                    ("Çalışma sıcaklığı", "−30 °C ile +70 °C"),
                    ("Koruma sınıfı", "IP65"),
                ]),
            ],
            Sertifikalar = [Ce],
        },
    ];

    public static Urun? Bul(string? kod) =>
        Tumu.FirstOrDefault(u => string.Equals(u.Kod, kod, StringComparison.OrdinalIgnoreCase));
}
