using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ESTMS.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakeTournamentManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Tournaments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TournamentManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentManagers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_ManagerId",
                table: "Tournaments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentManagers_ApplicationUserId",
                table: "TournamentManagers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_TournamentManagers_ManagerId",
                table: "Tournaments",
                column: "ManagerId",
                principalTable: "TournamentManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_TournamentManagers_ManagerId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "TournamentManagers");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_ManagerId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Tournaments");
        }
    }
}
