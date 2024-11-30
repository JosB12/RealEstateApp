using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChatUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Chats");
        }
    }
}
