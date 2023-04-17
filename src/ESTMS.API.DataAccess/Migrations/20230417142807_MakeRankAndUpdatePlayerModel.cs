using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ESTMS.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakeRankAndUpdatePlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutMeText",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "RankId",
                table: "Players",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Bronze" },
                    { (short)2, "Silver" },
                    { (short)3, "Gold" },
                    { (short)4, "Platinum" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_RankId",
                table: "Players",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Ranks_RankId",
                table: "Players",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Ranks_RankId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Players_RankId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "AboutMeText",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "Players");
        }
    }
}
