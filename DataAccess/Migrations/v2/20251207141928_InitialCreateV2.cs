using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.v2
{
    /// <inheritdoc />
    public partial class InitialCreateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dersler",
                columns: table => new
                {
                    ders_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    ders_kodu = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ders_adi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    haftalik_ders_saati = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    kredi = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    akts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dersler", x => x.ders_uuid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fakulteler",
                columns: table => new
                {
                    fakulte_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    fakulte_ad = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    web_adres = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    kurulus_tarihi = table.Column<DateTime>(type: "date", nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fakulteler", x => x.fakulte_uuid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "kullanicilar",
                columns: table => new
                {
                    kullanici_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    kullanici_tipi = table.Column<string>(type: "ENUM('OGRENCI', 'AKADEMISYEN', 'PERSONEL')", nullable: false, defaultValueSql: "'OGRENCI'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orta_ad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    soyad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    kurum_eposta = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    kurum_sicil_no = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parola_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parola_tuz = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullanicilar", x => x.kullanici_uuid);
                    table.CheckConstraint("CHK_Kullanicilar_KurumEpostaDomain", "kurum_eposta LIKE '%@ahievran.edu.tr'");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sinavlar",
                columns: table => new
                {
                    sinav_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    ders_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    sinav_tipi = table.Column<string>(type: "ENUM('QUIZ','VIZE','FINAL','PROJE','BUTUNLEME')", nullable: false, defaultValueSql: "'QUIZ'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sinav_tarihi = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "UTC_DATE()"),
                    toplam_puan = table.Column<int>(type: "int", nullable: false, defaultValueSql: "100"),
                    sinav_agirligi = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValueSql: "0.00"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    DersUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sinavlar", x => x.sinav_uuid);
                    table.ForeignKey(
                        name: "FK_Sinavlar_Dersler",
                        column: x => x.ders_uuid,
                        principalTable: "dersler",
                        principalColumn: "ders_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sinavlar_dersler_DersUuid1",
                        column: x => x.DersUuid1,
                        principalTable: "dersler",
                        principalColumn: "ders_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ana_dallar",
                columns: table => new
                {
                    ana_dal_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    ana_dal_ad = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fakulte_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    kurulus_tarihi = table.Column<DateTime>(type: "date", nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    FakulteUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ana_dallar", x => x.ana_dal_uuid);
                    table.ForeignKey(
                        name: "FK_AnaDallar_Fakulteler",
                        column: x => x.fakulte_uuid,
                        principalTable: "fakulteler",
                        principalColumn: "fakulte_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ana_dallar_fakulteler_FakulteUuid1",
                        column: x => x.FakulteUuid1,
                        principalTable: "fakulteler",
                        principalColumn: "fakulte_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "adresler",
                columns: table => new
                {
                    adres_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    kullanici_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    sokak = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sehir = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ilce = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    posta_kodu = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ulke = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    oncelikli = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    KullaniciUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adresler", x => x.adres_uuid);
                    table.ForeignKey(
                        name: "FK_Adresler_Kullanicilar",
                        column: x => x.kullanici_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_adresler_kullanicilar_KullaniciUuid1",
                        column: x => x.KullaniciUuid1,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "akademisyen_ders_atamalari",
                columns: table => new
                {
                    atama_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    akademisyen_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ders_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    AkademisyenKullaniciUuid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DersUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_akademisyen_ders_atamalari", x => x.atama_uuid);
                    table.ForeignKey(
                        name: "FK_AkademisyenAtama_Dersler",
                        column: x => x.ders_uuid,
                        principalTable: "dersler",
                        principalColumn: "ders_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkademisyenAtama_Kullanicilar",
                        column: x => x.akademisyen_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_akademisyen_ders_atamalari_dersler_DersUuid1",
                        column: x => x.DersUuid1,
                        principalTable: "dersler",
                        principalColumn: "ders_uuid");
                    table.ForeignKey(
                        name: "FK_akademisyen_ders_atamalari_kullanicilar_AkademisyenKullanici~",
                        column: x => x.AkademisyenKullaniciUuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "epostalar",
                columns: table => new
                {
                    eposta_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    kullanici_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    eposta_adresi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eposta_tipi = table.Column<string>(type: "ENUM('KISISEL','IS','DIGER')", nullable: false, defaultValueSql: "'KISISEL'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    oncelikli = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    KullaniciUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_epostalar", x => x.eposta_uuid);
                    table.ForeignKey(
                        name: "FK_Epostalar_Kullanicilar",
                        column: x => x.kullanici_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_epostalar_kullanicilar_KullaniciUuid1",
                        column: x => x.KullaniciUuid1,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ogrenci_ders_kayitlari",
                columns: table => new
                {
                    kayit_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    ogrenci_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ders_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    durum = table.Column<string>(type: "ENUM('DEVAMEDIYOR','GECTI','KALDI')", nullable: false, defaultValue: "DEVAMEDIYOR")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OgrenciKullaniciUuid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DersUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ogrenci_ders_kayitlari", x => x.kayit_uuid);
                    table.ForeignKey(
                        name: "FK_OgrenciKayit_Dersler",
                        column: x => x.ders_uuid,
                        principalTable: "dersler",
                        principalColumn: "ders_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OgrenciKayit_Kullanicilar",
                        column: x => x.ogrenci_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ogrenci_ders_kayitlari_dersler_DersUuid1",
                        column: x => x.DersUuid1,
                        principalTable: "dersler",
                        principalColumn: "ders_uuid");
                    table.ForeignKey(
                        name: "FK_ogrenci_ders_kayitlari_kullanicilar_OgrenciKullaniciUuid",
                        column: x => x.OgrenciKullaniciUuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "telefonlar",
                columns: table => new
                {
                    telefon_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    kullanici_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ulke_kodu = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefon_numarasi = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefon_tipi = table.Column<string>(type: "ENUM('CEP','EV','IS','DIGER')", nullable: false, defaultValueSql: "'CEP'")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    oncelikli = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    KullaniciUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telefonlar", x => x.telefon_uuid);
                    table.ForeignKey(
                        name: "FK_Telefonlar_Kullanicilar",
                        column: x => x.kullanici_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_telefonlar_kullanicilar_KullaniciUuid1",
                        column: x => x.KullaniciUuid1,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notlar",
                columns: table => new
                {
                    not_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    sinav_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ogrenci_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    alinan_puan = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    SinavUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    OgrenciKullaniciUuid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notlar", x => x.not_uuid);
                    table.ForeignKey(
                        name: "FK_Notlar_Kullanicilar",
                        column: x => x.ogrenci_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notlar_Sinavlar",
                        column: x => x.sinav_uuid,
                        principalTable: "sinavlar",
                        principalColumn: "sinav_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notlar_kullanicilar_OgrenciKullaniciUuid",
                        column: x => x.OgrenciKullaniciUuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                    table.ForeignKey(
                        name: "FK_notlar_sinavlar_SinavUuid1",
                        column: x => x.SinavUuid1,
                        principalTable: "sinavlar",
                        principalColumn: "sinav_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bolumler",
                columns: table => new
                {
                    bolum_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    bolum_ad = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ana_dal_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    kurulus_tarihi = table.Column<DateTime>(type: "date", nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    AnaDalUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bolumler", x => x.bolum_uuid);
                    table.ForeignKey(
                        name: "FK_Bolumler_AnaDallar",
                        column: x => x.ana_dal_uuid,
                        principalTable: "ana_dallar",
                        principalColumn: "ana_dal_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bolumler_ana_dallar_AnaDalUuid1",
                        column: x => x.AnaDalUuid1,
                        principalTable: "ana_dallar",
                        principalColumn: "ana_dal_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "akademisyen_bolum_atamalari",
                columns: table => new
                {
                    bolum_atama_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    kullanici_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    bolum_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    AkademisyenKullaniciUuid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BolumUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_akademisyen_bolum_atamalari", x => x.bolum_atama_uuid);
                    table.ForeignKey(
                        name: "FK_AkademisyenBolumAtama_Bolumler",
                        column: x => x.bolum_uuid,
                        principalTable: "bolumler",
                        principalColumn: "bolum_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkademisyenBolumAtama_Kullanicilar",
                        column: x => x.kullanici_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_akademisyen_bolum_atamalari_bolumler_BolumUuid1",
                        column: x => x.BolumUuid1,
                        principalTable: "bolumler",
                        principalColumn: "bolum_uuid");
                    table.ForeignKey(
                        name: "FK_akademisyen_bolum_atamalari_kullanicilar_AkademisyenKullanic~",
                        column: x => x.AkademisyenKullaniciUuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ogrenci_bolum_kayitlari",
                columns: table => new
                {
                    bolum_kayit_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    kullanici_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    bolum_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    OgrenciKullaniciUuid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    BolumUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ogrenci_bolum_kayitlari", x => x.bolum_kayit_uuid);
                    table.ForeignKey(
                        name: "FK_OgrenciBolumKayit_Bolumler",
                        column: x => x.bolum_uuid,
                        principalTable: "bolumler",
                        principalColumn: "bolum_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OgrenciBolumKayit_Kullanicilar",
                        column: x => x.kullanici_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ogrenci_bolum_kayitlari_bolumler_BolumUuid1",
                        column: x => x.BolumUuid1,
                        principalTable: "bolumler",
                        principalColumn: "bolum_uuid");
                    table.ForeignKey(
                        name: "FK_ogrenci_bolum_kayitlari_kullanicilar_OgrenciKullaniciUuid",
                        column: x => x.OgrenciKullaniciUuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_adresler_KullaniciUuid1",
                table: "adresler",
                column: "KullaniciUuid1");

            migrationBuilder.CreateIndex(
                name: "UK_OncelikliAdres",
                table: "adresler",
                columns: new[] { "kullanici_uuid", "oncelikli" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_akademisyen_bolum_atamalari_AkademisyenKullaniciUuid",
                table: "akademisyen_bolum_atamalari",
                column: "AkademisyenKullaniciUuid");

            migrationBuilder.CreateIndex(
                name: "IX_akademisyen_bolum_atamalari_bolum_uuid",
                table: "akademisyen_bolum_atamalari",
                column: "bolum_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_akademisyen_bolum_atamalari_BolumUuid1",
                table: "akademisyen_bolum_atamalari",
                column: "BolumUuid1");

            migrationBuilder.CreateIndex(
                name: "UK_AkademisyenBolumAtama_Kullanici_Bolum",
                table: "akademisyen_bolum_atamalari",
                columns: new[] { "kullanici_uuid", "bolum_uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_akademisyen_ders_atamalari_AkademisyenKullaniciUuid",
                table: "akademisyen_ders_atamalari",
                column: "AkademisyenKullaniciUuid");

            migrationBuilder.CreateIndex(
                name: "IX_akademisyen_ders_atamalari_ders_uuid",
                table: "akademisyen_ders_atamalari",
                column: "ders_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_akademisyen_ders_atamalari_DersUuid1",
                table: "akademisyen_ders_atamalari",
                column: "DersUuid1");

            migrationBuilder.CreateIndex(
                name: "UK_Akademisyen_Ders",
                table: "akademisyen_ders_atamalari",
                columns: new[] { "akademisyen_uuid", "ders_uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ana_dallar_fakulte_uuid",
                table: "ana_dallar",
                column: "fakulte_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_ana_dallar_FakulteUuid1",
                table: "ana_dallar",
                column: "FakulteUuid1");

            migrationBuilder.CreateIndex(
                name: "IX_bolumler_ana_dal_uuid",
                table: "bolumler",
                column: "ana_dal_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_bolumler_AnaDalUuid1",
                table: "bolumler",
                column: "AnaDalUuid1");

            migrationBuilder.CreateIndex(
                name: "UK_Dersler_DersKodu",
                table: "dersler",
                column: "ders_kodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_epostalar_KullaniciUuid1",
                table: "epostalar",
                column: "KullaniciUuid1");

            migrationBuilder.CreateIndex(
                name: "UK_Epostalar_EpostaAdresi",
                table: "epostalar",
                column: "eposta_adresi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_OncelikliEposta",
                table: "epostalar",
                columns: new[] { "kullanici_uuid", "oncelikli" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Kullanicilar_KurumEposta",
                table: "kullanicilar",
                column: "kurum_eposta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Kullanicilar_KurumSicilNo",
                table: "kullanicilar",
                column: "kurum_sicil_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notlar_ogrenci_uuid",
                table: "notlar",
                column: "ogrenci_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_notlar_OgrenciKullaniciUuid",
                table: "notlar",
                column: "OgrenciKullaniciUuid");

            migrationBuilder.CreateIndex(
                name: "IX_notlar_SinavUuid1",
                table: "notlar",
                column: "SinavUuid1");

            migrationBuilder.CreateIndex(
                name: "UK_Notlar_Sinav_Ogrenci",
                table: "notlar",
                columns: new[] { "sinav_uuid", "ogrenci_uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_bolum_kayitlari_bolum_uuid",
                table: "ogrenci_bolum_kayitlari",
                column: "bolum_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_bolum_kayitlari_BolumUuid1",
                table: "ogrenci_bolum_kayitlari",
                column: "BolumUuid1");

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_bolum_kayitlari_OgrenciKullaniciUuid",
                table: "ogrenci_bolum_kayitlari",
                column: "OgrenciKullaniciUuid");

            migrationBuilder.CreateIndex(
                name: "UK_OgrenciBolumKayit_Kullanici_Bolum",
                table: "ogrenci_bolum_kayitlari",
                columns: new[] { "kullanici_uuid", "bolum_uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_ders_kayitlari_ders_uuid",
                table: "ogrenci_ders_kayitlari",
                column: "ders_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_ders_kayitlari_DersUuid1",
                table: "ogrenci_ders_kayitlari",
                column: "DersUuid1");

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_ders_kayitlari_OgrenciKullaniciUuid",
                table: "ogrenci_ders_kayitlari",
                column: "OgrenciKullaniciUuid");

            migrationBuilder.CreateIndex(
                name: "UK_Ogrenci_Ders",
                table: "ogrenci_ders_kayitlari",
                columns: new[] { "ogrenci_uuid", "ders_uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sinavlar_ders_uuid",
                table: "sinavlar",
                column: "ders_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_sinavlar_DersUuid1",
                table: "sinavlar",
                column: "DersUuid1");

            migrationBuilder.CreateIndex(
                name: "IX_telefonlar_KullaniciUuid1",
                table: "telefonlar",
                column: "KullaniciUuid1");

            migrationBuilder.CreateIndex(
                name: "UK_OncelikliTelefon",
                table: "telefonlar",
                columns: new[] { "kullanici_uuid", "oncelikli" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Telefonlar_Kullanici_Telefon",
                table: "telefonlar",
                columns: new[] { "kullanici_uuid", "telefon_numarasi" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adresler");

            migrationBuilder.DropTable(
                name: "akademisyen_bolum_atamalari");

            migrationBuilder.DropTable(
                name: "akademisyen_ders_atamalari");

            migrationBuilder.DropTable(
                name: "epostalar");

            migrationBuilder.DropTable(
                name: "notlar");

            migrationBuilder.DropTable(
                name: "ogrenci_bolum_kayitlari");

            migrationBuilder.DropTable(
                name: "ogrenci_ders_kayitlari");

            migrationBuilder.DropTable(
                name: "telefonlar");

            migrationBuilder.DropTable(
                name: "sinavlar");

            migrationBuilder.DropTable(
                name: "bolumler");

            migrationBuilder.DropTable(
                name: "kullanicilar");

            migrationBuilder.DropTable(
                name: "dersler");

            migrationBuilder.DropTable(
                name: "ana_dallar");

            migrationBuilder.DropTable(
                name: "fakulteler");
        }
    }
}
