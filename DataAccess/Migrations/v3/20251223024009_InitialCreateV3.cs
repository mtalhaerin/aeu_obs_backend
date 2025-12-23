using System;
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
            migrationBuilder.RenameColumn(
                name: "IslemYetkisiUuid",
                table: "kullanici_islem_yetkisi",
                newName: "islem_yetkisi_uuid");

            migrationBuilder.RenameIndex(
                name: "IX_kullanici_islem_yetkisi_IslemYetkisiUuid",
                table: "kullanici_islem_yetkisi",
                newName: "IX_KullaniciIslemYetkisi_IslemYetkisiUuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "islem_yetkisi_uuid",
                table: "kullanici_islem_yetkisi",
                type: "char(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "islem_yetkisi_uuid",
                table: "kullanici_islem_yetkisi",
                newName: "IslemYetkisiUuid");

            migrationBuilder.RenameIndex(
                name: "IX_KullaniciIslemYetkisi_IslemYetkisiUuid",
                table: "kullanici_islem_yetkisi",
                newName: "IX_kullanici_islem_yetkisi_IslemYetkisiUuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "IslemYetkisiUuid",
                table: "kullanici_islem_yetkisi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldMaxLength: 36)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}
