using Entities.Concrete;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class AEUContext : DbContext
    {
        //public AEUContext(DbContextOptions<AEUContext> options)
        //: base(options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;database=aeu_obs;user=root;password=1967",
                    new MySqlServerVersion(new Version(8, 0, 44)));
            }
            }

        // Entity Database tablo eşleltirme 
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<AkademisyenDersAtama> AkademisyenDersAtamalari { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Eposta> Epostalar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Not> Notlar { get; set; }
        public DbSet<OgrenciKayit> OgrenciKayitlari { get; set; }
        public DbSet<Sinav> Sinavlar { get; set; }
        public DbSet<Telefon> Telefonlar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // kullanicilar
            modelBuilder.Entity<Kullanici>(entity =>
            {
                entity.ToTable("kullanicilar", t =>
                {
                    t.HasCheckConstraint("CHK_Kullanicilar_KurumEpostaDomain", "kurum_eposta LIKE '%@ahievran.edu.tr'");
                });
                entity.HasKey(e => e.KullaniciUuid);

                entity.Property(e => e.KullaniciUuid)
                      .HasColumnName("kullanici_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                // SQL enum default 'OGRENCI' -> enum underlying int assumed 0
                entity.Property(e => e.KullaniciTipi)
                      .HasColumnName("kullanici_tipi")
                      .HasColumnType("ENUM('OGRENCI', 'AKADEMISYEN', 'PERSONEL')")
                      //.HasConversion<int>()
                      .IsRequired()
                      .HasDefaultValue(KullaniciTipi.OGRENCI);

                entity.Property(e => e.Ad)
                      .HasColumnName("ad")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.OrtaAd)
                      .HasColumnName("orta_ad")
                      .HasMaxLength(50);

                entity.Property(e => e.Soyad)
                      .HasColumnName("soyad")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.KurumEposta)
                      .HasColumnName("kurum_eposta")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.KurumSicilNo)
                      .HasColumnName("kurum_sicil_no")
                      .HasMaxLength(20);

                entity.Property(e => e.ParolaHash)
                      .HasColumnName("parola_hash")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.ParolaTuz)
                      .HasColumnName("parola_tuz")
                      .HasMaxLength(64);

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                // constraints / indexes
                entity.HasIndex(e => e.KurumEposta).IsUnique().HasDatabaseName("UK_Kullanicilar_KurumEposta");
                entity.HasIndex(e => e.KurumSicilNo).IsUnique().HasDatabaseName("UK_Kullanicilar_KurumSicilNo");
            });

            // epostalar
            modelBuilder.Entity<Eposta>(entity =>
            {
                entity.ToTable("epostalar");
                entity.HasKey(e => e.EpostaUuid);

                entity.Property(e => e.EpostaUuid)
                      .HasColumnName("eposta_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.KullaniciUuid)
                      .HasColumnName("kullanici_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.EpostaAdresi)
                      .HasColumnName("eposta_adresi")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.EpostaTipi)
                      .HasColumnName("eposta_tipi")
                      .HasColumnType("int")
                      .HasConversion<int>()
                      .HasDefaultValue(EpostaTipi.IS);

                entity.Property(e => e.Oncelikli)
                      .HasColumnName("oncelikli")
                      .HasDefaultValue(false);

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => e.EpostaAdresi).IsUnique().HasDatabaseName("UK_Epostalar_EpostaAdresi");
                entity.HasIndex(e => new { e.KullaniciUuid, e.Oncelikli }).IsUnique().HasDatabaseName("UK_OncelikliEposta");
                entity.HasOne<Kullanici>().WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Epostalar_Kullanicilar");
            });

            // telefonlar
            modelBuilder.Entity<Telefon>(entity =>
            {
                entity.ToTable("telefonlar");
                entity.HasKey(e => e.TelefonUuid);

                entity.Property(e => e.TelefonUuid)
                      .HasColumnName("telefon_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.KullaniciUuid)
                      .HasColumnName("kullanici_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.UlkeKodu)
                      .HasColumnName("ulke_kodu")
                      .HasMaxLength(5)
                      .IsRequired();

                entity.Property(e => e.TelefonNo)
                      .HasColumnName("telefon_numarasi")
                      .HasMaxLength(15)
                      .IsRequired();

                entity.Property(e => e.TelefonTipi)
                      .HasColumnName("telefon_tipi")
                      .HasColumnType("int")
                      .HasConversion<int>()
                      .HasDefaultValue(TelefonTipi.IS);

                entity.Property(e => e.Oncelikli)
                      .HasColumnName("oncelikli")
                      .HasDefaultValue(false);

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => new { e.KullaniciUuid, e.TelefonNo }).IsUnique().HasDatabaseName("UK_Telefonlar_Kullanici_Telefon");
                entity.HasIndex(e => new { e.KullaniciUuid, e.Oncelikli }).IsUnique().HasDatabaseName("UK_OncelikliTelefon");
                entity.HasOne<Kullanici>().WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Telefonlar_Kullanicilar");
            });

            // adresler
            modelBuilder.Entity<Adres>(entity =>
            {
                entity.ToTable("adresler");
                entity.HasKey(e => e.AdresUuid);

                entity.Property(e => e.AdresUuid)
                      .HasColumnName("adres_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.KullaniciUuid)
                      .HasColumnName("kullanici_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.Sokak)
                      .HasColumnName("sokak")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Sehir)
                      .HasColumnName("sehir")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Ilce)
                      .HasColumnName("ilce")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.PostaKodu)
                      .HasColumnName("posta_kodu")
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Ulke)
                      .HasColumnName("ulke")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Oncelikli)
                      .HasColumnName("oncelikli")
                      .HasDefaultValue(false);

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => new { e.KullaniciUuid, e.Oncelikli }).IsUnique().HasDatabaseName("UK_OncelikliAdres");
                entity.HasOne<Kullanici>().WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Adresler_Kullanicilar");
            });

            // dersler
            modelBuilder.Entity<Ders>(entity =>
            {
                entity.ToTable("dersler");
                entity.HasKey(e => e.DersUuid);

                entity.Property(e => e.DersUuid)
                      .HasColumnName("ders_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.DersKodu)
                      .HasColumnName("ders_kodu")
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.DersAdi)
                      .HasColumnName("ders_adi")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Aciklama)
                      .HasColumnName("aciklama");

                entity.Property(e => e.HaftalikDersSaati)
                      .HasColumnName("haftalik_ders_saati")
                      .IsRequired();

                entity.Property(e => e.Kredi)
                      .HasColumnName("kredi")
                      .IsRequired();

                entity.Property(e => e.Akts)
                      .HasColumnName("akts")
                      .IsRequired();

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => e.DersKodu).IsUnique().HasDatabaseName("UK_Dersler_DersKodu");
            });

            // akademisyen_ders_atamalari
            modelBuilder.Entity<AkademisyenDersAtama>(entity =>
            {
                entity.ToTable("akademisyen_ders_atamalari");
                entity.HasKey(e => e.AtamaUuid);

                entity.Property(e => e.AtamaUuid)
                      .HasColumnName("atama_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.AkademisyenUuid)
                      .HasColumnName("akademisyen_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.DersUuid)
                      .HasColumnName("ders_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => new { e.AkademisyenUuid, e.DersUuid }).IsUnique().HasDatabaseName("UK_Akademisyen_Ders");
                entity.HasOne<Kullanici>().WithMany().HasForeignKey(e => e.AkademisyenUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_AkademisyenAtama_Kullanicilar");
                entity.HasOne<Ders>().WithMany().HasForeignKey(e => e.DersUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_AkademisyenAtama_Dersler");
            });

            // ogrenci_kayitlari
            modelBuilder.Entity<OgrenciKayit>(entity =>
            {
                entity.ToTable("ogrenci_kayitlari");
                entity.HasKey(e => e.KayitUuid);

                entity.Property(e => e.KayitUuid)
                      .HasColumnName("kayit_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.OgrenciUuid)
                      .HasColumnName("ogrenci_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.DersUuid)
                      .HasColumnName("ders_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
      .HasColumnName("guncelleme_tarihi")
      .HasColumnType("timestamp")
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAddOrUpdate();

                // enum Durum default PASIF -> underlying int assumed 0
                entity.Property(e => e.Durum)
                      .HasColumnName("durum")
                      .HasColumnType("int")
                      .HasConversion<int>()
                      .HasDefaultValue(Durum.PASIF);

                entity.HasIndex(e => new { e.OgrenciUuid, e.DersUuid }).IsUnique().HasDatabaseName("UK_Ogrenci_Ders");
                entity.HasOne<Kullanici>().WithMany().HasForeignKey(e => e.OgrenciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OgrenciKayit_Kullanicilar");
                entity.HasOne<Ders>().WithMany().HasForeignKey(e => e.DersUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OgrenciKayit_Dersler");
            });

            // sinavlar
            modelBuilder.Entity<Sinav>(entity =>
            {
                entity.ToTable("sinavlar");
                entity.HasKey(e => e.SinavUuid);

                entity.Property(e => e.SinavUuid)
                      .HasColumnName("sinav_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.DersUuid)
                      .HasColumnName("ders_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.SinavTipi)
                      .HasColumnName("sinav_tipi")
                      .HasColumnType("int")
                      .HasConversion<int>()
                      .IsRequired();

                entity.Property(e => e.SinavTarih)
                      .HasColumnName("sinav_tarihi")
                      .HasColumnType("date")
                      .IsRequired();

                entity.Property(e => e.ToplamPuan)
                      .HasColumnName("toplam_puan")
                      .IsRequired();

                entity.Property(e => e.SinavAgirligi)
                      .HasColumnName("sinav_agirligi")
                      .HasColumnType("decimal(5,2)")
                      .IsRequired();

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                entity.HasOne<Ders>().WithMany().HasForeignKey(e => e.DersUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Sinavlar_Dersler");
            });

            // notlar
            modelBuilder.Entity<Not>(entity =>
            {
                entity.ToTable("notlar");
                entity.HasKey(e => e.NotUuid);

                entity.Property(e => e.NotUuid)
                      .HasColumnName("not_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.SinavUuid)
                      .HasColumnName("sinav_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.OgrenciUuid)
                      .HasColumnName("ogrenci_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.AlinanPuan)
                      .HasColumnName("alinan_puan")
                      .IsRequired();

                entity.Property(e => e.OlusturmaTarihi)
                      .HasColumnName("olusturma_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.GuncellemeTarihi)
                      .HasColumnName("guncelleme_tarihi")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => new { e.SinavUuid, e.OgrenciUuid }).IsUnique().HasDatabaseName("UK_Notlar_Sinav_Ogrenci");
                entity.HasOne<Sinav>().WithMany().HasForeignKey(e => e.SinavUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Notlar_Sinavlar");
                entity.HasOne<Kullanici>().WithMany().HasForeignKey(e => e.OgrenciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Notlar_Kullanicilar");
            });
        }
    }
}
