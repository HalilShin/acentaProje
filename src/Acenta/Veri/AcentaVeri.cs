using Microsoft.EntityFrameworkCore;

namespace Acenta.Veri;

public class AcentaVeri(DbContextOptions<AcentaVeri> secenekler) : DbContext(secenekler)
{
    public DbSet<Tur> Turlar => Set<Tur>();
    public DbSet<TurTarihi> TurTarihleri => Set<TurTarihi>();
    public DbSet<Kayit> Kayitlar => Set<Kayit>();
    public DbSet<Yolcu> Yolcular => Set<Yolcu>();

    /// <summary>
    /// Kalkışların dolu koltuk sayısını kayıtlardan yeniden hesaplayıp kaydeder.
    /// Kayıt değişiklikleri kaydedildikten (SaveChanges) sonra çağrılır.
    /// </summary>
    public async Task AyrilaniGuncelleAsync(IEnumerable<int> turTarihiIdleri, CancellationToken iptal = default)
    {
        foreach (var id in turTarihiIdleri.Distinct())
        {
            var sayi = await Yolcular.CountAsync(y => y.Kayit.TurTarihiId == id && !y.Kayit.Iptal, iptal);
            await TurTarihleri.Where(t => t.Id == id).ExecuteUpdateAsync(s => s.SetProperty(t => t.Ayrilan, sayi), iptal);
        }
    }

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Tur>(e =>
        {
            e.HasIndex(t => t.Kod).IsUnique();
            e.Property(t => t.Fiyat).HasPrecision(10, 2);
            e.HasMany(t => t.Tarihler).WithOne(t => t.Tur).HasForeignKey(t => t.TurId).OnDelete(DeleteBehavior.Cascade);
        });
        m.Entity<TurTarihi>(e =>
        {
            e.Property(t => t.Fiyat).HasPrecision(10, 2);
            e.Property(t => t.Tarih).HasColumnType("date");
            e.HasIndex(t => t.Tarih);
            // Kayıtları olan bir kalkış silinemez; önce kayıtlar taşınmalı ya da silinmeli.
            e.HasMany(t => t.Kayitlar).WithOne(k => k.TurTarihi).HasForeignKey(k => k.TurTarihiId).OnDelete(DeleteBehavior.Restrict);
        });
        m.Entity<Kayit>(e =>
        {
            e.Property(k => k.Tutar).HasPrecision(10, 2);
            e.Property(k => k.Alinan).HasPrecision(10, 2);
            e.HasMany(k => k.Yolcular).WithOne(y => y.Kayit).HasForeignKey(y => y.KayitId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
