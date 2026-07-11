using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clients_BusinessId_TelegramUserId",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "TelegramUserId",
                table: "clients");

            migrationBuilder.RenameColumn(
                name: "TelegramUserName",
                table: "clients",
                newName: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_clients_BusinessId_ExternalId",
                table: "clients",
                columns: new[] { "BusinessId", "ExternalId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clients_BusinessId_ExternalId",
                table: "clients");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "clients",
                newName: "TelegramUserName");

            migrationBuilder.AddColumn<long>(
                name: "TelegramUserId",
                table: "clients",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_clients_BusinessId_TelegramUserId",
                table: "clients",
                columns: new[] { "BusinessId", "TelegramUserId" },
                unique: true);
        }
    }
}
