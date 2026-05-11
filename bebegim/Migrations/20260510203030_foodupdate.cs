using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bebegim.Migrations
{
    /// <inheritdoc />
    public partial class foodupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodHistories_Food_FoodId",
                table: "FoodHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Food",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Food");

            migrationBuilder.RenameTable(
                name: "Food",
                newName: "Foods");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "FoodHistories",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Detail",
                table: "FoodHistories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "FoodHistories",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KidId",
                table: "Foods",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Foods",
                table: "Foods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodHistories_Foods_FoodId",
                table: "FoodHistories",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodHistories_Foods_FoodId",
                table: "FoodHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Foods",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "FoodHistories");

            migrationBuilder.DropColumn(
                name: "Detail",
                table: "FoodHistories");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "FoodHistories");

            migrationBuilder.DropColumn(
                name: "KidId",
                table: "Foods");

            migrationBuilder.RenameTable(
                name: "Foods",
                newName: "Food");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Food",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Food",
                table: "Food",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodHistories_Food_FoodId",
                table: "FoodHistories",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
