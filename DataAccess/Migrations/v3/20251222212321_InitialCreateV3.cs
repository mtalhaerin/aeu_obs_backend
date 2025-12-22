using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.v3
{
    /// <inheritdoc />
    public partial class InitialCreateV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "islem_yetkileri",
                columns: table => new
                {
                    islem_yetkisi_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    yetki_adi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    aciklama = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_islem_yetkileri", x => x.islem_yetkisi_uuid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "kullanici_islem_yetkisi",
                columns: table => new
                {
                    yetki_atama_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, defaultValueSql: "UUID()", collation: "ascii_general_ci"),
                    kullanici_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    yetki_veren_uuid = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    IslemYetkisiUuid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    KullaniciUuid1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullanici_islem_yetkisi", x => x.yetki_atama_uuid);
                    table.ForeignKey(
                        name: "FK_KullaniciIslemYetkisi_IslemYetkileri",
                        column: x => x.IslemYetkisiUuid,
                        principalTable: "islem_yetkileri",
                        principalColumn: "islem_yetkisi_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciIslemYetkisi_Kullanicilar",
                        column: x => x.kullanici_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciIslemYetkisi_YetkiVeren",
                        column: x => x.yetki_veren_uuid,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_kullanici_islem_yetkisi_kullanicilar_KullaniciUuid1",
                        column: x => x.KullaniciUuid1,
                        principalTable: "kullanicilar",
                        principalColumn: "kullanici_uuid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_kullanici_islem_yetkisi_IslemYetkisiUuid",
                table: "kullanici_islem_yetkisi",
                column: "IslemYetkisiUuid");

            migrationBuilder.CreateIndex(
                name: "IX_kullanici_islem_yetkisi_KullaniciUuid1",
                table: "kullanici_islem_yetkisi",
                column: "KullaniciUuid1");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciIslemYetkisi_KullaniciUuid",
                table: "kullanici_islem_yetkisi",
                column: "kullanici_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciIslemYetkisi_YetkiVerenUuid",
                table: "kullanici_islem_yetkisi",
                column: "yetki_veren_uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kullanici_islem_yetkisi");

            migrationBuilder.DropTable(
                name: "islem_yetkileri");
        }
    }
}
