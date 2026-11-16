namespace Tasimacilik.Markalar;

/// <summary>Üç taşımacılık markası. Sıra önemlidir: ilk marka, alan adı eşleşmediğinde kullanılır.</summary>
public static class MarkaKatalogu
{

    public static readonly IReadOnlyList<Marka> Tumu =
    [
        // ───────────────────────── Selçuklu Turizm ─────────────────────────
        new Marka
        {
            Kod = "selcuklu",
            Ad = "Selçuklu Turizm",
            Domain = "selcukluturizm.com",
            EkAlanAdlari = ["selcukluturizm.com.tr"],
            Sembol = """<circle cx="32" cy="32" r="31" fill="var(--ana)"/><rect x="14.5" y="14.5" width="35" height="35" rx="2" fill="none" stroke="var(--vurgu)" stroke-width="1.8"/><rect x="14.5" y="14.5" width="35" height="35" rx="2" fill="none" stroke="var(--vurgu)" stroke-width="1.8" transform="rotate(45 32 32)"/><text x="32" y="43" text-anchor="middle" font-family="Playfair Display, Georgia, serif" font-size="31" font-weight="700" fill="#fff">S</text>""",
            LogoUst = "SELÇUKLU",
            LogoAlt = "TURİZM",
            RenkAna = "#0e7c86",
            RenkKoyu = "#0b2f3a",
            RenkVurgu = "#d9a441",
            RenkAcik = "#eef7f7",
            FontBaslik = "\"Playfair Display\", Georgia, serif",
            VitrinAraci = "midibus",
            Baslik = "Selçuklu Turizm — Ankara Çıkışlı Tur ve Gezi Taşımacılığı",
            Aciklama = "Selçuklu Turizm; Ankara çıkışlı günübirlik turlar, Ege, GAP, Karadeniz ve Doğu turları ile şehir içi ve dışı grup taşımacılığı için şoförlü araç sağlar.",
            Etiket = "Ankara çıkışlı · Tur ve gezi taşımacılığı",
            HeroBaslik = "Gezinizi siz planlayın,",
            HeroVurgu = "yolu bize bırakın",
            HeroMetin = "Günübirlik gezilerden Ege, GAP, Karadeniz ve Doğu turlarına kadar; grubunuza uygun aracı şoförüyle sağlıyor, program boyunca yanınızda oluyoruz.",
            Rozetler = ["D2 belgeli taşımacılık", "Şoförlü araç", "Yazılı fiyat teklifi"],
            HizmetlerBaslik = "Tur ve gezi taşımacılığı",
            HizmetlerMetin = "Bir günlük kültür gezisinden bir haftalık bölge turuna kadar her program için ayrı bir ulaşım planı yapıyoruz.",
            Hizmetler =
            [
                new("turizm-ve-gezi-tasimaciligi", "Turizm ve Gezi Taşımacılığı", "harita",
                    "Dernek, kurum ve gruplar için gezi programına göre şoförlü araç.",
                    [
                        "Gezinin programını siz hazırlarsınız; biz duraklama noktalarını, mola sürelerini ve dönüş saatini dikkate alarak ulaşımı kurarız. Araç ve şoför program boyunca grubunuza ayrılır.",
                        "Dernek gezisi, şirket gezisi, emekli grubu ya da arkadaş topluluğu fark etmez: kişi sayınıza göre Vito'dan 54 kişilik otobüse kadar uygun aracı öneririz.",
                    ],
                    ["Gezi programına göre güzergâh ve mola planı", "Kişi sayısına uygun araç", "Günübirlik ve konaklamalı programlar", "D2 belgeli tur taşımacılığı"]),
                new("gunubirlik-turlar", "Günübirlik Turlar", "takvim",
                    "Sabah çıkıp akşam Ankara'ya döndüğünüz bir günlük geziler.",
                    [
                        "Ankara'dan bir günde gidilip dönülebilecek pek çok rota var: Kapadokya, Eskişehir, Safranbolu, Beypazarı, Tuz Gölü gibi. Gideceğiniz yeri ve grubunuzu söyleyin, kalkış saatini ve molaları birlikte belirleyelim.",
                        "Araç gün boyu grubunuzla kalır; gezi noktalarında sizi bekler, akşam kalkış noktasına geri getirir.",
                    ],
                    ["Ankara'dan sabah çıkış, akşam dönüş", "Gezi noktalarında bekleyen araç", "Kalkış noktasını siz belirlersiniz", "Hafta içi ve hafta sonu"]),
                new("ege-turu", "Ege Turu", "dalga",
                    "Ankara çıkışlı, çok günlü Ege programları için otobüs ve şoför.",
                    [
                        "Efes, Pamukkale, Şirince, Kuşadası, Bodrum gibi duraklardan oluşan bir Ege programında yol uzun, duraklar çoktur. Programınızı alıp günlük sürüş sürelerini ve mola noktalarını buna göre planlıyoruz.",
                        "Çok günlü turlarda araç ve şoför tur süresince grubunuza ayrılır; konaklama yerinden her sabah aynı araçla yola devam edersiniz.",
                    ],
                    ["Çok günlü programa uygun planlama", "Tur boyunca aynı araç ve şoför", "Uzun yola uygun otobüs", "Program değişikliklerinde esneklik"]),
                new("gap-turu", "GAP Turu", "gunes",
                    "Güneydoğu Anadolu'ya kültür turları için uzun yol ulaşımı.",
                    [
                        "Şanlıurfa, Göbeklitepe, Harran, Mardin, Midyat, Nemrut Dağı gibi noktaları kapsayan bir GAP turu, Ankara'dan uzun bir yolculukla başlar. Bu yolu rahat geçirmeniz için geniş bagajlı, yatar koltuklu otobüslerle çalışıyoruz.",
                        "Bölgedeki duraklar arasındaki mesafeler de uzundur; günlük programı, sürüş ve mola süreleri gerçekçi kalacak şekilde birlikte kuruyoruz.",
                    ],
                    ["Uzun yol için yatar koltuklu otobüs", "Gerçekçi günlük sürüş planı", "Tur boyunca aynı araç ve şoför", "Kalabalık gruplar için 54 koltuk"]),
                new("karadeniz-turu", "Karadeniz Turu", "agac",
                    "Yayla ve kıyı rotalarına, Karadeniz yollarına uygun araçla tur.",
                    [
                        "Trabzon, Sümela, Uzungöl, Rize ve Ayder yaylası gibi rotalarda yollar dar ve virajlıdır. Grubunuzun büyüklüğüne ve gideceğiniz yaylalara göre otobüs, midibüs ya da minibüs öneriyoruz.",
                        "Bazı yaylalara büyük otobüsle çıkılamaz; böyle bir durak varsa bunu planlama aşamasında söyler, çözümünü birlikte buluruz.",
                    ],
                    ["Rotaya göre araç büyüklüğü", "Yayla yolları için midibüs ve minibüs seçeneği", "Çok günlü programa uygun planlama", "Tur boyunca aynı şoför"]),
                new("dogu-turu", "Doğu Turu", "dag",
                    "Kars, Ani, Van ve Doğubayazıt gibi rotalarda uzun soluklu turlar.",
                    [
                        "Doğu Anadolu turları uzun mesafeli ve çok günlüdür; Kars, Ani Harabeleri, Doğubayazıt, İshak Paşa Sarayı ve Van gibi duraklar arasında saatler süren yollar vardır. Programınızı, bu yolları yormadan geçecek şekilde birlikte planlıyoruz.",
                        "Mevsime göre yol ve hava koşullarını hesaba katıyor, tarih seçerken bunu sizinle paylaşıyoruz.",
                    ],
                    ["Uzun mesafeye uygun otobüs", "Mevsime göre planlama", "Tur boyunca aynı araç ve şoför", "Kişi sayısına göre araç seçimi"]),
                new("sehir-ici-ve-disi-toplu-tasima", "Şehir İçi ve Dışı Toplu Taşıma", "kisiler",
                    "Kurumların ve grupların düzenli ya da tek seferlik toplu ulaşımı.",
                    [
                        "Bir kurumun çalışanlarını, bir derneğin üyelerini ya da bir etkinliğin katılımcılarını bir noktadan diğerine toplu olarak taşıyoruz. Şehir içinde belirlediğiniz duraklardan toplama yapabilir, şehir dışındaki bir adrese götürebiliriz.",
                        "Düzenli seferler için güzergâh ve saatleri sizinle kurar, kişi sayısı değiştiğinde aracı ona göre güncelleriz.",
                    ],
                    ["Belirlenen duraklardan toplama", "Düzenli ya da tek seferlik çalışma", "Şehir içi ve şehirlerarası", "Kişi sayısına göre araç"]),
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
            AraclarMetin = "8 kişilik Vito'dan 54 kişilik otobüse kadar altı araçlık filomuzla, kişi sayınıza uygun aracı planlıyoruz.",
            Araclar =
            [
                new("vip", "VIP Minivan", "Mercedes-Benz Vito", "8 kişilik", "Yönetici transferleri, küçük heyetler ve misafir karşılama için.", ["Klima", "Bagaj alanı", "Kapıdan kapıya ulaşım"]),
                new("minibus", "Minibüs", "Mercedes-Benz Sprinter", "19 kişilik", "Küçük gezi grupları ve dar yollu rotalar için.", ["Klima", "Kolay iniş-biniş", "Şehir içine uygun boyut"]),
                new("midibus", "Midibüs", "Otokar Sultan", "31 kişilik", "Orta büyüklükteki gruplar, günübirlik turlar ve yayla rotaları için.", ["Klima", "Bagaj bölmesi", "Şehir içi ve şehir dışı"]),
                new("otobus", "Otobüs", "Mercedes-Benz Travego", "46 kişilik", "Konaklamalı kültür turları ve uzun yol programları için.", ["Klima", "Yatar koltuk", "Geniş bagaj hacmi"]),
                new("otobus", "Otobüs", "MAN Lion's Coach", "54 kişilik", "Kalabalık tur grupları ve bölge turları için.", ["Klima", "Yatar koltuk", "Geniş bagaj hacmi"]),
                new("otobus", "Otobüs", "Mercedes-Benz Tourismo", "54 kişilik", "Büyük grupların şehirlerarası gezileri için.", ["Klima", "Yatar koltuk", "Geniş bagaj hacmi"]),
            ],
            Sss =
            [
                ("Tur programını siz mi hazırlıyorsunuz?", "Biz ulaşım tarafını üstleniyoruz: programınıza göre aracı, şoförü, güzergâhı ve mola planını hazırlıyoruz. Programınız henüz netleşmediyse gideceğiniz bölgeyi ve gün sayısını yazın, ulaşımı birlikte kuralım."),
                ("Çok günlü turlarda araç ve şoför ne yapar?", "Araç ve şoför tur süresince grubunuza ayrılır, her gün programa göre yola devam edersiniz. Şoför konaklaması gibi giderler teklifte ayrıca belirtilir."),
                ("Fiyat neye göre belirleniyor?", "Güzergâhın uzunluğu, gün sayısı ve araç büyüklüğü fiyatı belirler. Bilgileri ilettiğinizde yazılı teklif hazırlıyoruz."),
                ("Tek günlük geziler için de çalışıyor musunuz?", "Evet. Ankara'dan sabah çıkıp akşam dönülen günübirlik geziler için de araç tahsis ediyoruz."),
            ],
            HakkimizdaBaslik = "Yolculuğun planlı olanı güzeldir",
            Hakkimizda =
            [
                "Selçuklu Turizm, Ankara merkezli bir yolcu taşımacılığı firmasıdır. Günübirlik gezilerin, Ege, GAP, Karadeniz ve Doğu turlarının ve grupların şehir içi ve dışı ulaşımını D2 belgesiyle üstleniyoruz.",
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
            EkAlanAdlari = ["ankaraotobuskirala.com.tr", "ankaraotobuskiralama.com.tr"],
            Sembol = """<circle cx="32" cy="32" r="31" fill="var(--ana)"/><path d="M5 27h6M3 32h8M6 37h5" stroke="#fff" stroke-width="2.2" stroke-linecap="round" opacity=".85"/><path d="M14 41V26c0-3.3 2.7-6 6-6h27c3 0 5.4 2 6.2 4.9L56 35v6z" fill="#fff"/><path d="M18 23.5h28.5l2.6 8.5H18z" fill="var(--koyu)"/><rect x="14" y="35" width="42" height="2.6" fill="var(--vurgu)"/><circle cx="23" cy="42" r="4.6" fill="var(--koyu)"/><circle cx="23" cy="42" r="1.7" fill="#fff"/><circle cx="47" cy="42" r="4.6" fill="var(--koyu)"/><circle cx="47" cy="42" r="1.7" fill="#fff"/><path d="M12 50h44" stroke="var(--vurgu)" stroke-width="2.4" stroke-dasharray="6 4" stroke-linecap="round"/>""",
            LogoUst = "ANKARA",
            LogoAlt = "OTOBÜS KİRALA",
            RenkAna = "#c8102e",
            RenkKoyu = "#1c1f26",
            RenkVurgu = "#ffb703",
            RenkAcik = "#fdf1f2",
            FontBaslik = "\"Barlow Condensed\", \"Arial Narrow\", sans-serif",
            VitrinAraci = "otobus",
            Baslik = "Ankara Otobüs Kirala — Şoförlü Otobüs, Midibüs ve Minibüs Kiralama",
            Aciklama = "Ankara'da şoförlü otobüs, midibüs, minibüs ve VIP minibüs kiralama. Tur, okul gezisi, film ve dizi çekimi ve şehirlerarası yolculuklar için D2 belgeli araç kiralayın.",
            Etiket = "Şoförlü araç kiralama · Ankara",
            HeroBaslik = "Grubunuz kaç kişi?",
            HeroVurgu = "Ona göre otobüs kiralayın",
            HeroMetin = "8 kişilik Vito'dan 54 kişilik otobüse kadar şoförlü araç kiralıyoruz. Tarihi ve güzergâhı söyleyin, fiyatı aynı gün bildirelim.",
            Rozetler = ["8 – 54 koltuk", "Şoför dahil", "D2 belgeli"],
            HizmetlerBaslik = "Hangi araca ihtiyacınız var?",
            HizmetlerMetin = "Kiralama türünü seçin; kapasiteyi, kullanım alanlarını ve fiyata nelerin dahil olduğunu görün.",
            Hizmetler =
            [
                new("kiralik-otobus", "Kiralık Otobüs", "otobus",
                    "46 ve 54 kişilik otobüsler; saatlik, günlük ya da sefer bazlı kiralama.",
                    [
                        "Kalabalık bir grubu tek araçta taşımanın en ekonomik yolu otobüs kiralamaktır. Filomuzda 46 kişilik Mercedes-Benz Travego ile 54 kişilik Mercedes-Benz Tourismo ve MAN Lion's Coach var.",
                        "Kiralama süresini ihtiyacınıza göre seçersiniz: birkaç saatlik bir transfer, tam gün bir gezi ya da günlerce süren bir program. Fiyatı süreye ve mesafeye göre çıkarıyoruz.",
                    ],
                    ["46 ve 54 kişilik seçenek", "Saatlik, günlük ya da sefer bazlı", "Şehir içi ve şehirlerarası", "Şoförüyle birlikte"]),
                new("kiralik-minibus", "Kiralık Minibüs", "minibus",
                    "19 kişilik Mercedes-Benz Sprinter ve 31 kişilik Otokar Sultan midibüs.",
                    [
                        "Otobüs için az kalabalık gruplarınız için 19 kişilik Sprinter minibüs ya da 31 kişilik Otokar Sultan midibüs kiralayın. Şehir içinde park ve manevra açısından otobüse göre çok daha pratiktir.",
                        "Birkaç otomobil yerine tek bir araçla yola çıkarsınız: konvoy derdi olmaz, yol masrafı kişi başına düşer, herkes aynı anda varır.",
                    ],
                    ["Sprinter 19, Sultan 31 kişilik", "Adresten alma ve bırakma", "Dar sokak ve site içlerine uygun", "Kısa süreli kiralamaya uygun"]),
                new("soforlu-otobus-kiralama", "Şoförlü Otobüs Kiralama", "yol",
                    "Araç, şoför ve yakıt tek fiyatta; siz yalnızca yolculuğu düşünün.",
                    [
                        "Kiraladığınız her araç şoförüyle birlikte gelir; şoförsüz kiralama yapmıyoruz. Güzergâhı, kalkış ve dönüş saatlerini bildirirsiniz, gerisini şoförünüz yönetir.",
                        "Fiyata araç, şoför ve yakıt dahildir. Otoyol, köprü ve otopark gibi giderler ile çok günlü işlerde şoför konaklaması teklifte ayrıca yazılır; yolculuk sonunda sürpriz çıkmaz.",
                    ],
                    ["Şoför ve yakıt fiyata dahil", "Ek giderler teklifte açıkça yazılı", "Güzergâhı siz belirlersiniz", "Tüm araçlarda geçerli"]),
                new("tur-otobusu-kiralama", "Tur Otobüsü Kiralama", "harita",
                    "Çok günlü turlar ve uzun yol için yatar koltuklu otobüs.",
                    [
                        "Ege, Karadeniz, GAP ya da Doğu gibi çok günlü programlar için tur boyunca grubunuza ayrılan bir otobüs kiralayın. Yatar koltuklar ve geniş bagaj bölmesi uzun yolu rahat geçirmenizi sağlar.",
                        "Tur acenteleri ve grup organizatörleri için programa göre günlük sürüş planı çıkarıyor, aynı araç ve şoförle turu baştan sona tamamlıyoruz.",
                    ],
                    ["Tur boyunca aynı araç ve şoför", "Yatar koltuk ve geniş bagaj", "Acente ve organizatörlere uygun", "D2 belgeli"]),
                new("vip-minibus-kiralama", "VIP Minibüs Kiralama", "yildiz",
                    "8 kişilik Mercedes-Benz Vito ile küçük gruplara özel yolculuk.",
                    [
                        "Misafir karşılama, yönetici ulaşımı ya da ailece yolculuk için 8 kişilik Mercedes-Benz Vito kiralayın. Küçük bir grubu otomobilden rahat, minibüsten daha özel bir şekilde taşır.",
                        "Havalimanı ve otogar ulaşımında bavullarınız için yer vardır; araç yalnızca sizin grubunuza ayrılır.",
                    ],
                    ["8 kişilik Mercedes-Benz Vito", "Misafir ve yönetici ulaşımı", "Bavul alanı", "Havalimanı ve otogar ulaşımı"]),
                new("d2-belgeli-otobus-kiralama", "D2 Belgeli Otobüs Kiralama", "belge",
                    "Tarifesiz yolcu taşımacılığı için D2 yetki belgeli araç.",
                    [
                        "Ücretli ya da organizasyon kapsamındaki yolcu taşımacılığında aracın D2 yetki belgesine sahip bir firmaya ait olması gerekir. Araçlarımız D2 belgemiz kapsamında çalışır.",
                        "Tur, okul gezisi, kurumsal etkinlik ya da şehirlerarası grup yolculuğu için kiraladığınız araçla belgeli taşımacılık yaparsınız. İhtiyaç duyarsanız belge bilgilerini teklifle birlikte paylaşırız.",
                    ],
                    ["D2 yetki belgesi kapsamında", "Tur ve organizasyonlara uygun", "Belge bilgisi talep üzerine", "Tüm araç tiplerinde"]),
                new("film-dizi-ozel-kiralama", "Film ve Dizi İçin Özel Kiralama", "kamera",
                    "Set ekibi ulaşımı ve çekimlerde kullanılacak araç kiralama.",
                    [
                        "Film, dizi ve reklam çekimlerinde ekibin sete ulaşımı için ya da sahnede kullanılacak araç olarak otobüs, midibüs ve minibüs kiralıyoruz.",
                        "Çekim günlerinin uzun ve değişken olduğunu biliyoruz; aracı gün boyu sette tutabilir, programa göre saatleri birlikte ayarlayabiliriz.",
                    ],
                    ["Set ekibi ulaşımı", "Çekimde kullanılacak araç", "Gün boyu sette bekleyen araç", "Çekim programına göre esnek saat"]),
                new("okul-gezileri", "Okul Gezileri", "okul",
                    "Öğrenci ve öğretmen sayısına uygun, okuldan alıp okula bırakan araç.",
                    [
                        "Müze ziyareti, bilim merkezi gezisi ya da şehir dışı okul turu için öğrenci ve öğretmen sayısına uygun aracı kiralayın.",
                        "Gezi programını iletmeniz yeterli; kalkış, bekleme ve dönüş saatlerini programa göre ayarlıyoruz. D2 belgeli araçlarla çalıştığımız için okul gezisi izin süreçlerinde istenen bilgileri sağlayabiliriz.",
                    ],
                    ["Öğrenci sayısına uygun araç", "Okuldan alma, okula bırakma", "Gezi programına göre bekleme", "D2 belgeli taşımacılık"]),
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
            AraclarMetin = "Altı araç, 8'den 54'e kadar koltuk. Hepsi şoförüyle kiralanır.",
            Araclar =
            [
                new("otobus", "Otobüs", "Mercedes-Benz Tourismo", "54 kişilik", "Kalabalık gruplar ve okul gezileri tek araçta.", ["Yatar koltuk", "Büyük bagaj", "Klima"]),
                new("otobus", "Otobüs", "MAN Lion's Coach", "54 kişilik", "Şehirlerarası grup yolculukları ve uzun yol için.", ["Yatar koltuk", "Büyük bagaj", "Klima"]),
                new("otobus", "Otobüs", "Mercedes-Benz Travego", "46 kişilik", "40 kişiyi aşan gruplar için ekonomik otobüs seçeneği.", ["Yatar koltuk", "Büyük bagaj", "Klima"]),
                new("midibus", "Midibüs", "Otokar Sultan", "31 kişilik", "Şirket gezileri, set ekipleri ve günübirlik turlar.", ["Klima", "Bagaj bölmesi", "Kolay park"]),
                new("minibus", "Minibüs", "Mercedes-Benz Sprinter", "19 kişilik", "Aile ve arkadaş grupları, kısa mesafe yolculuklar.", ["Klima", "Adrese kadar ulaşım", "Ekonomik"]),
                new("vip", "VIP Minivan", "Mercedes-Benz Vito", "8 kişilik", "Küçük gruplar ve havalimanı ya da otogar ulaşımı.", ["Klima", "Bagaj alanı", "Adrese kadar ulaşım"]),
            ],
            Sss =
            [
                ("Araçlar şoförlü mü kiralanıyor?", "Evet, tüm araçlar şoförüyle birlikte kiralanır. Şoförsüz kiralama yapmıyoruz."),
                ("Fiyata neler dahil?", "Araç, şoför ve yakıt fiyata dahildir. Otoyol ve köprü geçişleri ile otopark gibi masraflar teklifte ayrıca belirtilir."),
                ("Kaç saatliğine kiralayabilirim?", "Tek yön transferden çok günlü turlara kadar her süre için kiralama yapılır; fiyat süreye ve mesafeye göre hesaplanır."),
                ("Ne kadar önceden rezervasyon yapmalıyım?", "Özellikle hafta sonları, bayramlar ve okul gezisi dönemlerinde araçlar erken dolar. Tarihiniz belli olur olmaz bildirmenizi öneririz."),
                ("Şehir dışına gidiyor musunuz?", "Evet. Ankara çıkışlı tüm şehirlerarası güzergâhlar için otobüs, midibüs ve minibüs kiralayabilirsiniz."),
                ("Araçlarınız D2 belgeli mi?", "Evet. Araçlarımız D2 yetki belgemiz kapsamında çalışır; belge bilgilerini talep ederseniz teklifle birlikte paylaşırız."),
            ],
            HakkimizdaBaslik = "İşimiz otobüs kiralamak",
            Hakkimizda =
            [
                "Ankara Otobüs Kirala, D2 belgeli şoförlü otobüs, midibüs, minibüs ve VIP minibüs kiralama hizmeti verir. Turlar, okul gezileri, film ve dizi çekimleri ve şehirlerarası grup yolculukları için araç sağlıyoruz.",
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
            EkAlanAdlari = ["ankaraturizmtransfer.com.tr"],
            Sembol = """<path d="M32 3C20.4 3 11 12.2 11 23.5 11 38.5 32 61 32 61s21-22.5 21-37.5C53 12.2 43.6 3 32 3z" fill="var(--ana)"/><circle cx="32" cy="23.5" r="12.5" fill="#fff"/><path d="M24.5 23.5h14M33.5 18l5.5 5.5-5.5 5.5" fill="none" stroke="var(--vurgu)" stroke-width="3.4" stroke-linecap="round" stroke-linejoin="round"/>""",
            LogoUst = "ANKARA TURİZM",
            LogoAlt = "TRANSFER",
            RenkAna = "#2b3a8c",
            RenkKoyu = "#101536",
            RenkVurgu = "#19c39c",
            RenkAcik = "#eef0fb",
            FontBaslik = "\"Sora\", \"Segoe UI\", sans-serif",
            VitrinAraci = "vip",
            Baslik = "Ankara Turizm Transfer — Havalimanı, Otel ve Şehirlerarası Transfer",
            Aciklama = "Ankara Esenboğa Havalimanı transferi, otel transferi, şehirlerarası yolcu taşımacılığı, kurumsal etkinlik ve kongre taşımacılığı. Önceden rezervasyon, sabit fiyat.",
            Etiket = "Esenboğa · Otel · Şehirlerarası",
            HeroBaslik = "İndiğinizde aracınız",
            HeroVurgu = "sizi bekliyor olsun",
            HeroMetin = "Esenboğa Havalimanı'ndan otelinize, evinize ya da toplantınıza özel araçla transfer. Uçuş saatinizi bildirin; şoförünüz sizi çıkışta karşılasın.",
            Rozetler = ["Önceden rezervasyon", "Sabit fiyat", "Kapıdan kapıya"],
            HizmetlerBaslik = "Transfer hizmetlerimiz",
            HizmetlerMetin = "Havalimanından otele, otelden etkinliğe, Ankara'dan başka şehirlere; tek yolcudan kalabalık heyete kadar.",
            Hizmetler =
            [
                new("havalimani-transferi", "Havalimanı Transferleri", "ucak",
                    "Esenboğa Havalimanı'ndan adresinize, adresinizden havalimanına özel transfer.",
                    [
                        "Uçaktan indikten sonra taksi sırası beklemeyin, bavullarla servis aramayın. Rezervasyon sırasında uçuş bilgilerinizi verirsiniz; şoförünüz geliş katında adınızın yazılı olduğu levhayla bekler.",
                        "Gidiş yönünde uçuş saatinize göre sizi adresinizden alır, terminale bırakırız. Tek yolcu için 8 kişilik Vito, kalabalık gruplar için minibüs ya da otobüs ayarlanır.",
                    ],
                    ["Geliş katında isimli karşılama", "Uçuş saatine göre planlama", "Kişi ve bavul sayısına uygun araç", "Adresinize kadar ulaşım"]),
                new("otel-transferi", "Otel Transferi", "bina",
                    "Havalimanı, otogar ve gar ile oteliniz arasında, otelden toplantı ve etkinliklere ulaşım.",
                    [
                        "Ankara'ya gelen misafirlerinizi havalimanından, otogardan ya da gardan alıp otele bırakıyor; konaklama boyunca otelden toplantıya, yemeğe ya da gezi noktalarına götürüyoruz.",
                        "Oteller ve acenteler için misafir listesine göre karşılama planı çıkarıyor, farklı saatlerde gelen misafirleri ayrı seferlerle taşıyoruz.",
                    ],
                    ["Havalimanı, otogar ve gar karşılaması", "Otelden toplantı ve etkinliğe ulaşım", "Misafir listesine göre planlama", "Otel ve acentelerle çalışma"]),
                new("sehirlerarasi-yolcu-tasimaciligi", "Şehirlerarası Yolcu Taşımacılığı", "yol",
                    "Ankara'dan başka şehirlere aktarmasız, kapıdan kapıya yolculuk.",
                    [
                        "Kapadokya, Konya, Eskişehir, İstanbul ya da başka bir şehre aktarma yapmadan, kendi saatinizde gidin. Sizi Ankara'daki adresinizden ya da havalimanından alıp varış adresinize kadar götürüyoruz.",
                        "Tek yön ya da gidiş-dönüş çalışıyoruz; araç yalnızca sizin yolcularınızı taşır, tarifeli seferlerin saatine bağlı kalmazsınız.",
                    ],
                    ["Aktarmasız, kapıdan kapıya", "Tek yön ya da gidiş-dönüş", "Kalkış saatini siz seçersiniz", "Yalnızca sizin yolcularınız"]),
                new("kurumsal-etkinlik-tasimaciligi", "Kurumsal Etkinlik Taşımacılığı", "takvim",
                    "Lansman, şirket toplantısı, bayi buluşması ve gala ulaşımı.",
                    [
                        "Şirket etkinliklerinde katılımcıların zamanında ve bir arada gelmesi, organizasyonun ilk izlenimidir. Katılımcı listesini ve etkinlik takvimini alıp toplama noktalarını, kalkış saatlerini ve dönüş seferlerini planlıyoruz.",
                        "Etkinlik günü araçlar programa göre hazır bekler; son dakika değişikliklerini tek bir irtibat kişisiyle çözersiniz.",
                    ],
                    ["Katılımcı listesine göre planlama", "Toplama noktalarından alma", "Etkinlik sonrası dönüş seferi", "Tek irtibat kişisi"]),
                new("kongre-tasimaciligi", "Kongre Taşımacılığı", "kisiler",
                    "Kongre, fuar ve sempozyumlarda katılımcıların otel, salon ve havalimanı ulaşımı.",
                    [
                        "Çok günlü bir kongrede yüzlerce katılımcı farklı saatlerde gelir, her sabah otelden salona gider, akşam geri döner. Kongre takvimini alıp havalimanı karşılamalarını, otel – salon ring seferlerini ve uğurlamaları saat saat çıkarıyoruz.",
                        "Katılımcı sayısına göre Vito'dan 54 kişilik otobüse kadar farklı büyüklükte araçları aynı anda planlıyoruz.",
                    ],
                    ["Havalimanı karşılama ve uğurlama", "Otel – kongre salonu ring seferleri", "Kongre takvimine göre sefer çizelgesi", "Aynı anda birden fazla araç"]),
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
                new("vip", "VIP Minivan", "Mercedes-Benz Vito", "8 yolcu", "Bireysel yolcular, aileler ve özel misafirler için havalimanı ve şehir içi transfer.", ["Geniş koltuk", "Klima", "Bavul alanı"]),
                new("minibus", "Minibüs", "Mercedes-Benz Sprinter", "19 yolcu", "Aynı uçuşla gelen ekipler ve küçük tur grupları için.", ["Klima", "Bagaj bölmesi", "Kolay iniş-biniş"]),
                new("midibus", "Midibüs", "Otokar Sultan", "31 yolcu", "Kongre katılımcıları ve orta büyüklükteki gruplar için toplu transfer.", ["Klima", "Büyük bagaj", "Tek araçta ulaşım"]),
                new("otobus", "Otobüs", "Mercedes-Benz Travego", "46 yolcu", "Kalabalık grupların havalimanı ve şehirlerarası transferi için.", ["Klima", "Yatar koltuk", "Büyük bagaj"]),
                new("otobus", "Otobüs", "MAN Lion's Coach", "54 yolcu", "Tur grupları ve büyük heyetler için tek araçta transfer.", ["Klima", "Yatar koltuk", "Büyük bagaj"]),
                new("otobus", "Otobüs", "Mercedes-Benz Tourismo", "54 yolcu", "Etkinlik ve fuar katılımcılarının toplu transferi için.", ["Klima", "Yatar koltuk", "Büyük bagaj"]),
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
                "Ankara Turizm Transfer; Esenboğa Havalimanı ve otel transferleri, Ankara çıkışlı şehirlerarası yolcu taşımacılığı ile kurumsal etkinlik ve kongre taşımacılığı hizmeti verir.",
                "Bir yolculuğun en yorucu kısmı çoğu zaman havalimanı ile varış adresi arasındaki kısa mesafedir. Biz o kısmı önceden planlıyoruz: araç ayrılmış, fiyat belli, şoför sizi bekliyor.",
                "İş seyahatine gelen tek bir yolcudan aynı uçakla inen kalabalık bir gruba kadar, kişi ve bavul sayısına uygun aracı ayarlıyoruz.",
            ],
            TeklifBaslik = "Transferinizi şimdi ayırtın",
            TeklifMetin = "Tarih, saat ve adresleri yazın; araç tipi ve sabit fiyatla rezervasyon onayınızı gönderelim.",
            WhatsAppMesaj = "Merhaba, transfer rezervasyonu için fiyat almak istiyorum.",
        },
    ];

    /// <summary>Alan adına (www'li ya da www'suz, ana ya da ek alan adı) göre markayı bulur.</summary>
    public static Marka? AlanAdinaGore(string? host)
    {
        if (string.IsNullOrEmpty(host)) return null;
        var ad = host.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? host[4..] : host;
        return Tumu.FirstOrDefault(m =>
            string.Equals(m.Domain, ad, StringComparison.OrdinalIgnoreCase) ||
            m.EkAlanAdlari.Any(e => string.Equals(e, ad, StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>Sunucuda tanımlanacak tüm alan adları (www'li ve www'suz).</summary>
    public static IEnumerable<string> TumAlanAdlari =>
        Tumu.SelectMany(m => new[] { m.Domain }.Concat(m.EkAlanAdlari)).SelectMany(a => new[] { a, "www." + a });

    public static Marka? KodaGore(string? kod) =>
        Tumu.FirstOrDefault(m => string.Equals(m.Kod, kod, StringComparison.OrdinalIgnoreCase));
}
