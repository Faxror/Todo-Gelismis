using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class mehmetişişlemişler1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tessts_TestId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TestId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AssignedPerson",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedPerson",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TestId",
                table: "Users",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tessts_TestId",
                table: "Users",
                column: "TestId",
                principalTable: "Tessts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
