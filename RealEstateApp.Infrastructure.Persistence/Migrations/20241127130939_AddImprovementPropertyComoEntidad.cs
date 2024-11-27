using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImprovementPropertyComoEntidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImprovementProperty");

            migrationBuilder.CreateTable(
                name: "ImprovementProperties",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    ImprovementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImprovementProperties", x => new { x.PropertyId, x.ImprovementId });
                    table.ForeignKey(
                        name: "FK_ImprovementProperties_Improvements_ImprovementId",
                        column: x => x.ImprovementId,
                        principalTable: "Improvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImprovementProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImprovementProperties_ImprovementId",
                table: "ImprovementProperties",
                column: "ImprovementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImprovementProperties");

            migrationBuilder.CreateTable(
                name: "ImprovementProperty",
                columns: table => new
                {
                    ImprovementsId = table.Column<int>(type: "int", nullable: false),
                    PropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImprovementProperty", x => new { x.ImprovementsId, x.PropertiesId });
                    table.ForeignKey(
                        name: "FK_ImprovementProperty_Improvements_ImprovementsId",
                        column: x => x.ImprovementsId,
                        principalTable: "Improvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImprovementProperty_Properties_PropertiesId",
                        column: x => x.PropertiesId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImprovementProperty_PropertiesId",
                table: "ImprovementProperty",
                column: "PropertiesId");
        }
    }
}
