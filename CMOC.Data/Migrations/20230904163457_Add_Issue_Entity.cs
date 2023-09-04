using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMOC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Issue_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IssueId",
                table: "EQUIPMENT",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IssueId",
                table: "COMPONENTS",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    ExpectedCompletion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_IssueId",
                table: "EQUIPMENT",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPONENTS_IssueId",
                table: "COMPONENTS",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_COMPONENTS_Issues_IssueId",
                table: "COMPONENTS",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EQUIPMENT_Issues_IssueId",
                table: "EQUIPMENT",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COMPONENTS_Issues_IssueId",
                table: "COMPONENTS");

            migrationBuilder.DropForeignKey(
                name: "FK_EQUIPMENT_Issues_IssueId",
                table: "EQUIPMENT");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_EQUIPMENT_IssueId",
                table: "EQUIPMENT");

            migrationBuilder.DropIndex(
                name: "IX_COMPONENTS_IssueId",
                table: "COMPONENTS");

            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "EQUIPMENT");

            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "COMPONENTS");
        }
    }
}
