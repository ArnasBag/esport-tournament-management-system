using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESTMS.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenTeamAndTeamManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamManagerId",
                table: "Teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamManagerId",
                table: "Teams",
                column: "TeamManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_TeamManagers_TeamManagerId",
                table: "Teams",
                column: "TeamManagerId",
                principalTable: "TeamManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_TeamManagers_TeamManagerId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TeamManagerId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamManagerId",
                table: "Teams");
        }
    }
}
