using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acenta.Veri.Gocler
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Turlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Bolge = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Turu = table.Column<int>(type: "int", nullable: false),
                    Sure = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Ulasim = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Ozet = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Bulusma = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dahil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Haric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notlar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Fotograf = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Yayinda = table.Column<bool>(type: "bit", nullable: false),
                    OneCikan = table.Column<bool>(type: "bit", nullable: false),
                    Olusturma = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TurTarihleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TurId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "date", nullable: false),
                    Kontenjan = table.Column<int>(type: "int", nullable: false),
                    Ayrilan = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Not = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurTarihleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurTarihleri_Turlar_TurId",
                        column: x => x.TurId,
                        principalTable: "Turlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kayitlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TurTarihiId = table.Column<int>(type: "int", nullable: false),
                    IletisimAd = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BinisYeri = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Tutar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Alinan = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Not = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Iptal = table.Column<bool>(type: "bit", nullable: false),
                    Olusturma = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kayitlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kayitlar_TurTarihleri_TurTarihiId",
                        column: x => x.TurTarihiId,
                        principalTable: "TurTarihleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Yolcular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KayitId = table.Column<int>(type: "int", nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KimlikNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Bindi = table.Column<bool>(type: "bit", nullable: true),
                    YoklamaZamani = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yolcular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yolcular_Kayitlar_KayitId",
                        column: x => x.KayitId,
                        principalTable: "Kayitlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kayitlar_TurTarihiId",
                table: "Kayitlar",
                column: "TurTarihiId");

            migrationBuilder.CreateIndex(
                name: "IX_Turlar_Kod",
                table: "Turlar",
                column: "Kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TurTarihleri_Tarih",
                table: "TurTarihleri",
                column: "Tarih");

            migrationBuilder.CreateIndex(
                name: "IX_TurTarihleri_TurId",
                table: "TurTarihleri",
                column: "TurId");

            migrationBuilder.CreateIndex(
                name: "IX_Yolcular_KayitId",
                table: "Yolcular",
                column: "KayitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Yolcular");

            migrationBuilder.DropTable(
                name: "Kayitlar");

            migrationBuilder.DropTable(
                name: "TurTarihleri");

            migrationBuilder.DropTable(
                name: "Turlar");
        }
    }
}
