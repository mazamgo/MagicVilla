using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTableNumeroVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumeroVillas_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 6, 10, 32, 16, 403, DateTimeKind.Local).AddTicks(1751), new DateTime(2024, 4, 6, 10, 32, 16, 403, DateTimeKind.Local).AddTicks(1727) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 6, 10, 32, 16, 403, DateTimeKind.Local).AddTicks(1754), new DateTime(2024, 4, 6, 10, 32, 16, 403, DateTimeKind.Local).AddTicks(1753) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3571), new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3554) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3574), new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3573) });
        }
    }
}
