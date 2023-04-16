using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESTMS.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_ReceiverId",
                table: "Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_SenderId",
                table: "Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_Team_TeamId",
                table: "Invitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Invitation",
                newName: "Invitations");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_TeamId",
                table: "Invitations",
                newName: "IX_Invitations_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_SenderId",
                table: "Invitations",
                newName: "IX_Invitations_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_ReceiverId",
                table: "Invitations",
                newName: "IX_Invitations_ReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations",
                column: "Id");

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
                name: "FK_Invitations_Teams_TeamId",
                table: "Invitations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                name: "FK_Invitations_Teams_TeamId",
                table: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "Invitation");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_TeamId",
                table: "Invitation",
                newName: "IX_Invitation_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_SenderId",
                table: "Invitation",
                newName: "IX_Invitation_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_ReceiverId",
                table: "Invitation",
                newName: "IX_Invitation_ReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_ReceiverId",
                table: "Invitation",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_SenderId",
                table: "Invitation",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_Team_TeamId",
                table: "Invitation",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
