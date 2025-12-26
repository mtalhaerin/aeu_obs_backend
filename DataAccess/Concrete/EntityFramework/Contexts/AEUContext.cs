using Entities.Concrete.DersEntities;
using Entities.Concrete.FakulteEntities;
using Entities.Concrete.OzlukEntities;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using Core.Entities.Concrete.YetkiEntities;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class AEUContext : DbContext
    {
        public AEUContext() : base()
        {

        }
        public AEUContext(DbContextOptions<AEUContext> options) : base(options)
        {
        }

        // Entity Database tablo eşleltirme 

        #region Ozluk Entity'leri
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<Eposta> Epostalar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Telefon> Telefonlar { get; set; }
        #endregion

        #region Yetki Entity'leri (Core)
        public DbSet<KullaniciIslemYetkisi> KullaniciIslemYetkileri { get; set; }
        public DbSet<IslemYetkisi> IslemYetkileri { get; set; }
        #endregion

        #region Fakulte Entity'leri
        public DbSet<Fakulte> Fakulteler { get; set; }
        public DbSet<AnaDal> AnaDallar { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<AkademisyenBolumAtama> AkademisyenBolumAtamalari { get; set; }
        public DbSet<OgrenciBolumKayit> OgrenciBolumKayitlari { get; set; }
        #endregion

        #region Ders Entity'leri
        public DbSet<AkademisyenDersAtama> AkademisyenDersAtamalari { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Not> Notlar { get; set; }
        public DbSet<OgrenciDersKayit> OgrenciDersKayitlari { get; set; }
        public DbSet<Sinav> Sinavlar { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=100.64.207.39;database=aeu_obs;user=mustafa;password=Trabzon1967.Trabzon1967.",
                    new MySqlServerVersion(new Version(8, 0, 44)));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureOzlukEntities(modelBuilder);
            ConfigureYetkiEntities(modelBuilder);
            ConfigureFakulteEntities(modelBuilder);
            ConfigureDersEntities(modelBuilder);

        }

        private void ConfigureYetkiEntities(ModelBuilder modelBuilder)
        {
            // --- ISLEM YETKILERI ---
            modelBuilder.Entity<IslemYetkisi>(entity =>
            {
                entity.ToTable("islem_yetkileri");
                entity.HasKey(e => e.IslemYetkisiUuid);

                entity.Property(e => e.IslemYetkisiUuid)
                      .HasColumnName("islem_yetkisi_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.YetkiAdi)
                      .HasColumnName("yetki_adi")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Aciklama)
                      .HasColumnName("aciklama")
                      .HasColumnType("TEXT");

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
            });

            // --- KULLANICI ISLEM YETKISI ---
            modelBuilder.Entity<KullaniciIslemYetkisi>(entity =>
            {
                entity.ToTable("kullanici_islem_yetkisi");
                entity.HasKey(e => e.YetkiAtamaUuid);

                entity.Property(e => e.YetkiAtamaUuid)
                      .HasColumnName("yetki_atama_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.KullaniciUuid)
                      .HasColumnName("kullanici_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.YetkiVerenUuid)
                      .HasColumnName("yetki_veren_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.IslemYetkisiUuid)
                      .HasColumnName("islem_yetkisi_uuid")
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

                // --- İLİŞKİLER (FIX OLAN KISIM) ---

                // 1. Yetkiyi ALAN Kullanıcı İlişkisi
                // Kullanıcı sınıfındaki "KullaniciIslemYetkileri" listesi, o kullanıcının SAHİP OLDUĞU yetkileri gösterir.
                entity.HasOne(e => e.Kullanici)
                      .WithMany(k => k.KullaniciIslemYetkileri)
                      .HasForeignKey(e => e.KullaniciUuid)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_KullaniciIslemYetkisi_Kullanicilar");

                // 2. Yetkiyi VEREN Kullanıcı İlişkisi
                // DİKKAT: Buradaki .WithMany() içini BOŞ bırakıyoruz.
                // Eğer buraya da (k => k.KullaniciIslemYetkileri) yazarsanız döngü oluşur ve hata alırsınız.
                entity.HasOne<Kullanici>()
                      .WithMany() // <-- BOŞ BIRAKILDI
                      .HasForeignKey(e => e.YetkiVerenUuid)
                      .OnDelete(DeleteBehavior.Restrict) // Yetki veren silinirse kayıtlar bozulmasın
                      .HasConstraintName("FK_KullaniciIslemYetkisi_YetkiVeren");

                // 3. İşlem Yetkisi İlişkisi
                entity.HasOne(e => e.IslemYetkisi)
                      .WithMany(i => i.KullaniciIslemYetkileri)
                      .HasForeignKey(e => e.IslemYetkisiUuid)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_KullaniciIslemYetkisi_IslemYetkileri");

                // İndeksler
                entity.HasIndex(e => e.KullaniciUuid).HasDatabaseName("IX_KullaniciIslemYetkisi_KullaniciUuid");
                entity.HasIndex(e => e.YetkiVerenUuid).HasDatabaseName("IX_KullaniciIslemYetkisi_YetkiVerenUuid");
                entity.HasIndex(e => e.IslemYetkisiUuid).HasDatabaseName("IX_KullaniciIslemYetkisi_IslemYetkisiUuid");
            });
        }
        private void ConfigureOzlukEntities(ModelBuilder modelBuilder)
        {
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
                      .HasDefaultValue(false)
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

                entity.HasIndex(e => new { e.KullaniciUuid, e.Oncelikli }).IsUnique().HasDatabaseName("UK_OncelikliAdres");
                entity.HasOne(e => e.Kullanici).WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Adresler_Kullanicilar");
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
                      .HasColumnType("ENUM('KISISEL','IS','DIGER')")
                      .HasConversion<string>()
                      .HasDefaultValueSql("'KISISEL'");

                entity.Property(e => e.Oncelikli)
                      .HasColumnName("oncelikli")
                      .HasDefaultValue(false)
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

                entity.HasIndex(e => e.EpostaAdresi).IsUnique().HasDatabaseName("UK_Epostalar_EpostaAdresi");
                entity.HasIndex(e => new { e.KullaniciUuid, e.Oncelikli }).IsUnique().HasDatabaseName("UK_OncelikliEposta");
                entity.HasOne(e => e.Kullanici).WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Epostalar_Kullanicilar");
            });

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
                      .HasConversion<string>()
                      .IsRequired()
                      .HasDefaultValueSql("'OGRENCI'");

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
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.ParolaHash)
                      .HasColumnName("parola_hash")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.ParolaTuz)
                      .HasColumnName("parola_tuz")
                      .HasMaxLength(255)
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

                // constraints / indexes
                entity.HasIndex(e => e.KurumEposta).IsUnique().HasDatabaseName("UK_Kullanicilar_KurumEposta");
                entity.HasIndex(e => e.KurumSicilNo).IsUnique().HasDatabaseName("UK_Kullanicilar_KurumSicilNo");
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
                      .HasColumnType("ENUM('CEP','EV','IS','DIGER')")
                      .HasConversion<string>()
                      .HasDefaultValueSql("'CEP'");

                entity.Property(e => e.Oncelikli)
                      .HasColumnName("oncelikli")
                      .HasDefaultValue(false)
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

                entity.HasIndex(e => new { e.KullaniciUuid, e.TelefonNo }).IsUnique().HasDatabaseName("UK_Telefonlar_Kullanici_Telefon");
                entity.HasIndex(e => new { e.KullaniciUuid, e.Oncelikli }).IsUnique().HasDatabaseName("UK_OncelikliTelefon");
                entity.HasOne(e => e.Kullanici).WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Telefonlar_Kullanicilar");
            });

        }
        private void ConfigureFakulteEntities(ModelBuilder modelBuilder)
        {
            // fakulteler
            modelBuilder.Entity<Fakulte>(entity =>
            {
                entity.ToTable("fakulteler");
                entity.HasKey(e => e.FakulteUuid);

                entity.Property(e => e.FakulteUuid)
                      .HasColumnName("fakulte_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.FakulteAdi)
                      .HasColumnName("fakulte_ad")
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(e => e.WebAdres)
                      .HasColumnName("web_adres")
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(e => e.KurulusTarihi)
                      .HasColumnName("kurulus_tarihi")
                      .HasColumnType("date")
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
            });

            // ana_dallar
            modelBuilder.Entity<AnaDal>(entity =>
            {
                entity.ToTable("ana_dallar");
                entity.HasKey(e => e.AnaDalUuid);

                entity.Property(e => e.AnaDalUuid)
                      .HasColumnName("ana_dal_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.AnaDalAdi)
                      .HasColumnName("ana_dal_ad")
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(e => e.FakulteUuid)
                      .HasColumnName("fakulte_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.KurulusTarihi)
                      .HasColumnName("kurulus_tarihi")
                      .HasColumnType("date")
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

                entity.HasOne(e => e.Fakulte).WithMany().HasForeignKey(e => e.FakulteUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_AnaDallar_Fakulteler");
            });

            // bolumler
            modelBuilder.Entity<Bolum>(entity =>
            {
                entity.ToTable("bolumler");
                entity.HasKey(e => e.BolumUuid);

                entity.Property(e => e.BolumUuid)
                      .HasColumnName("bolum_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.BolumAdi)
                      .HasColumnName("bolum_ad")
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(e => e.AnaDalUuid)
                      .HasColumnName("ana_dal_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.KurulusTarihi)
                      .HasColumnName("kurulus_tarihi")
                      .HasColumnType("date")
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

                entity.HasOne<AnaDal>().WithMany().HasForeignKey(e => e.AnaDalUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Bolumler_AnaDallar");
            });

            // akademisyen_bolum_atamalari
            modelBuilder.Entity<AkademisyenBolumAtama>(entity =>
            {
                entity.ToTable("akademisyen_bolum_atamalari");
                entity.HasKey(e => e.AtamaUuid);

                entity.Property(e => e.AtamaUuid)
                      .HasColumnName("bolum_atama_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.KullaniciUuid)
                      .HasColumnName("kullanici_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.BolumUuid)
                      .HasColumnName("bolum_uuid")
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

                entity.HasIndex(e => new { e.KullaniciUuid, e.BolumUuid }).IsUnique().HasDatabaseName("UK_AkademisyenBolumAtama_Kullanici_Bolum");
                entity.HasOne(e => e.Akademisyen).WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_AkademisyenBolumAtama_Kullanicilar");
                entity.HasOne(e => e.Bolum).WithMany().HasForeignKey(e => e.BolumUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_AkademisyenBolumAtama_Bolumler");
            });

            // ogrenci_bolum_kayitlari
            modelBuilder.Entity<OgrenciBolumKayit>(entity =>
            {
                entity.ToTable("ogrenci_bolum_kayitlari");
                entity.HasKey(e => e.BolumKayitUuid);

                entity.Property(e => e.BolumKayitUuid)
                      .HasColumnName("bolum_kayit_uuid")
                      .HasMaxLength(36)
                      .IsRequired()
                      .HasDefaultValueSql("UUID()");

                entity.Property(e => e.KullaniciUuid)
                      .HasColumnName("kullanici_uuid")
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.BolumUuid)
                      .HasColumnName("bolum_uuid")
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

                entity.HasIndex(e => new { e.KullaniciUuid, e.BolumUuid }).IsUnique().HasDatabaseName("UK_OgrenciBolumKayit_Kullanici_Bolum");
                entity.HasOne(e => e.Ogrenci).WithMany().HasForeignKey(e => e.KullaniciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OgrenciBolumKayit_Kullanicilar");
                entity.HasOne(e => e.Bolum).WithMany().HasForeignKey(e => e.BolumUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OgrenciBolumKayit_Bolumler");
            });
        }
        private void ConfigureDersEntities(ModelBuilder modelBuilder)
        {
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
                entity.HasOne(e => e.Akademisyen).WithMany().HasForeignKey(e => e.AkademisyenUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_AkademisyenAtama_Kullanicilar");
                entity.HasOne(e => e.Ders).WithMany().HasForeignKey(e => e.DersUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_AkademisyenAtama_Dersler");
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
                      .IsRequired()
                      .HasDefaultValue(0);

                entity.Property(e => e.Kredi)
                      .HasColumnName("kredi")
                      .IsRequired()
                      .HasDefaultValue(0);

                entity.Property(e => e.Akts)
                      .HasColumnName("akts")
                      .IsRequired()
                      .HasDefaultValue(0);

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
                      .IsRequired()
                      .HasDefaultValueSql("0");

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
                entity.HasOne(e => e.Sinav).WithMany().HasForeignKey(e => e.SinavUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Notlar_Sinavlar");
                entity.HasOne(e => e.Ogrenci).WithMany().HasForeignKey(e => e.OgrenciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Notlar_Kullanicilar");
            });

            // ogrenci_kayitlari
            modelBuilder.Entity<OgrenciDersKayit>(entity =>
            {
                entity.ToTable("ogrenci_ders_kayitlari");
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
                
                // enum Durum default PASIF -> underlying int assumed 0
                entity.Property(e => e.Durum)
                      .HasColumnName("durum")
                      .HasColumnType("ENUM('DEVAMEDIYOR','GECTI','KALDI')")
                      .HasConversion<string>()
                      .IsRequired()
                      .HasDefaultValue(Durum.DEVAMEDIYOR);
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

                

                entity.HasIndex(e => new { e.OgrenciUuid, e.DersUuid }).IsUnique().HasDatabaseName("UK_Ogrenci_Ders");
                entity.HasOne(e => e.Ogrenci).WithMany().HasForeignKey(e => e.OgrenciUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OgrenciKayit_Kullanicilar");
                entity.HasOne(e => e.Ders).WithMany().HasForeignKey(e => e.DersUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OgrenciKayit_Dersler");
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
                      .HasColumnType("ENUM('QUIZ','VIZE','FINAL','PROJE','BUTUNLEME')")
                      .HasConversion<string>()
                      .IsRequired()
                      .HasDefaultValueSql("'QUIZ'");

                entity.Property(e => e.SinavTarih)
                      .HasColumnName("sinav_tarihi")
                      .HasColumnType("date")
                      .IsRequired()
                      .HasDefaultValueSql("UTC_DATE()");

                entity.Property(e => e.ToplamPuan)
                      .HasColumnName("toplam_puan")
                      .IsRequired()
                      .HasDefaultValueSql("100");

                entity.Property(e => e.SinavAgirligi)
                      .HasColumnName("sinav_agirligi")
                      .HasColumnType("decimal(5,2)")
                      .IsRequired()
                      .HasDefaultValueSql("0.00");

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

                entity.HasOne(e => e.Ders).WithMany().HasForeignKey(e => e.DersUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Sinavlar_Dersler");
            });
        }

    }
}
