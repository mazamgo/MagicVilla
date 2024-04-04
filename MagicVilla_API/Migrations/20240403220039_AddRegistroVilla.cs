using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistroVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la Villa", new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3571), new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3554), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la Villa", new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3574), new DateTime(2024, 4, 3, 16, 0, 39, 214, DateTimeKind.Local).AddTicks(3573), "", 40, "Premiun Villa Piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
