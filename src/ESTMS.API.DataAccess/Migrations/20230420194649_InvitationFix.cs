using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESTMS.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InvitationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Players_ReceiverId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_TeamManagers_SenderId",
                table: "Invitations");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Invitations",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Invitations",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

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
                name: "FK_Invitations_AspNetUsers_ReceiverId",
                table: "Invitations",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_AspNetUsers_SenderId",
                table: "Invitations",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Players_PlayerId",
                table: "Invitations",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_ReceiverId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_SenderId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Players_PlayerId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_PlayerId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Invitations");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "Invitations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "Invitations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Players_ReceiverId",
                table: "Invitations",
                column: "ReceiverId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_TeamManagers_SenderId",
                table: "Invitations",
                column: "SenderId",
                principalTable: "TeamManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
