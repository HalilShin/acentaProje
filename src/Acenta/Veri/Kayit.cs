using System.ComponentModel.DataAnnotations;

namespace Acenta.Veri;

/// <summary>
/// Bir kalkış tarihine yapılan rezervasyon (WhatsApp ya da telefonla alınıp panele girilir).
/// Her koltuk bir <see cref="Yolcu"/> satırıdır; isim verilmemiş koltuklar da satır olarak tutulur.
/// </summary>
public class Kayit
{
    public int Id { get; set; }
    public int TurTarihiId { get; set; }
    public TurTarihi TurTarihi { get; set; } = null!;

    /// <summary>Rezervasyonu yapan, iletişim kurulacak kişi.</summary>
    [MaxLength(100)] public string IletisimAd { get; set; } = "";
    [MaxLength(30)] public string Telefon { get; set; } = "";
    [MaxLength(120)] public string? BinisYeri { get; set; }

    /// <summary>Toplam ücret (TL).</summary>
    public decimal Tutar { get; set; }
    /// <summary>Alınan ödeme (kapora dahil, TL).</summary>
    public decimal Alinan { get; set; }

    [MaxLength(500)] public string? Not { get; set; }
    /// <summary>İptal edilen kayıt koltuk tutmaz ama raporda görünür.</summary>
    public bool Iptal { get; set; }
    public DateTime Olusturma { get; set; } = DateTime.Now;

    public List<Yolcu> Yolcular { get; set; } = [];

    public decimal Kalan => Math.Max(0, Tutar - Alinan);
    public string OdemeDurumu => Tutar > 0 && Alinan >= Tutar ? "Ödendi" : Alinan > 0 ? "Kapora" : "Bekliyor";
}

/// <summary>Kayıttaki bir koltuk/yolcu. Tur günü yoklaması bu satırlar üzerinden yapılır.</summary>
public class Yolcu
{
    public int Id { get; set; }
    public int KayitId { get; set; }
    public Kayit Kayit { get; set; } = null!;

    [MaxLength(100)] public string AdSoyad { get; set; } = "";
    /// <summary>TC kimlik ya da pasaport no (yolcu listesi için, isteğe bağlı).</summary>
    [MaxLength(20)] public string? KimlikNo { get; set; }
    public int Sira { get; set; }
    /// <summary>Tur günü yoklama: null = bekleniyor, true = bindi, false = gelmedi.</summary>
    public bool? Bindi { get; set; }
    /// <summary>Yoklamanın işaretlendiği zaman.</summary>
    public DateTime? YoklamaZamani { get; set; }
}
