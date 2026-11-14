namespace Tasimacilik.Markalar;

/// <summary>Üç taşımacılık markası. Sıra önemlidir: ilk marka, alan adı eşleşmediğinde kullanılır.</summary>
public static class MarkaKatalogu
{
    private const string Inter = "&family=Inter:wght@400;500;600;700&display=swap";

    public static readonly IReadOnlyList<Marka> Tumu =
    [
        // ───────────────────────── Selçuklu Turizm ─────────────────────────
        new Marka
        {
            Kod = "selcuklu",
            Ad = "Selçuklu Turizm",
            Domain = "selcukluturizm.com",
            Sembol = """<rect x="9" y="9" width="30" height="30" rx="3" fill="var(--ana)"/><rect x="9" y="9" width="30" height="30" rx="3" fill="var(--ana)" transform="rotate(45 24 24)"/><circle cx="24" cy="24" r="8" fill="var(--vurgu)"/><circle cx="24" cy="24" r="3.5" fill="var(--koyu)"/>""",
            LogoUst = "SELÇUKLU",
            LogoAlt = "TURİZM",
            RenkAna = "#0e7c86",
            RenkKoyu = "#0b2f3a",
            RenkVurgu = "#d9a441",
            RenkAcik = "#eef7f7",
            FontBaslik = "\"Playfair Display\", Georgia, serif",
            FontAdresi = "https://fonts.googleapis.com/css2?family=Playfair+Display:wght@600;700;800" + Inter,
            VitrinAraci = "midibus",
            Baslik = "Selçuklu Turizm — Ankara Personel, Gezi ve Organizasyon Taşımacılığı",
            Aciklama = "Selçuklu Turizm; Ankara'da personel taşımacılığı, gezi ve tur ulaşımı, kongre ve organizasyon servisleri ile şehirlerarası özel seferler sunar.",
            Etiket = "Ankara · Kurumsal taşımacılık",
            HeroBaslik = "Ekibinizi ve misafirlerinizi",
            HeroVurgu = "zamanında, rahat taşıyoruz",
            HeroMetin = "Personel servisinden kültür gezisine, kongre ulaşımından şehirlerarası özel sefere kadar; güzergâhınıza ve kişi sayınıza uygun aracı planlıyor, yolculuğu baştan sona biz yönetiyoruz.",
            Rozetler = ["Şoförlü araç", "Yazılı fiyat teklifi", "Güzergâha özel planlama"],
            HizmetlerBaslik = "Kurumlara ve gruplara ulaşım hizmetleri",
            HizmetlerMetin = "Düzenli servislerden tek seferlik organizasyonlara kadar her iş için ayrı bir plan yapıyoruz.",
            Hizmetler =
            [
                new("personel-tasimaciligi", "Personel Taşımacılığı", "kisiler",
                    "Vardiya saatlerinize göre planlanan, düzenli personel servisi.",
                    [
                        "Çalışanlarınızın işe zamanında ve dinlenmiş gelmesi, servis güzergâhının doğru kurulmasıyla başlar. Adres listenizi alıyor, durakları ve kalkış saatlerini vardiya düzeninize göre çıkarıyoruz.",
                        "Personel sayınız ya da vardiyanız değiştiğinde güzergâhı ve araç büyüklüğünü birlikte güncelliyoruz.",
                    ],
                    ["Adres listesine göre güzergâh ve durak planı", "Vardiya saatlerine uygun kalkış düzeni", "Kişi sayısına göre minibüs, midibüs ya da otobüs", "Aylık sözleşmeli çalışma"]),
                new("gezi-ve-tur-tasimaciligi", "Gezi ve Tur Taşımacılığı", "harita",
                    "Kültür gezileri ve günübirlik turlar için şoförlü araç.",
                    [
                        "Dernek, okul, şirket ya da arkadaş grubunuzla çıkacağınız gezinin programını bize iletin; duraklama noktalarını ve süreleri dikkate alarak ulaşımı planlayalım.",
                        "Günübirlik turlarda da konaklamalı gezilerde de araç ve şoför program boyunca grubunuzla kalır.",
                    ],
                    ["Günübirlik ve konaklamalı programlar", "Gezi programına göre mola ve bekleme planı", "Grup büyüklüğüne uygun araç", "Ankara çıkışlı şehir dışı turlar"]),
                new("kongre-ve-organizasyon-ulasimi", "Kongre ve Organizasyon Ulaşımı", "takvim",
                    "Toplantı, fuar ve kongrelerde katılımcı ulaşımı.",
                    [
                        "Çok sayıda katılımcının farklı saatlerde ve farklı noktalardan taşınması ayrı bir planlama ister. Etkinlik takviminizi alıp otel, mekân ve havalimanı arasındaki seferleri saat saat çıkarıyoruz.",
                        "Etkinlik günlerinde araçlar programa göre hazır bekler; değişiklikleri tek bir kişiyle konuşarak çözersiniz.",
                    ],
                    ["Otel – etkinlik mekânı ring seferleri", "Havalimanı karşılama ve uğurlama", "Etkinlik programına göre sefer çizelgesi", "Tek irtibat kişisiyle koordinasyon"]),
                new("sehirlerarasi-ozel-sefer", "Şehirlerarası Özel Sefer", "yol",
                    "Grubunuza özel, kapıdan kapıya şehirlerarası yolculuk.",
                    [
                        "Tarifeli sefer saatlerine bağlı kalmadan, kendi belirlediğiniz saatte ve adresten yola çıkın. Araç yalnızca sizin grubunuzu taşır.",
                        "Gidiş-dönüş ya da tek yön seferler için tarih, kişi sayısı ve güzergâhı iletmeniz yeterli.",
                    ],
                    ["Adresten alma, adrese bırakma", "Tek yön ya da gidiş-dönüş", "Yalnızca grubunuza ayrılmış araç", "Kalkış saatini siz belirlersiniz"]),
            ],
            NedenBaslik = "Selçuklu Turizm ile çalışmak",
            Nedenler =
            [
                ("Önce planlıyoruz", "Fiyat vermeden önce güzergâhı, saatleri ve kişi sayısını netleştiriyor; aracı buna göre seçiyoruz."),
                ("Yazılı teklif", "Ne ödeyeceğinizi baştan bilirsiniz; teklif yazılı olarak iletilir."),
                ("Tek muhatap", "Planlamadan yolculuk gününe kadar aynı kişiyle görüşürsünüz."),
                ("İhtiyaca uygun araç", "Küçük ekipler için minibüs, kalabalık gruplar için otobüs: boş koltuğa para ödemezsiniz."),
            ],
            AdimlarBaslik = "Üç adımda yola çıkın",
            Adimlar =
            [
                ("Talebinizi iletin", "Tarih, güzergâh ve kişi sayısını form, telefon ya da WhatsApp ile bildirin."),
                ("Teklifinizi alın", "Size uygun aracı ve fiyatı içeren yazılı teklifi gönderelim."),
                ("Yola çıkın", "Onayınızla birlikte araç ve şoför belirlenen saatte adresinizde olur."),
            ],
            AraclarBaslik = "Araç filomuz",
            AraclarMetin = "Kişi sayınıza göre üç farklı büyüklükte araçla çalışıyoruz.",
            Araclar =
            [
                new("otobus", "Otobüs", "46 – 54 koltuk", "Kalabalık personel servisleri, kongre grupları ve uzun yol gezileri için.", ["Geniş bagaj hacmi", "Klima", "Yatar koltuk"]),
                new("midibus", "Midibüs", "27 – 31 koltuk", "Orta büyüklükteki ekipler ve günübirlik turlar için dengeli seçenek.", ["Klima", "Bagaj bölmesi", "Şehir içine uygun boyut"]),
                new("minibus", "Minibüs", "16 – 19 koltuk", "Küçük ekiplerin servisleri ve dar sokaklı güzergâhlar için.", ["Klima", "Kolay iniş-biniş", "Ekonomik kullanım"]),
            ],
            Sss =
            [
                ("Personel servisi için en az kaç kişi gerekir?", "Alt sınır yok; kişi sayısına göre uygun büyüklükte aracı öneriyoruz. Küçük ekipler için minibüsle başlanabilir."),
                ("Fiyat neye göre belirleniyor?", "Güzergâhın uzunluğu, araç büyüklüğü, sefer sayısı ve çalışma süresi fiyatı belirler. Bilgileri ilettiğinizde yazılı teklif hazırlıyoruz."),
                ("Ankara dışına sefer yapıyor musunuz?", "Evet. Ankara çıkışlı şehirlerarası özel seferler ve konaklamalı geziler için araç tahsis ediyoruz."),
                ("Tek seferlik işler için de çalışıyor musunuz?", "Evet. Tek günlük gezi, toplantı ya da organizasyon ulaşımı için de teklif alabilirsiniz."),
            ],
            HakkimizdaBaslik = "Yolculuğun planlı olanı güzeldir",
            Hakkimizda =
            [
                "Selçuklu Turizm, Ankara merkezli bir yolcu taşımacılığı firmasıdır. Kurumların personel servislerini, grupların gezi ve tur ulaşımını, kongre ve organizasyonların katılımcı taşımasını üstleniyoruz.",
                "İşimizin büyük kısmı araç yola çıkmadan önce yapılır: güzergâhın kurulması, saatlerin belirlenmesi, doğru büyüklükte aracın seçilmesi. Yolculuk günü geldiğinde size yalnızca araca binmek kalır.",
                "Her talebi ayrı değerlendiriyor, fiyatı yazılı olarak bildiriyor ve süreç boyunca tek bir irtibat kişisi üzerinden iletişim kuruyoruz.",
            ],
            TeklifBaslik = "Ulaşım planınızı birlikte yapalım",
            TeklifMetin = "Tarihi, güzergâhı ve kişi sayısını yazın; size uygun araç ve fiyat teklifiyle dönelim.",
            WhatsAppMesaj = "Merhaba, Selçuklu Turizm'den ulaşım hizmeti için bilgi almak istiyorum.",
        },

        // ───────────────────────── Ankara Otobüs Kirala ─────────────────────────
        new Marka
        {
            Kod = "otobuskirala",
            Ad = "Ankara Otobüs Kirala",
            Domain = "ankaraotobuskirala.com",
            Sembol = """<rect width="48" height="48" rx="12" fill="var(--ana)"/><rect x="11" y="8" width="26" height="28" rx="5" fill="#fff"/><rect x="14" y="12" width="20" height="11" rx="2" fill="var(--koyu)"/><circle cx="16.5" cy="30" r="2.2" fill="var(--vurgu)"/><circle cx="31.5" cy="30" r="2.2" fill="var(--vurgu)"/><rect x="13" y="36" width="6" height="5" rx="1.5" fill="#fff"/><rect x="29" y="36" width="6" height="5" rx="1.5" fill="#fff"/>""",
            LogoUst = "ANKARA",
            LogoAlt = "OTOBÜS KİRALA",
            RenkAna = "#c8102e",
            RenkKoyu = "#1c1f26",
            RenkVurgu = "#ffb703",
            RenkAcik = "#fdf1f2",
            FontBaslik = "\"Barlow Condensed\", \"Arial Narrow\", sans-serif",
            FontAdresi = "https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@600;700;800" + Inter,
            VitrinAraci = "otobus",
            Baslik = "Ankara Otobüs Kirala — Şoförlü Otobüs, Midibüs ve Minibüs Kiralama",
            Aciklama = "Ankara'da şoförlü otobüs, midibüs ve minibüs kiralama. Düğün, gezi, okul ve şehirlerarası yolculuklar için grubunuza uygun aracı kiralayın.",
            Etiket = "Şoförlü araç kiralama · Ankara",
            HeroBaslik = "Grubunuz kaç kişi?",
            HeroVurgu = "Ona göre otobüs kiralayın",
            HeroMetin = "16 kişilik minibüsten 54 kişilik otobüse kadar şoförlü araç kiralıyoruz. Tarihi ve güzergâhı söyleyin, fiyatı aynı gün bildirelim.",
            Rozetler = ["16 – 54 koltuk", "Şoför dahil", "Günlük ya da sefer bazlı"],
            HizmetlerBaslik = "Hangi araca ihtiyacınız var?",
            HizmetlerMetin = "Kiralama türünü seçin; kapasiteyi ve kullanım alanlarını görün.",
            Hizmetler =
            [
                new("soforlu-otobus-kiralama", "Şoförlü Otobüs Kiralama", "otobus",
                    "46 – 54 koltuklu otobüsler, kalabalık gruplar ve uzun yol için.",
                    [
                        "40 kişiyi aşan gruplar için en ekonomik çözüm tek bir otobüstür: herkes aynı araçta, aynı saatte varır. Otobüsü şoförüyle birlikte, yakıt dahil olarak kiralarsınız.",
                        "Şehirlerarası yolculuklarda bagaj hacmi ve yatar koltuklar uzun yolu rahat geçirmenizi sağlar.",
                    ],
                    ["46 – 54 koltuk kapasitesi", "Şoför ve yakıt fiyata dahil", "Geniş bagaj bölmesi", "Şehir içi ve şehirlerarası kullanım"]),
                new("midibus-kiralama", "Midibüs Kiralama", "midibus",
                    "27 – 31 koltuklu midibüsler, orta büyüklükteki gruplar için.",
                    [
                        "Otobüs için az, minibüs için fazla olan gruplar midibüsle rahat eder. Şehir içinde park ve manevra açısından otobüse göre daha pratiktir.",
                        "Günübirlik geziler, şirket etkinlikleri ve davet ulaşımı için en çok tercih edilen araç tipidir.",
                    ],
                    ["27 – 31 koltuk kapasitesi", "Şehir içine uygun boyut", "Klima ve bagaj bölmesi", "Yarım gün ve tam gün kiralama"]),
                new("minibus-kiralama", "Minibüs Kiralama", "minibus",
                    "16 – 19 koltuklu minibüsler, küçük gruplar ve kısa mesafeler için.",
                    [
                        "Aile, arkadaş grubu ya da küçük bir ekiple yola çıkacaksanız birkaç otomobil yerine tek bir minibüs kiralayın: konvoy derdi olmaz, yol masrafı kişi başına düşer.",
                        "Dar sokaklara ve site içlerine rahat girdiği için adresten alma ve bırakma kolaydır.",
                    ],
                    ["16 – 19 koltuk kapasitesi", "Adresten alma ve bırakma", "Kısa süreli kiralamaya uygun", "Havalimanı ve otogar ulaşımı"]),
                new("dugun-ve-davet-araci", "Düğün ve Davet Aracı", "kalp",
                    "Düğün, nişan ve davetlerde misafir taşıma.",
                    [
                        "Misafirlerinizin salona nasıl geleceğini düşünmek zorunda kalmayın. Belirlediğiniz noktalardan toplayıp salona getiriyor, davet bitince aynı noktalara bırakıyoruz.",
                        "Şehir dışındaki düğünler için gidiş-dönüş kiralama yapabilir, misafir sayısına göre birden fazla araç ayarlayabiliriz.",
                    ],
                    ["Belirlenen noktalardan toplama", "Davet bitiminde dönüş seferi", "Şehir dışı düğünlere gidiş-dönüş", "Misafir sayısına göre birden fazla araç"]),
                new("okul-gezisi-otobusu", "Okul Gezisi Otobüsü", "okul",
                    "Okul gezileri ve öğrenci etkinlikleri için otobüs.",
                    [
                        "Müze ziyareti, bilim merkezi gezisi ya da şehir dışı okul turu için öğrenci ve öğretmen sayısına uygun aracı kiralayın.",
                        "Gezi programını iletmeniz yeterli; kalkış, bekleme ve dönüş saatlerini programa göre ayarlıyoruz.",
                    ],
                    ["Öğrenci sayısına uygun araç", "Gezi programına göre bekleme", "Okuldan alma, okula bırakma", "Günübirlik ve şehir dışı geziler"]),
            ],
            NedenBaslik = "Neden bizden kiralamalısınız?",
            Nedenler =
            [
                ("Net fiyat", "Şoför ve yakıt dahil tek bir fiyat alırsınız; yolculuk sonunda sürpriz çıkmaz."),
                ("Doğru büyüklük", "Kişi sayınızı söyleyin, fazla koltuğa para ödemeyeceğiniz aracı önerelim."),
                ("Hızlı dönüş", "Talebiniz aynı gün içinde fiyatlanır."),
                ("Esnek süre", "Birkaç saatlik transfer de, günlerce süren tur da kiralanabilir."),
            ],
            AdimlarBaslik = "Kiralama nasıl işliyor?",
            Adimlar =
            [
                ("Bilgileri verin", "Tarih, kalkış ve varış noktası ile kişi sayısını yazın ya da arayın."),
                ("Fiyatı öğrenin", "Uygun aracı ve şoför dahil fiyatı bildirelim."),
                ("Aracınız gelsin", "Rezervasyonu onaylayın; araç belirlediğiniz saatte kapınızda olsun."),
            ],
            AraclarBaslik = "Kiralık araçlar",
            AraclarMetin = "Üç araç tipi, 16'dan 54'e kadar koltuk seçeneği.",
            Araclar =
            [
                new("otobus", "Otobüs", "46 – 54 koltuk", "Düğün konvoyları, okul gezileri ve şehirlerarası yolculuklar.", ["Yatar koltuk", "Büyük bagaj", "Klima"]),
                new("midibus", "Midibüs", "27 – 31 koltuk", "Şirket gezileri, davetler ve günübirlik turlar.", ["Klima", "Bagaj bölmesi", "Kolay park"]),
                new("minibus", "Minibüs", "16 – 19 koltuk", "Aile ve arkadaş grupları, kısa mesafe transferler.", ["Klima", "Adrese kadar ulaşım", "Ekonomik"]),
            ],
            Sss =
            [
                ("Araçlar şoförlü mü kiralanıyor?", "Evet, tüm araçlar şoförüyle birlikte kiralanır. Şoförsüz kiralama yapmıyoruz."),
                ("Fiyata neler dahil?", "Araç, şoför ve yakıt fiyata dahildir. Otoyol ve köprü geçişleri ile otopark gibi masraflar teklifte ayrıca belirtilir."),
                ("Kaç saatliğine kiralayabilirim?", "Tek yön transferden çok günlü turlara kadar her süre için kiralama yapılır; fiyat süreye ve mesafeye göre hesaplanır."),
                ("Ne kadar önceden rezervasyon yapmalıyım?", "Özellikle hafta sonları ve düğün sezonunda araçlar erken dolar. Tarihiniz belli olur olmaz bildirmenizi öneririz."),
                ("Şehir dışına gidiyor musunuz?", "Evet. Ankara çıkışlı tüm şehirlerarası güzergâhlar için otobüs, midibüs ve minibüs kiralayabilirsiniz."),
            ],
            HakkimizdaBaslik = "İşimiz otobüs kiralamak",
            Hakkimizda =
            [
                "Ankara Otobüs Kirala, şoförlü otobüs, midibüs ve minibüs kiralama hizmeti verir. Düğünler, okul gezileri, şirket etkinlikleri ve şehirlerarası grup yolculukları için araç sağlıyoruz.",
                "Kiralamayı basit tutuyoruz: kaç kişi olduğunuzu, nereden nereye ve hangi tarihte gideceğinizi söylersiniz; biz de size uygun aracı ve şoför dahil fiyatı bildiririz.",
                "Her grubun ihtiyacı farklıdır. Bu yüzden hazır paket satmıyor, her talep için ayrı fiyat çıkarıyoruz.",
            ],
            TeklifBaslik = "Kaç kişisiniz, nereye gidiyorsunuz?",
            TeklifMetin = "Bilgileri doldurun, size uygun aracı ve fiyatı aynı gün bildirelim.",
            WhatsAppMesaj = "Merhaba, şoförlü araç kiralamak istiyorum. Fiyat alabilir miyim?",
        },

        // ───────────────────────── Ankara Turizm Transfer ─────────────────────────
        new Marka
        {
            Kod = "transfer",
            Ad = "Ankara Turizm Transfer",
            Domain = "ankaraturizmtransfer.com",
            Sembol = """<path d="M24 3C14.600 3 7 10.400 7 19.500 7 31.500 24 45 24 45s17-13.500 17-25.500C41 10.400 33.400 3 24 3z" fill="var(--ana)"/><path d="M15 20.500h15.500M25.500 15l5.500 5.500-5.500 5.500" stroke="var(--vurgu)" stroke-width="3.400" fill="none" stroke-linecap="round" stroke-linejoin="round"/>""",
            LogoUst = "ANKARA TURİZM",
            LogoAlt = "TRANSFER",
            RenkAna = "#2b3a8c",
            RenkKoyu = "#101536",
            RenkVurgu = "#19c39c",
            RenkAcik = "#eef0fb",
            FontBaslik = "\"Sora\", \"Segoe UI\", sans-serif",
            FontAdresi = "https://fonts.googleapis.com/css2?family=Sora:wght@600;700;800" + Inter,
            VitrinAraci = "vip",
            Baslik = "Ankara Turizm Transfer — Esenboğa Havalimanı ve Şehir İçi Transfer",
            Aciklama = "Ankara Esenboğa Havalimanı transferi, otel ve şehir içi transfer, VIP araçla özel transfer ve şehirlerarası transfer hizmeti. Önceden rezervasyon, sabit fiyat.",
            Etiket = "Esenboğa · Otel · Şehirlerarası",
            HeroBaslik = "İndiğinizde aracınız",
            HeroVurgu = "sizi bekliyor olsun",
            HeroMetin = "Esenboğa Havalimanı'ndan otelinize, evinize ya da toplantınıza özel araçla transfer. Uçuş saatinizi bildirin; şoförünüz sizi çıkışta karşılasın.",
            Rozetler = ["Önceden rezervasyon", "Sabit fiyat", "Kapıdan kapıya"],
            HizmetlerBaslik = "Transfer hizmetlerimiz",
            HizmetlerMetin = "Tek kişilik yolculuktan grup transferine, kısa mesafeden şehirlerarasına.",
            Hizmetler =
            [
                new("esenboga-havalimani-transferi", "Esenboğa Havalimanı Transferi", "ucak",
                    "Havalimanından şehre, şehirden havalimanına özel transfer.",
                    [
                        "Uçaktan indikten sonra taksi sırası beklemeyin, bavullarla servis aramayın. Rezervasyon sırasında uçuş bilgilerinizi verirsiniz; şoförünüz geliş katında adınızın yazılı olduğu levhayla bekler.",
                        "Gidiş yönünde ise uçuş saatinize göre sizi adresinizden alır, terminale bırakırız.",
                    ],
                    ["Geliş katında isimli karşılama", "Uçuş saatine göre planlama", "Bavul sayısına uygun araç", "Adresinize kadar ulaşım"]),
                new("otel-ve-sehir-ici-transfer", "Otel ve Şehir İçi Transfer", "bina",
                    "Otel, toplantı ve etkinlikler arasında şehir içi ulaşım.",
                    [
                        "Ankara'da bir günde birkaç adrese gidecekseniz her seferinde araç aramak yerine aracı ve şoförü belirlediğiniz süre boyunca yanınızda tutabilirsiniz.",
                        "Otel misafirleri, iş ziyaretçileri ve etkinlik katılımcıları için noktadan noktaya ya da saatlik tahsis seçenekleri var.",
                    ],
                    ["Noktadan noktaya transfer", "Saatlik ve günlük araç tahsisi", "Otel ve toplantı mekânı ulaşımı", "Çoklu adres programları"]),
                new("vip-arac-ile-transfer", "VIP Araç ile Transfer", "yildiz",
                    "Konforlu VIP araçla özel yolculuk.",
                    [
                        "Önemli bir misafiri karşılayacaksanız ya da yolculuğu çalışarak geçirmek istiyorsanız VIP araç seçin. Geniş ve karşılıklı koltuk düzeni yol boyunca rahat konuşmanızı ve çalışmanızı sağlar.",
                        "Aile yolculuklarında da herkesin aynı araçta, bavullarıyla birlikte rahat etmesi için tercih edilir.",
                    ],
                    ["Geniş koltuk düzeni", "Yalnızca size ayrılmış araç", "Misafir karşılama için uygun", "Bavul için ayrı alan"]),
                new("sehirlerarasi-transfer", "Şehirlerarası Transfer", "yol",
                    "Ankara'dan çevre illere ve turistik bölgelere özel transfer.",
                    [
                        "Kapadokya, Konya, Eskişehir ya da başka bir şehre aktarma yapmadan, kendi saatinizde gidin. Sizi Ankara'daki adresinizden ya da havalimanından alıp varış adresinize kadar götürüyoruz.",
                        "Dönüş için de aynı aracı ayırtabilir, tek yön ya da gidiş-dönüş fiyat alabilirsiniz.",
                    ],
                    ["Aktarmasız, kapıdan kapıya", "Tek yön ya da gidiş-dönüş", "Havalimanı çıkışlı şehirlerarası transfer", "Kalkış saatini siz seçersiniz"]),
                new("grup-transferi", "Grup Transferi", "kisiler",
                    "Kalabalık gruplar için tek araçla transfer.",
                    [
                        "Aynı uçakla gelen bir ekip ya da tur grubu için herkesi tek araçta taşıyoruz. Kişi ve bavul sayısını bildirmeniz yeterli; minibüs, midibüs ya da otobüs ayarlanır.",
                        "Farklı saatlerde inen misafirler için karşılama saatlerine göre ayrı araçlar planlanabilir.",
                    ],
                    ["Kişi ve bavul sayısına göre araç", "Tek araçta toplu ulaşım", "Farklı iniş saatleri için ayrı planlama", "Otel ve etkinlik mekânına teslim"]),
            ],
            NedenBaslik = "Transferde fark eden ayrıntılar",
            Nedenler =
            [
                ("Sabit fiyat", "Fiyat rezervasyon sırasında belirlenir; trafik ya da bekleme nedeniyle değişmez."),
                ("Uçuşunuza göre karşılama", "Uçuş bilgilerinizi verirsiniz, karşılama saati ona göre ayarlanır."),
                ("Kapıdan kapıya", "Aktarma, durak ya da paylaşımlı yolculuk yok; araç yalnızca sizin içindir."),
                ("Kişi sayısına uygun araç", "Tek yolcu için VIP araç, kalabalık grup için minibüs ya da otobüs."),
            ],
            AdimlarBaslik = "Rezervasyon üç adım",
            Adimlar =
            [
                ("Yolculuğu bildirin", "Tarih, saat, uçuş bilgisi, adres ve kişi sayısını iletin."),
                ("Onay alın", "Araç tipi ve sabit fiyatla rezervasyon onayınızı gönderelim."),
                ("Karşılanın", "Şoförünüz belirlenen saatte buluşma noktasında sizi bekler."),
            ],
            AraclarBaslik = "Transfer araçları",
            AraclarMetin = "Yolcu ve bavul sayınıza göre araç seçilir.",
            Araclar =
            [
                new("vip", "VIP Araç", "6 – 9 yolcu", "Bireysel yolcular, aileler ve özel misafirler için konforlu transfer aracı.", ["Geniş koltuk", "Klima", "Bavul alanı"]),
                new("minibus", "Minibüs", "16 – 19 yolcu", "Aynı uçuşla gelen ekipler ve küçük tur grupları için.", ["Klima", "Bagaj bölmesi", "Kolay iniş-biniş"]),
                new("midibus", "Midibüs", "27 – 31 yolcu", "Kongre katılımcıları ve kalabalık gruplar için toplu transfer.", ["Klima", "Büyük bagaj", "Tek araçta ulaşım"]),
            ],
            Sss =
            [
                ("Uçağım gecikirse ne olur?", "Rezervasyonda uçuş bilgilerinizi alıyoruz. Gecikme durumunda bize haber vermeniz yeterli; karşılama saatini güncelliyoruz."),
                ("Fiyat kişi başı mı, araç başı mı?", "Fiyat araç başına verilir. Aracın kapasitesi dahilinde kaç kişi olduğunuz fiyatı değiştirmez."),
                ("Havalimanında şoförü nasıl bulurum?", "Şoförünüz geliş katı çıkışında adınızın yazılı olduğu levhayla bekler. Ayrıca rezervasyon onayında iletişim numarası paylaşılır."),
                ("Ne kadar önceden rezervasyon yapmalıyım?", "Aracın ayrılabilmesi için yolculuktan en az bir gün önce bildirmenizi öneririz. Aynı gün talepleri için arayarak uygunluk sorabilirsiniz."),
                ("Çocuk koltuğu sağlıyor musunuz?", "İhtiyacınızı rezervasyon sırasında belirtin; uygunluk durumunu onay sırasında bildiririz."),
            ],
            HakkimizdaBaslik = "Yolculuğun ilk ve son kilometresi bizde",
            Hakkimizda =
            [
                "Ankara Turizm Transfer, Esenboğa Havalimanı ile şehir arasında ve Ankara çıkışlı şehirlerarası güzergâhlarda özel transfer hizmeti verir.",
                "Bir yolculuğun en yorucu kısmı çoğu zaman havalimanı ile varış adresi arasındaki kısa mesafedir. Biz o kısmı önceden planlıyoruz: araç ayrılmış, fiyat belli, şoför sizi bekliyor.",
                "İş seyahatine gelen tek bir yolcudan aynı uçakla inen kalabalık bir gruba kadar, kişi ve bavul sayısına uygun aracı ayarlıyoruz.",
            ],
            TeklifBaslik = "Transferinizi şimdi ayırtın",
            TeklifMetin = "Tarih, saat ve adresleri yazın; araç tipi ve sabit fiyatla rezervasyon onayınızı gönderelim.",
            WhatsAppMesaj = "Merhaba, transfer rezervasyonu için fiyat almak istiyorum.",
        },
    ];

    /// <summary>Alan adına (www'li ya da www'suz) göre markayı bulur.</summary>
    public static Marka? AlanAdinaGore(string? host)
    {
        if (string.IsNullOrEmpty(host)) return null;
        var ad = host.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? host[4..] : host;
        return Tumu.FirstOrDefault(m => string.Equals(m.Domain, ad, StringComparison.OrdinalIgnoreCase));
    }

    public static Marka? KodaGore(string? kod) =>
        Tumu.FirstOrDefault(m => string.Equals(m.Kod, kod, StringComparison.OrdinalIgnoreCase));
}
