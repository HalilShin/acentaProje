using System.ComponentModel.DataAnnotations;

namespace Acenta.Veri;

public enum TurTuru
{
    Gunubirlik = 0,
    Konaklamali = 1,
}

/// <summary>Bir tur: tanıtım bilgileri, programı ve kalkış tarihleri. Panelden yönetilir.</summary>
public class Tur
{
    public int Id { get; set; }

    /// <summary>Adreste kullanılan kısa ad: /turlar/{Kod}.</summary>
    [MaxLength(120)] public string Kod { get; set; } = "";
    [MaxLength(160)] public string Ad { get; set; } = "";
    /// <summary>Bolgeler.Tumu içindeki kod (kapadokya, gap...).</summary>
    [MaxLength(40)] public string Bolge { get; set; } = "";
    public TurTuru Turu { get; set; }
    /// <summary>"1 gece 2 gün", "Günübirlik" gibi.</summary>
    [MaxLength(60)] public string Sure { get; set; } = "";
    [MaxLength(60)] public string Ulasim { get; set; } = "Otobüsle";
    [MaxLength(300)] public string Ozet { get; set; } = "";
    /// <summary>Buluşma yeri ve saati, kısa: "Kızılay, 07.00".</summary>
    [MaxLength(120)] public string Bulusma { get; set; } = "";

    /// <summary>Gün gün program; her satır bir gün ya da durak. "1. Gün: ..." biçiminde yazılabilir.</summary>
    public string Program { get; set; } = "";
    /// <summary>Fiyata dahil olanlar; her satır bir madde.</summary>
    public string Dahil { get; set; } = "";
    /// <summary>Fiyata dahil olmayanlar; her satır bir madde.</summary>
    public string Haric { get; set; } = "";
    /// <summary>Buluşma noktası, iptal koşulları gibi notlar.</summary>
    public string Notlar { get; set; } = "";

    /// <summary>Kişi başı fiyat (TL). Tarihe özel fiyat yoksa bu kullanılır.</summary>
    public decimal Fiyat { get; set; }

    /// <summary>Yüklenen kapak fotoğrafının dosya adı (App_Data/fotograflar); boşsa bölge fotoğrafı.</summary>
    [MaxLength(80)] public string? Fotograf { get; set; }

    public bool Yayinda { get; set; } = true;
    /// <summary>Ana sayfada öne çıkarılır.</summary>
    public bool OneCikan { get; set; }
    public DateTime Olusturma { get; set; } = DateTime.Now;

    public List<TurTarihi> Tarihler { get; set; } = [];

    public string Adres => "/turlar/" + Kod;

    public IEnumerable<string> Satirlar(string metin) =>
        metin.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    /// <summary>Bugünden sonraki tarihler, sırayla.</summary>
    public IEnumerable<TurTarihi> GelecekTarihler(DateTime bugun) =>
        Tarihler.Where(t => t.Tarih.Date >= bugun.Date).OrderBy(t => t.Tarih);
}

/// <summary>Bir turun kalkış tarihi ve kontenjanı.</summary>
public class TurTarihi
{
    public int Id { get; set; }
    public int TurId { get; set; }
    public Tur Tur { get; set; } = null!;

    public DateTime Tarih { get; set; }
    /// <summary>Toplam koltuk.</summary>
    public int Kontenjan { get; set; }
    /// <summary>Dolu koltuk: iptal edilmemiş kayıtlardaki yolcu sayısı. Kayıt değiştikçe panel günceller (sitede hızlı okumak için tutulur).</summary>
    public int Ayrilan { get; set; }
    /// <summary>Bu tarihe özel kişi başı fiyat; boşsa turun fiyatı.</summary>
    public decimal? Fiyat { get; set; }
    [MaxLength(120)] public string? Not { get; set; }

    public List<Kayit> Kayitlar { get; set; } = [];

    public int Bos => Math.Max(0, Kontenjan - Ayrilan);
    public bool Dolu => Bos == 0;
    /// <summary>Kontenjanın azaldığını göstermek için: 10 ve altı boş yer.</summary>
    public bool Az => !Dolu && Bos <= 10;
}
