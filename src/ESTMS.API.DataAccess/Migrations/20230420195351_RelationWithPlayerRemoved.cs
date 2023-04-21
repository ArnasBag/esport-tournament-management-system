using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESTMS.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RelationWithPlayerRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Players_PlayerId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_PlayerId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Invitations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Invitations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_PlayerId",
                table: "Invitations",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Players_PlayerId",
                table: "Invitations",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
