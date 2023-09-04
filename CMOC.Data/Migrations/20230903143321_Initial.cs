using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMOC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CAPABILITIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAPABILITIES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMPONENT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPONENT_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENT_REDUNDANCIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT_REDUNDANCIES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LOCATIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCATIONS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SERVICE_REDUNDANCIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SERVICE_REDUNDANCIES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SERVICES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SERVICES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    OperationalOverride = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EQUIPMENT_EQUIPMENT_TYPES_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EQUIPMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EQUIPMENT_LOCATIONS_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LOCATIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CAPABILITY_SUPPORT_RELATIONSHIPS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    CapabilityId = table.Column<int>(type: "INTEGER", nullable: false),
                    RedundantWithId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAPABILITY_SUPPORT_RELATIONSHIPS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CAPABILITY_SUPPORT_RELATIONSHIPS_CAPABILITIES_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "CAPABILITIES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CAPABILITY_SUPPORT_RELATIONSHIPS_SERVICES_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "SERVICES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CAPABILITY_SUPPORT_RELATIONSHIPS_SERVICE_REDUNDANCIES_RedundantWithId",
                        column: x => x.RedundantWithId,
                        principalTable: "SERVICE_REDUNDANCIES",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "COMPONENT_RELATIONSHIPS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    FailureThreshold = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPONENT_RELATIONSHIPS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COMPONENT_RELATIONSHIPS_COMPONENT_TYPES_TypeId",
                        column: x => x.TypeId,
                        principalTable: "COMPONENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMPONENT_RELATIONSHIPS_EQUIPMENT_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "EQUIPMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SERVICE_SUPPORT_RELATIONSHIPS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    FailureThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    RedundantWithId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SERVICE_SUPPORT_RELATIONSHIPS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SERVICE_SUPPORT_RELATIONSHIPS_EQUIPMENT_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "EQUIPMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SERVICE_SUPPORT_RELATIONSHIPS_EQUIPMENT_REDUNDANCIES_RedundantWithId",
                        column: x => x.RedundantWithId,
                        principalTable: "EQUIPMENT_REDUNDANCIES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SERVICE_SUPPORT_RELATIONSHIPS_EQUIPMENT_TYPES_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EQUIPMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SERVICE_SUPPORT_RELATIONSHIPS_SERVICES_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "SERVICES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPONENTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Operational = table.Column<bool>(type: "INTEGER", nullable: false),
                    ComponentOfId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPONENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COMPONENTS_COMPONENT_RELATIONSHIPS_ComponentOfId",
                        column: x => x.ComponentOfId,
                        principalTable: "COMPONENT_RELATIONSHIPS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMPONENTS_COMPONENT_TYPES_TypeId",
                        column: x => x.TypeId,
                        principalTable: "COMPONENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAPABILITY_SUPPORT_RELATIONSHIPS_CapabilityId",
                table: "CAPABILITY_SUPPORT_RELATIONSHIPS",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_CAPABILITY_SUPPORT_RELATIONSHIPS_RedundantWithId",
                table: "CAPABILITY_SUPPORT_RELATIONSHIPS",
                column: "RedundantWithId");

            migrationBuilder.CreateIndex(
                name: "IX_CAPABILITY_SUPPORT_RELATIONSHIPS_ServiceId",
                table: "CAPABILITY_SUPPORT_RELATIONSHIPS",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPONENT_RELATIONSHIPS_EquipmentId",
                table: "COMPONENT_RELATIONSHIPS",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPONENT_RELATIONSHIPS_TypeId",
                table: "COMPONENT_RELATIONSHIPS",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPONENTS_ComponentOfId",
                table: "COMPONENTS",
                column: "ComponentOfId");

            migrationBuilder.CreateIndex(
                name: "IX_COMPONENTS_TypeId",
                table: "COMPONENTS",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_LocationId",
                table: "EQUIPMENT",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENT_TypeId",
                table: "EQUIPMENT",
                column: "TypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_SUPPORT_RELATIONSHIPS_EquipmentId",
                table: "SERVICE_SUPPORT_RELATIONSHIPS",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_SUPPORT_RELATIONSHIPS_RedundantWithId",
                table: "SERVICE_SUPPORT_RELATIONSHIPS",
                column: "RedundantWithId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_SUPPORT_RELATIONSHIPS_ServiceId",
                table: "SERVICE_SUPPORT_RELATIONSHIPS",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_SUPPORT_RELATIONSHIPS_TypeId",
                table: "SERVICE_SUPPORT_RELATIONSHIPS",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CAPABILITY_SUPPORT_RELATIONSHIPS");

            migrationBuilder.DropTable(
                name: "COMPONENTS");

            migrationBuilder.DropTable(
                name: "SERVICE_SUPPORT_RELATIONSHIPS");

            migrationBuilder.DropTable(
                name: "CAPABILITIES");

            migrationBuilder.DropTable(
                name: "SERVICE_REDUNDANCIES");

            migrationBuilder.DropTable(
                name: "COMPONENT_RELATIONSHIPS");

            migrationBuilder.DropTable(
                name: "EQUIPMENT_REDUNDANCIES");

            migrationBuilder.DropTable(
                name: "SERVICES");

            migrationBuilder.DropTable(
                name: "COMPONENT_TYPES");

            migrationBuilder.DropTable(
                name: "EQUIPMENT");

            migrationBuilder.DropTable(
                name: "EQUIPMENT_TYPES");

            migrationBuilder.DropTable(
                name: "LOCATIONS");
        }
    }
}
