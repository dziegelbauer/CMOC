using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMOC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Unique_TypeId_Constraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EQUIPMENT_TypeId",
                table: "EQUIPMENT");

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_TypeId",
                table: "EQUIPMENT",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EQUIPMENT_TypeId",
                table: "EQUIPMENT");

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_TypeId",
                table: "EQUIPMENT",
                column: "TypeId",
                unique: true);
        }
    }
}
