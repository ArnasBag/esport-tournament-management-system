using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESTMS.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakePlayerModelFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Ranks_RankId",
                table: "Players");

            migrationBuilder.AlterColumn<short>(
                name: "RankId",
                table: "Players",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "Players",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PicturePath",
                table: "Players",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AboutMeText",
                table: "Players",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Ranks_RankId",
                table: "Players",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Ranks_RankId",
                table: "Players");

            migrationBuilder.AlterColumn<short>(
                name: "RankId",
                table: "Players",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PicturePath",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutMeText",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Ranks_RankId",
                table: "Players",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
